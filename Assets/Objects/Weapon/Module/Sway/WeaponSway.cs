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
	public class WeaponSway : Weapon.Module
	{
        [SerializeField]
        protected SpeedData speed = new SpeedData(2f, 10f);
        [Serializable]
        public struct SpeedData
        {
            [SerializeField]
            float set;
            public float Set { get { return set; } }

            [SerializeField]
            float reset;
            public float Reset { get { return reset; } }

            public SpeedData(float set, float reset)
            {
                this.set = set;
                this.reset = reset;
            }
        }

        IList<Module> modules;
        protected virtual void InitModules()
        {
            modules = GetComponentsInChildren<Module>();

            for (int i = 0; i < modules.Count; i++)
                modules[i].Init(weapon, this);
        }

        public override void Init(Weapon weapon)
        {
            base.Init(weapon);

            InitModules();

            weapon.ProcessEvent += Process;
            weapon.LateProcessEvent += LateProcess;
        }

        void Process(bool input)
        {
            for (int i = 0; i < modules.Count; i++)
                modules[i].ApplyOffset(-offset);
        }

        protected virtual void LateProcess(bool input)
        {
            CalculateOffset(GetInput());

            for (int i = 0; i < modules.Count; i++)
                modules[i].ApplyOffset(offset);
        }

        protected virtual Vector2 GetInput()
        {
            if (enabled)
                return new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            else
                return Vector2.zero;
        }

        protected Vector2 offset;
        protected virtual void CalculateOffset(Vector2 input)
        {
            offset.x = ProcessAxis(offset.x, input.x);
            offset.y = ProcessAxis(offset.y, input.y);
        }

        public virtual float ProcessAxis(float value, float input)
        {
            value = Mathf.Lerp(value, 1f * Mathf.Sign(input), speed.Set * Time.deltaTime * Math.Abs(input));

            value = Mathf.Lerp(value, 0, Mathf.Lerp(0f, speed.Reset, Mathf.Abs(value)) * Time.deltaTime);

            return value;
        }

        public abstract class Module : MonoBehaviour
        {
            [SerializeField]
            protected Transform target;

            [SerializeField]
            protected MultiplierData multiplier;
            [Serializable]
            public struct MultiplierData
            {
                [SerializeField]
                float vertical;
                public float Vertical { get { return vertical; } }

                [SerializeField]
                float horizontal;
                public float Horizontal { get { return horizontal; } }

                public MultiplierData(float vertical, float horizontal)
                {
                    this.vertical = vertical;
                    this.horizontal = horizontal;
                }
            }

            protected Weapon weapon;
            protected WeaponSway sway;

            protected virtual void Reset()
            {
                target = transform;
            }

            protected virtual void Start()
            {

            }

            public virtual void Init(Weapon weapon, WeaponSway sway)
            {
                this.weapon = weapon;
                this.sway = sway;
            }

            public abstract void ApplyOffset(Vector3 offset);
        }
    }
}