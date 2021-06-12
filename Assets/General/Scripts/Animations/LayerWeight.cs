using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeight : StateMachineBehaviour
{
    [Range(0f, 1f)]
    public float target = 1f;

    [SerializeField]
    float speed = 5f;
    public float Speed => speed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Process(animator, layerIndex, target);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        Process(animator, layerIndex, target);
    }

    void Process(Animator animator, int layer, float target)
    {
        var weight = animator.GetLayerWeight(layer);
        if (weight == target) return;

        weight = Mathf.MoveTowards(weight, target, speed * Time.deltaTime);
        animator.SetLayerWeight(layer, weight);
    }
}
