using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartStateMachineBehaviour : StateMachineBehaviour
{
    public Animator Animator { get; protected set; }

    public int LayerIndex { get; protected set; }

    public float LayerWeight
    {
        get => Animator.GetLayerWeight(LayerIndex);
        set => Animator.SetLayerWeight(LayerIndex, value);
    }

    protected virtual void Prepare(Animator Animator, int LayerIndex)
    {
        if (this.Animator == Animator) return;

        Init(Animator, LayerIndex);
    }
    protected virtual void Init(Animator Animator, int LayerIndex)
    {
        this.Animator = Animator;
        this.LayerIndex = LayerIndex;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Prepare(animator, layerIndex);

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
}

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
