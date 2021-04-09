using UnityEngine;

public class Hero_Jump_Behaviour : StateMachineBehaviour
{
    public float yMax;
    
    private float _progress;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Ensures that when changing direction, we keep the jump progression
        animator.Play(0, layerIndex, _progress);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _progress = stateInfo.normalizedTime;
        
        if (_progress >= 1) {
            _progress = 0;
            animator.GetComponentInParent<HeroAnim>().SwitchMode(Utils.HeroMode.Move);
        }

        // amplitude = f(progress), such that f(0) = 0, f(0.5) = 1, f(1) = 0, using a quadratic polynomial 
        var amplitude = 4 * (1 - _progress) * _progress;
        animator.GetComponentInParent<Transform>().localPosition = new Vector3(0, yMax * amplitude, 0);
    }
}