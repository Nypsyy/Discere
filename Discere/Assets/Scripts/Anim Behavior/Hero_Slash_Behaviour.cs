using UnityEngine;

public class Hero_Slash_Behaviour : StateMachineBehaviour
{
    private float _blockedStart;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponentInParent<HeroAnim>().CurrentMode == Utils.HeroMode.BigSlash) {
            _blockedStart = 0.5f;
            animator.speed = 0;
        }
        else {
            _blockedStart = 0;
            animator.speed = 1;
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _blockedStart -= Time.deltaTime;
        
        if (_blockedStart <= 0)
            animator.speed = 1;
    }
}