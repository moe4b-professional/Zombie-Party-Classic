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
    [Serializable]
	public class GameScene
	{
        [SerializeField]
        protected Object asset;
#if UNITY_EDITOR
        public Object Asset { get { return asset; } }
#endif

        [SerializeField]
        protected string name;
        public string Name
        {
            get
            {
#if UNITY_EDITOR
                var internalName = GetName(asset);

                if (internalName != name)
                {
                    Debug.LogWarning("Scene name mismatch, old name: (" + name + "), new name: (" + internalName + "). Please check scene fields");

                    name = internalName;
                }
#endif

                return name;
            }
        }

        public static implicit operator string(GameScene scene)
        {
            return scene.name;
        }

        public static string GetName(Object asset)
        {
            if (asset == null)
                return "";
            else
                return asset.name;
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(GameScene))]
        public class Drawer : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                SerializedProperty asset;
                asset = property.FindPropertyRelative(nameof(asset));

                SerializedProperty name;
                name = property.FindPropertyRelative(nameof(name));

                asset.objectReferenceValue = EditorGUI.ObjectField(position, property.displayName, asset.objectReferenceValue, typeof(SceneAsset), false);

                name.stringValue = GetName(asset.objectReferenceValue);
            }
        }
#endif
    }
}