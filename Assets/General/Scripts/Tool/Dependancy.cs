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
	public static class Dependancy
	{
        public static TComponent Get<TComponent>(GameObject target)
        {
            return Get<TComponent>(target, Scope.RecursiveToChildern);
        }
        public static TComponent Get<TComponent>(GameObject target, Scope scope)
        {
            TComponent component;

            component = target.GetComponent<TComponent>();

            if (IsNull(component))
            {
                if (scope == Scope.RecursiveToChildern)
                    for (int i = 0; i < target.transform.childCount; i++)
                    {
                        component = Get<TComponent>(target.transform.GetChild(i).gameObject, scope);

                        if (!IsNull(component)) break;
                    }

                if (scope == Scope.RecursiveToParents && target.transform.parent != null)
                    component = Get<TComponent>(target.transform.parent.gameObject, scope);
            }

            return component;
        }

        public static List<TComponent> GetAll<TComponent>(GameObject target)
        {
            return GetAll<TComponent>(target, Scope.RecursiveToChildern);
        }
		public static List<TComponent> GetAll<TComponent>(GameObject target, Scope scope)
        {
            var list = new List<TComponent>();

            list.AddRange(target.GetComponents<TComponent>());

            if (scope == Scope.RecursiveToChildern)
                for (int i = 0; i < target.transform.childCount; i++)
                    list.AddRange(GetAll<TComponent>(target.transform.GetChild(i).gameObject, scope));

            if (scope == Scope.RecursiveToParents)
                if (target.transform.parent != null)
                    list.AddRange(GetAll<TComponent>(target.transform.parent.gameObject, scope));

            return list;
        }

        public enum Scope
        {
            Local, RecursiveToChildern, RecursiveToParents
        }

        public static NullReferenceException FormatException(string dependancy, object dependant)
        {
            var text = "No " + dependancy + " found for " + dependant.GetType().Name;

            var componentDependant = dependant as Component;

            if (componentDependant != null)
                text += " On gameObject: " + componentDependant.gameObject;

            return new NullReferenceException(text);
        }
        public static string FormatExceptionText(string dependancy, object dependant)
        {
            var text = "No " + dependancy + " specified for " + dependant.GetType().Name;

            var componentDependant = dependant as Component;

            if (componentDependant != null)
                text += " On gameObject: " + componentDependant.gameObject;

            return text;
        }

        public static bool IsNull(object target)
        {
            if (target == null) return true;

            if (target.Equals(null)) return true;

            return false;
        }
    }
}