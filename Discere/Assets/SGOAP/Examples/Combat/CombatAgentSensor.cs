using System.Linq;
using UnityEngine;

namespace SGoap
{
    public class CombatAgentSensor : MonoBehaviour, IDataBind<AgentBasicData>
    {
        public FovTargetSensor FovTargetSystem;
        public CharacterStatusController Status;

        public StringReference InMeleeRangeState;
        public float MeleeRange = 2;
        public float FacingTargetDotProduct;

        public bool Guarding => _agentData.Animator.GetBool("Guarding");
        private AgentBasicData _agentData;
        public States States => _agentData.Agent.States;
        public Agent Agent => _agentData.Agent;

        private void Awake()
        {
            Status.OnDamageTaken += OnDamageTaken;
        }

        private void Update()
        {
            if (_agentData.Target == null)
                return;

            // The distance to the target is greater than your melee, out of range
            if (_agentData.DistanceToTarget < MeleeRange)
                States.SetState(InMeleeRangeState.Value, 1);
            else
                States.RemoveState(InMeleeRangeState.Value);
        }

        private void OnDamageTaken(int hp, int maxHp, IAttacker attacker)
        {
            FovTargetSystem.SetTarget(attacker as ITarget);
            FacingTargetDotProduct = (Vector3.Dot(_agentData.DirectionToTarget, _agentData.Agent.transform.forward) - 1) / 2;

            var staggerTime = 1.0f;
            if (FacingTargetDotProduct >= 0.5f && Guarding)
                staggerTime = 0.3f;

            _agentData.Cooldown.Run(staggerTime);

            SetSurviveGoalPriority(Random.Range(1, 10));
            Agent.AbortPlan();

            _agentData.Animator.SetTrigger("Impact");
            _agentData.Animator.SetFloat("MoveVelocity", 0);
        }

        public void SetSurviveGoalPriority(int number)
        {
            var surviveGoal = Agent.Goals.FirstOrDefault(x => x.Key == "Survive");
            surviveGoal.Priority = number;
        }

        public void Bind(AgentBasicData data)
        {
            _agentData = data;
        }
    }
}