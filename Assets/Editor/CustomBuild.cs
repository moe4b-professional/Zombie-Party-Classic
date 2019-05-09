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
	public static class CustomBuild
	{
        public static Core Core
        {
            get
            {
                return Resources.LoadAll<Core>("").First();
            }
        }

        public static ScenesCore Scenes { get { return Core.Scenes; } }

        [MenuItem("Build/Client/Android")]
        public static void BuildAndroidClient()
        {
            var scenes = new EditorBuildSettingsScene[]
            {
                GetScene(Scenes.MainMenu.Name),
                GetScene(Scenes.Level.Name),
                GetScene(Scenes.Client.Name),
            };

            var path = "Build/" + Application.productName + ".apk";

            var buildOptions = BuildOptions.ShowBuiltPlayer | BuildOptions.StrictMode;

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, buildOptions);
        }

        [MenuItem("Build/Client/Windows")]
        public static void BuildWindowsClient()
        {
            var scenes = new EditorBuildSettingsScene[]
            {
                GetScene(Scenes.MainMenu.Name),
                GetScene(Scenes.Level.Name),
                GetScene(Scenes.Client.Name),
            };

            var path = "Build/" + Application.productName + " (Client)/" + Application.productName + ".exe";

            var buildOptions = BuildOptions.ShowBuiltPlayer | BuildOptions.StrictMode;

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.StandaloneWindows, buildOptions);
        }

        [MenuItem("Build/Server")]
        public static void BuildServer()
        {
            var scenes = new EditorBuildSettingsScene[]
            {
                GetScene(Scenes.MainMenu.Name),
                GetScene(Scenes.Level.Name),
                GetScene(Scenes.Server.Name),
            };

            var path = "Build/" + Application.productName + " (Server)/" + Application.productName + ".exe";

            var buildOptions = BuildOptions.ShowBuiltPlayer | BuildOptions.StrictMode;

            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.StandaloneWindows, buildOptions);
        }

        public static EditorBuildSettingsScene GetScene(string name)
        {
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                if (EditorBuildSettings.scenes[i].path.Contains(name))
                    return EditorBuildSettings.scenes[i];

            throw new NotImplementedException();
        }
	}
}