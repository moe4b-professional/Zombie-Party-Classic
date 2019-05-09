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
	public class ExecutionRewindOperation : Operation.Behaviour
	{
        [SerializeField]
        protected float delay = 0f;
        public float Delay
        {
            get
            {
                return delay;
            }
            set
            {
                if (value < 0f)
                    value = 0f;

                delay = value;
            }
        }

        public GameObject target;

        public Operation.GameObjectExecutionScope executionScope = Operation.GameObjectExecutionScope.FirstOperation;

        public override void Execute()
        {
            if (target == null)
                throw Dependancy.FormatException(nameof(target), this);

            StartCoroutine(Procedure());
        }

        protected virtual IEnumerator Procedure()
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            Operation.ExecuteIn(target, executionScope);
        }
    }
}