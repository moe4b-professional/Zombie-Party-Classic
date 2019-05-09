using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimatorRandomBool : StateMachineBehaviour
{
    public string field;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        var value = Random.Range(0f, 100f) > 50f ? true : false;

        animator.SetBool(field, value);
    }
}