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
	public static class DisableCollisions
	{
		public static void Do(GameObject gameobject1, GameObject gameobject2)
        {
            var colliders1 = Dependancy.GetAll<Collider>(gameobject1);

            var colliders2 = Dependancy.GetAll<Collider>(gameobject2);

            for (int x = 0; x < colliders1.Count; x++)
                for (int y = 0; y < colliders2.Count; y++)
                    Physics.IgnoreCollision(colliders1[x], colliders2[y], true);
        }
	}
}