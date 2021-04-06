using System;
using System.Collections;
using Pathfinding;
using SGoap;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderAction : BasicAction
{
    public AIPath aiPath;
    public AIDestinationSetter aiDest;
    public Transform destPos;

    [Header("Itérations")]
    public int minIterations;

    public int maxIterations;

    [Header("Temps d'itération")]
    public float minIterationTime;

    public float maxIterationTime;

    [Header("Zone de déplacement")]
    public float maxWanderRadius;

    private int _iterations;
    private float _iterationTime;

    private bool _done;

    protected virtual IEnumerator WanderIteration() {
        destPos.position = AgentData.Agent.gameObject.transform.position + (Vector3) Random.insideUnitCircle * maxWanderRadius;
        yield return new WaitForSeconds(_iterationTime);
    }

    protected IEnumerator Wandering() {
        aiDest.target = destPos;

        for (var i = 0; i < _iterations; i++) {
            yield return WanderIteration();
        }

        _done = true;
        yield return null;
    }

    public override bool PrePerform() {
        _done = false;
        aiPath.canMove = true;
        _iterations = Random.Range(minIterations, maxIterations + 1);
        _iterationTime = Random.Range(minIterationTime, maxIterationTime);

        StartCoroutine(Wandering());
        return base.PrePerform();
    }

    public override EActionStatus Perform() {
        return _done ? EActionStatus.Success : EActionStatus.Running;
    }

    public override bool PostPerform() {
        aiPath.canMove = false;
        aiDest.target = FindObjectOfType<Hero>().gameObject.transform;
        // Wander goal reset
        AgentData.Agent.Goals[2].Priority = 1;
        AgentData.Agent.UpdateGoalOrderCache();
        return base.PostPerform();
    }
}