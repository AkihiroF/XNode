using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace _Source.Editor
{
    public static class BuildProcessTool
    {
        private static List<string> _scenesToBuild = new List<string>();
        private static BuildTarget _buildTarget = BuildTarget.StandaloneWindows;

        [MenuItem("File/Fast Build")]
        public static void ShowCustomBuildWindow()
        {
            EditorWindow.GetWindow(typeof(CustomBuildWindow), true, "Custom Build Settings");
        }

        public class CustomBuildWindow : EditorWindow
        {
            private Vector2 scrollPos;

            void OnGUI()
            {
                GUILayout.Label("Build Settings", EditorStyles.boldLabel);

                _buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", _buildTarget);

                EditorGUILayout.Space();

                GUILayout.Label("Scenes to Include in Build:");
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                foreach (var scene in EditorBuildSettings.scenes)
                {
                    bool isSceneIncluded = _scenesToBuild.Contains(scene.path);
                    bool shouldIncludeScene = EditorGUILayout.ToggleLeft(scene.path, isSceneIncluded);
                    if (shouldIncludeScene && !isSceneIncluded)
                    {
                        _scenesToBuild.Add(scene.path);
                    }
                    else if (!shouldIncludeScene && isSceneIncluded)
                    {
                        _scenesToBuild.Remove(scene.path);
                    }
                }
                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space();

                if (GUILayout.Button("Build"))
                {
                    BuildGame();
                    Close();
                }
            }
        }

        private static void BuildGame()
        {
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

            if (path.Length <= 0)
                return;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = _scenesToBuild.ToArray(),
                locationPathName = path + "/Build" + GetExtensionForTarget(_buildTarget),
                target = _buildTarget,
                options = BuildOptions.None
            };

            Build(buildPlayerOptions);
        }

        private static void Build(BuildPlayerOptions options)
        {
            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder(options.locationPathName);
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError("Build failed");
            }
        }

        private static string GetExtensionForTarget(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneOSX:
                    return ".app";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return ".exe";
                // Додайте інші платформи тут
                default:
                    return "";
            }
        }
    }
}
