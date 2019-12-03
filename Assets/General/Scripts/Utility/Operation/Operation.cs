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
	public static class Operation
	{
        public static void Execute(Interface operation)
        {
            operation.Execute();
        }

        public static void ExecuteAll(IList<Interface> operation)
        {
            for (int i = 0; i < operation.Count; i++)
                Execute(operation[i]);
        }

        public enum GameObjectExecutionScope
        {
            FirstOperation, EntireGameObject, RecursiveToChildern
        }
        public static void ExecuteIn(GameObject target, GameObjectExecutionScope scope)
        {
            switch (scope)
            {
                case GameObjectExecutionScope.FirstOperation:
                    var operation = target.GetComponent<Interface>();

                    if (operation != null)
                        Execute(operation);
                    break;

                case GameObjectExecutionScope.EntireGameObject:
                case GameObjectExecutionScope.RecursiveToChildern:
                    ExecuteAll(target.GetComponents<Interface>());

                    if (scope == GameObjectExecutionScope.RecursiveToChildern)
                        for (int i = 0; i < target.transform.childCount; i++)
                            ExecuteIn(target.transform.GetChild(i).gameObject, GameObjectExecutionScope.RecursiveToChildern);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public interface Interface
        {
            void Execute();
        }

        public abstract class Behaviour : MonoBehaviour, Interface
        {
            public abstract void Execute();
        }
	}
}