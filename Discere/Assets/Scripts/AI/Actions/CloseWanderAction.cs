using System.Collections;
using UnityEngine;

public class CloseWanderAction : WanderAction
{
    public float idleTime;

    protected override IEnumerator WanderIteration() {
        yield return base.WanderIteration();
        aiPath.canMove = false;
        yield return new WaitForSeconds(idleTime);
        aiPath.canMove = true;
        yield return null;
    }
}