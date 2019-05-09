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
	public class WeaponRotatingBarrel : Weapon.Module, Weapon.IConstraint
	{
        [SerializeField]
        protected float acceleration = 0.3f;

        [SerializeField]
        protected float deAcceleration = 0.4f;

        float rate = 0f;
        public float Rate { get { return rate; } }

        [SerializeField]
        [Range(0f, 1f)]
        protected float activationRate = 0.25f;

        public bool Active { get { return rate < activationRate; } }

        [SerializeField]
        protected float rotationMultiplier = 20f;

        [SerializeField]
        protected Transform target;

        [SerializeField]
        protected Vector3 axis = Vector3.forward;

        void Reset()
        {
            target = transform;
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            weapon.ProcessEvent += Process;
            weapon.LateProcessEvent += LateProcess;
        }

        void Process(bool input)
        {
            if (input)
                rate = Mathf.MoveTowards(rate, 1f, acceleration * Time.deltaTime);
            else
                rate = Mathf.MoveTowards(rate, 0f, deAcceleration * Time.deltaTime);
        }

        void LateProcess(bool input)
        {
            target.Rotate(axis * rate * rotationMultiplier);
        }
	}
}