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

namespace Game
{
	public class Weapon : MonoBehaviour
	{
        public WeaponState State { get; protected set; }

        public WeaponModel Model { get; protected set; }

        public Entity Owner { get; protected set; }
        public void Damage(Entity target, float damage)
        {
            target.TakeDamage(Owner, damage);
        }


        public void Init(Entity owner)
        {
            this.Owner = owner;

            State = WeaponState.Idle;

            Model = GetComponentInChildren<WeaponModel>();
            Model.Init();

            InitModules();

            InitConstraints();
        }


        IList<IModule> modules;
        public virtual T FindModule<T>()
            where T : IModule
        {
            for (int i = 0; i < modules.Count; i++)
                if (modules[i] is T)
                    return (T)modules[i];

            return default(T);
        }
        public interface IModule
        {
            void Init(Weapon weapon);
        }
        public abstract class Module : MonoBehaviour, IModule
        {
            protected Weapon weapon;

            public virtual void Init(Weapon weapon)
            {
                this.weapon = weapon;
            }

            protected virtual void Start()
            {

            }
        }
        public void InitModules()
        {
            modules = GetComponentsInChildren<IModule>();

            for (int i = 0; i < modules.Count; i++)
                modules[i].Init(this);
        }


        IList<IConstraint> constraints;
        public interface IConstraint
        {
            bool Active { get; }
        }
        public void InitConstraints()
        {
            constraints = GetComponentsInChildren<IConstraint>();
        }
        public bool HasActiveConstraints()
        {
            for (int i = 0; i < constraints.Count; i++)
                if (constraints[i].Active)
                    return true;

            return false;
        }


        public delegate void ProcessDelegate(bool input);
        public event ProcessDelegate ProcessEvent;
        public void Process(bool input)
        {
            if(HasActiveConstraints())
            {
                State = WeaponState.Idle;
            }
            else
            {
                if (input)
                    Action();
                else
                    State = WeaponState.Idle;
            }

            ProcessInput = input;

            if (ProcessEvent != null) ProcessEvent(input);
        }

        public event Action ActionEvent;
        protected void Action()
        {
            State = WeaponState.Action;

            if (ActionEvent != null) ActionEvent();
        }

        bool? ProcessInput;
        void LateUpdate()
        {
            if (ProcessInput.HasValue)
                LateProcess(ProcessInput.Value);
        }

        public event ProcessDelegate LateProcessEvent;
        public void LateProcess(bool input)
        {
            ProcessInput = null;

            if (LateProcessEvent != null) LateProcessEvent(input);
        }
	}

    public enum WeaponState
    {
        Idle, Action
    }
}