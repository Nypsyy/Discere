using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSword : MonoBehaviour
{
    
    private Animator animator;
    
    private Vector2[] position_corrections = {
        new Vector2(0, 0.3f), // right slash
        new Vector2(-0.5f, 0), // up slash
        new Vector2(0, -0.3f), // left slash
        new Vector2(0.5f, 0.2f)  // down slash
    };
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void TriggerSlash(Vector2 direction) {
        // get direction as angle
        float angle_deg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle_deg);
        // ensuring angle is in [0;360[
        angle_deg = (angle_deg + 360 + 67.5f) % 360;
        // we want to have 0=right, 1=down, 2=left, 3=up
        int dir = (int)(angle_deg / 90);
        // then we modify the sword direction
        transform.localEulerAngles = new Vector3(0, 0, 90*dir);
        // and we modify the position of the sword
        transform.localPosition = position_corrections[dir];
        // finally we trigger the animation
        animator.SetTrigger("Slash");
    }
    
    public void ResetSlash() {
    
    }
}
