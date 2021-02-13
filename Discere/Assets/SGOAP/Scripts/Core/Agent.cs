using System.Collections.Generic;
using System.Linq;
using SGoap;
using UnityEngine;
using UnityEngine.Profiling;

namespace SGoap
{
    public class Agent : MonoBehaviour
    {
        public List<Goal> Goals;
        public PlannerSettings PlannerSettings;

        public States States = new States();
        public Queue<Action> StoredActionQueue = new Queue<Action>();
        private float _planStartTime;
        private bool _replan = true;
        private List<Goal> _orderedGoals;

        public Goal CurrentGoal { get; private set; }
        public Action CurrentAction { get; private set; }
        public List<Action> Actions { get; private set; }
        public Queue<Action> ActionQueue { get; private set; }

        public Planner Planner { get; } = new Planner();

        public virtual void Start()
        {
            var agentDependencies = GetComponentsInChildren<IDataBind<Agent>>(includeInactive: true);
            foreach (var dependency in agentDependencies)
                dependency.Bind(this);

            var stateMonitor = GetComponentInChildren<AgentStateMonitor>();
            if (stateMonitor != null)
            {
                foreach (var state in stateMonitor.PreStates)
                    States.AddState(state.Key, state.Value);
            }

            Actions = GetComponentsInChildren<Action>().ToList();

            _orderedGoals = Goals.OrderByDescending(entry => entry.Priority).ToList();

            if (!Application.isEditor)
            {
                PlannerSettings.GenerateFailedPlansReport = false;
                PlannerSettings.GenerateGoalReport = false;
            }
        }

        [ContextMenu("Update Goal Order")]
        public void UpdateGoalOrderCache()
        {
            _orderedGoals = Goals.OrderByDescending(entry => entry.Priority).ToList();
        }

        public virtual void LateUpdate()
        {
            if (CurrentAction != null && CurrentAction.Running)
            {
                if (CurrentAction.ShouldAbort())
                {
                    AbortPlan();
                    return;
                }
                else
                {
                    Profiler.BeginSample("Agent.Run");
                    
                    var actionStatus = CurrentAction.Perform();
                    CurrentAction.OnPerform?.Invoke();

                    if (actionStatus == EActionStatus.Failed)
                        AbortPlan();

                    if (actionStatus == EActionStatus.Success)
                    {
                        CurrentAction.PostPerform();
                        CurrentAction.OnPostPerform?.Invoke();
                        CurrentAction.Running = false;
                    }

                    Profiler.EndSample();
                }

                Profiler.BeginSample("Agent.CanAbortPlan");

                if (PlannerSettings.CanAbortPlans && CurrentAction.CanAbort() && Time.time - _planStartTime > PlannerSettings.PlanRate)
                    AbortPlan();

                Profiler.EndSample();
                return;
            }

            if (_replan || ActionQueue == null)
            {
                foreach (var goal in _orderedGoals)
                {
                    Profiler.BeginSample("Agent.Plan");
                    ActionQueue = Planner.Plan(this, goal.Key, World.Instance.StateMap,Actions, States.GetStates(), false);
                    _planStartTime = Time.time;
                    Profiler.EndSample();

                    if (ActionQueue == null)
                        continue;

                    CurrentGoal = goal;
                    _replan = false;

                    StoredActionQueue.Clear();
                    foreach (var action in ActionQueue)
                        StoredActionQueue.Enqueue(action);

                    break;
                }
            }
            
            // Goal reached!
            if (ActionQueue != null && ActionQueue.Count == 0)
            {
                if (CurrentGoal.Once)
                {
                    Goals.Remove(CurrentGoal);
                    _orderedGoals.Remove(CurrentGoal);
                }

                _replan = true;
            }

            // Try do Action
            Profiler.BeginSample("Agent.PrePerform");
            if (ActionQueue != null && ActionQueue.Count > 0)
            {
                CurrentAction = ActionQueue.Dequeue();

                if (CurrentAction.PrePerform())
                {
                    if(CurrentAction.TrackStopWatch)
                        CurrentAction.Stopwatch.Restart();
                    else
                        CurrentAction.Stopwatch.Reset();

                    CurrentAction.Running = true;
                    CurrentAction.OnPrePerform?.Invoke();
                }
                else
                    AbortPlan();
            }
            Profiler.EndSample();
        }

        public void ForceReplan()
        {
            _replan = true;
            AbortPlan();
        }

        public void AbortPlan()
        {
            Debug.Log("Replanning");

            if (CurrentAction != null)
            {
                CurrentAction.OnFailed();
                CurrentAction.Running = false;
            }

            ActionQueue = null;
            StoredActionQueue.Clear();
            CurrentAction = null;
        }
    }
}
