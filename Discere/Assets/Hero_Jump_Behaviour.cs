using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Jump_Behaviour : StateMachineBehaviour
{
    public float yMax;

    private float progress = 0;
    
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ensures that when changing direction, we keep the jump progression
        animator.Play(0, layerIndex, progress);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        progress = stateInfo.normalizedTime;
        if (progress >= 1) {
            progress = 0;
            animator.GetComponentInParent<HeroAnim>().SwitchMode(HeroAnim.Mode.Move);
        }
        // amplitude = f(progress), such that f(0) = 0, f(0.5) = 1, f(1) = 0, using a quadratic polynomial 
        float amplitude = 4 * (1-progress) * progress;
        animator.GetComponentInParent<Transform>().localPosition = new Vector3(0, yMax * amplitude, 0);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    progress = 0;    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
