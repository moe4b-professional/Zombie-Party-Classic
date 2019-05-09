using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeight : StateMachineBehaviour
{
    [Range(0f, 1f)]
    public float value = 1f;

    float initialValue;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        initialValue = animator.GetLayerWeight(layerIndex);

        animator.SetLayerWeight(layerIndex, value);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(layerIndex, initialValue);
    }
}
