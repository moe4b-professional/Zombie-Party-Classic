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
	public class ShowUIElementOperation : Operation.Behaviour
	{
        public UIElement target;

        protected virtual void Reset()
        {
            target = Dependancy.Get<UIElement>(gameObject, Dependancy.Scope.RecursiveToParents);
        }

        public override void Execute()
        {
            if (target == null)
                throw Dependancy.FormatException(nameof(target), this);

            target.Show();
        }
    }
}