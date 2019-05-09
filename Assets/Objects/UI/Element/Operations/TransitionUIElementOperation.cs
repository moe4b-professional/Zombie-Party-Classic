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
	public class TransitionUIElementOperation : Operation.Behaviour
    {
        public UIElement current;
        public UIElement target;

        protected virtual void Reset()
        {
            current = Dependancy.Get<UIElement>(gameObject, Dependancy.Scope.RecursiveToParents);
        }

        public override void Execute()
        {
            if (current == null)
                Debug.LogWarning(Dependancy.FormatExceptionText(nameof(current), this));
            else
                current.Hide();

            if (target == null)
                Debug.LogWarning(Dependancy.FormatExceptionText(nameof(target), this));
            else
                target.Show();
        }
    }
}