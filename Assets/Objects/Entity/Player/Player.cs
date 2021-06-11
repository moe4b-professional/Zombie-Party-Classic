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

using UnityEngine.Networking;

namespace Default
{
    [RequireComponent(typeof(Rigidbody))]
	public class Player : Entity
	{
#pragma warning disable CS0109
        new public Rigidbody rigidbody { get; protected set; }
#pragma warning restore CS0109

        public PlayerVariants Variants { get; protected set; }
        public PlayerMovement Movement { get; protected set; }
        public PlayerBody Body { get; protected set; }
        public PlayerAim Aim { get; protected set; }
        public PlayerWeapons Weapons { get; protected set; }
        public PlayerScore Score { get; protected set; }

        public RagdollController Ragdoll { get; protected set; }

        public Observer Observer { get; protected set; }
        public Client Client { get { return Observer.Client; } }
        public ObserverInput Input { get { return Observer.Input; } }

        public Level Level { get { return Level.Instance; } }
        public PlayersManager Manager { get { return Level.Players; } }

        public virtual void Init(Observer observer)
        {
            Manager.Add(this);

            this.Observer = observer;
            observer.Data.UpdateHealth(Health);

            rigidbody = GetComponent<Rigidbody>();

            Variants = Dependancy.Get<PlayerVariants>(gameObject);

            Movement = Dependancy.Get<PlayerMovement>(gameObject);
            Body = Dependancy.Get<PlayerBody>(gameObject);
            Aim = Dependancy.Get<PlayerAim>(gameObject);
            Weapons = Dependancy.Get<PlayerWeapons>(gameObject);
            Score = Dependancy.Get<PlayerScore>(gameObject);

            References.Init(this);

            Variants.Init(this);

            Ragdoll = Dependancy.Get<RagdollController>(gameObject);
            Ragdoll.Disable();

            Health.OnValueChanged += OnHealthChanged;
        }

        void Update()
        {
            
        }

        void OnHealthChanged(float value)
        {
            Observer.Data.UpdateHealth(Health);
        }

        protected override void Death(Entity Damager)
        {
            base.Death(Damager);

            Body.Animator.enabled = false;

            Ragdoll.Enable();
            Ragdoll.transform.parent = null;

            Manager.Remove(this);

            Destroy(gameObject);
        }
    }

    public interface IPlayerReference : References.Interface<Player>
    {

    }
}