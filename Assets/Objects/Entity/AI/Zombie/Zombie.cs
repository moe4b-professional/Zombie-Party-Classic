using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Default
{
	public class Zombie : AI
	{
        public NavMeshAgent Agent { get; protected set; }

        public Animator Animator { get; protected set; }

        public AnimationTriggersRewind TriggersRewind { get; protected set; }

        public RagdollController Ragdoll { get; protected set; }

        public Player Target { get; protected set; }

        protected override void Start()
        {
            base.Start();

            Agent = GetComponent<NavMeshAgent>();

            Animator = Dependancy.Get<Animator>(gameObject);
            Animator.SetFloat("Cycle Offset", Random.value);

            TriggersRewind = Dependancy.Get<AnimationTriggersRewind>(gameObject);
            TriggersRewind.Add(AttackConnected, "Attack Connected");

            Ragdoll = Dependancy.Get<RagdollController>(gameObject);
            Ragdoll.Disable();

            StartCoroutine(Procedure());
        }

        protected virtual IEnumerator Procedure()
        {
            while (Level == null)
                yield return null;

            while(IsAlive)
            {
                if (Target != null && Target.IsDead)
                    Target = null;

                if (Target == null)
                    yield return Idle();
                else
                    yield return Chase();
            }
        }

        protected virtual IEnumerator Idle()
        {
            while (Target == null && IsAlive)
            {
                Target = LocateNearestPlayer();

                yield return new WaitForSeconds(Random.Range(0f, 0.5f));
            }
        }

        protected virtual IEnumerator Chase()
        {
            Agent.isStopped = false;

            while (IsAlive)
            {
                if (Target == null)
                {
                    Agent.isStopped = true;
                    break;
                }

                Agent.SetDestination(Target.transform.position);

                if (Vector3.Distance(transform.position, Target.transform.position) <= Agent.stoppingDistance)
                {
                    RotateTowardsTarget();

                    if (!isAttacking)
                        StartCoroutine(Attack());
                }

                yield return null;
            }
        }

        protected virtual void RotateTowardsTarget()
        {
            var direction = (Target.transform.position - transform.position).normalized;

            direction.y = 0f;

            transform.rotation = 
                Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Agent.angularSpeed * Time.deltaTime);
        }

        #region Attack
        [SerializeField]
        protected float damage = 35f;
        public float Damage { get { return damage; } }

        [SerializeField]
        protected CheckArea checkArea;
        public CheckArea CheckArea { get { return checkArea; } }

        [SerializeField]
        protected float attackDuration = 1.5f;
        public float AttackDuration { get { return attackDuration; } }

        bool isAttacking = false;

        IEnumerator Attack()
        {
            isAttacking = true;

            Animator.SetInteger("Attack Type", Random.Range(0, 2));
            Animator.SetTrigger("Attack");

            yield return new WaitForSeconds(attackDuration);

            isAttacking = false;
        }

        public virtual void AttackConnected()
        {
            var colliders = checkArea.Check();

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].attachedRigidbody == null) return;

                var entity = colliders[i].attachedRigidbody.GetComponent<Entity>();

                if (entity == null) continue;

                entity.TakeDamage(this, damage);

                if (Burn.Active && entity.Burn != null)
                    entity.Burn.Apply(this, EntityBurn.MaxValue * 2);
            }
        }
        #endregion

        protected override void Update()
        {
            base.Update();

            Animator.SetFloat("Move", Agent.velocity.magnitude);
        }

        protected override void Death(Entity Damager)
        {
            base.Death(Damager);

            Animator.enabled = false;

            Ragdoll.transform.parent = null;
            Ragdoll.Fallout(20f);

            Destroy(gameObject);
        }

        protected virtual Player LocateNearestPlayer()
        {
            int? index = null;
            float distance = Mathf.Infinity;

            for (int i = 0; i < Players.List.Count; i++)
            {
                if (Players.List[i].IsDead) continue;

                var newDisance = Vector3.Distance(transform.position, Players.List[i].transform.position);

                if (newDisance < distance)
                {
                    index = i;
                    distance = newDisance;
                }
            }

            if (index.HasValue)
                return Players.List[index.Value];
            else
                return null;
        }
    }
}