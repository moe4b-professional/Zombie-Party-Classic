using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MB;

public class LayerWeight : SmartStateMachineBehaviour
{
    [Range(0f, 1f)]
    public float target = 1f;

    [SerializeField]
    float speed = 5f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Process();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        Process();
    }

    void Process()
    {
        if (LayerWeight == target) return;

        LayerWeight = Mathf.MoveTowards(LayerWeight, target, speed * Time.deltaTime);
        Animator.SetLayerWeight(LayerIndex, LayerWeight);
    }
}