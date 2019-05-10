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

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
	public class Player : Entity
	{
        public const short ControllerID = 1;

        new public Rigidbody rigidbody { get; protected set; }

        public PlayerMovement Movement { get; protected set; }
        public PlayerBody Body { get; protected set; }
        public PlayerAim Aim { get; protected set; }
        public interface IReference : References.Interface<Player>
        {

        }

        public RagdollController Ragdoll { get; protected set; }

        public Observer Observer { get; protected set; }
        public ObserverInput Input { get { return Observer.Input; } }

        public Level Level { get { return Level.Instance; } }
        public PlayersManager Manager { get { return Level.Players; } }

        public virtual void Init(Observer observer)
        {
            Manager.Add(this);

            this.Observer = observer;
            observer.Data.UpdateHealth(Health);

            rigidbody = GetComponent<Rigidbody>();

            Movement = Dependancy.Get<PlayerMovement>(gameObject);
            Body = Dependancy.Get<PlayerBody>(gameObject);
            Aim = Dependancy.Get<PlayerAim>(gameObject);

            Ragdoll = Dependancy.Get<RagdollController>(gameObject);
            Ragdoll.Disable();

            References.Init(this);

            Health.OnValueChanged += OnHealthChanged;
            Health.OnDamage += OnDamaged;
            Health.OnDeath += OnDeath;

            observer.DestroyEvent += OnObserverDestroyed;
        }

        void Update()
        {
            
        }

        void OnHealthChanged(float value)
        {
            Observer.Data.UpdateHealth(Health);
        }

        void OnDamaged(Entity damager, float value)
        {
            
        }

        void OnDeath(Entity damager)
        {
            Body.Animator.enabled = false;

            Ragdoll.Enable();
            Ragdoll.transform.parent = null;

            Destroy(gameObject);
        }

        void OnDestroy()
        {
            Manager.Remove(this);
        }

        void OnObserverDestroyed()
        {
            if (this == null) return;

            Destroy(gameObject);
        }
    }
}