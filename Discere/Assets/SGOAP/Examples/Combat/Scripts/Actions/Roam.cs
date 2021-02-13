using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class ListExtensions
{
    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T GetRandom<T>(this IList<T> list, T ignore)
    {
        var newList = new List<T>(list);
        newList.Remove(ignore);
        return list[Random.Range(0, list.Count)];
    }
}

namespace SGoap
{
    public class Roam : BasicAction
    {
        public FovTargetSensor FovTargetSystem;

        public override float CooldownTime => 5;
        public override float StaggerTime => 0;

        public Transform[] Waypoints;
        public NavMeshAgent NavMeshAgent;

        private Transform _currentWaypoint;
        private float _speed;

        public override bool PrePerform()
        {
            _speed = NavMeshAgent.speed;
            NavMeshAgent.speed = 0.75f;
            return base.PrePerform();
        }

        public override EActionStatus Perform()
        {
            _currentWaypoint =_currentWaypoint ?? Waypoints.GetRandom();
            NavMeshAgent.SetDestination(_currentWaypoint.position);

            var dist = Vector3.Distance(_currentWaypoint.position, AgentData.Position);
            if (dist <= 1.0f)
                _currentWaypoint = Waypoints.GetRandom(_currentWaypoint);

            AgentData.Animator.SetFloat("MoveVelocity", 0.2f);

            return FovTargetSystem.HasTarget ? EActionStatus.Success : EActionStatus.Running;
        }

        public override bool PostPerform()
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.speed = _speed;
            AgentData.Animator.SetFloat("MoveVelocity", 0);
            States.SetState("hasTarget", 1);
            return base.PostPerform();
        }
    }
}