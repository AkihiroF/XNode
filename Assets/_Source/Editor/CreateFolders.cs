using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Source.Editor
{
    public class CreateFolders : EditorWindow
    {
        [MenuItem("Assets/Create Default Folders")]
        private static void SetUpFolders()
        {
            CreateFolders window = ScriptableObject.CreateInstance<CreateFolders>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
            window.ShowPopup();
        }

        private static void CreateFoldersBase(string basePath, List<string> folders)
        {
            foreach (var folder in folders)
            {
                string fullPath = basePath + folder;
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    Debug.Log($"Folder created: {fullPath}");
                }
            }
            AssetDatabase.Refresh();
        }

        private static void CreateUnitysFolders()
        {
            List<string> folders = new List<string>
            {
                "Animations",
                "Audio",
                "Editor",
                "Materials",
                "Meshes",
                "Prefabs",
                "Scripts",
                "Scenes",
                "Shaders",
                "Textures",
                "UI/Assets",
                "UI/Fonts",
                "UI/Icon"
            };

            CreateFoldersBase("Assets/", folders);
        }

        private static void CreateNastyaFolders()
        {
            List<string> mainFolders = new List<string>
            {
                "_Source",
                "_Presentation",
                "Third Party",
                "Editor",
                "Resources",
                "Plugins"
            };

            CreateFoldersBase("Assets/", mainFolders);

            List<string> presentationFolders = new List<string>
            {
                "Animations",
                "Audio",
                "Materials",
                "Meshes",
                "Prefabs",
                "Scenes",
                "Shaders",
                "Textures",
                "UI/Assets",
                "UI/Fonts",
                "UI/Icon"
            };

            CreateFoldersBase("Assets/_Presentation/", presentationFolders);
        }

        void OnGUI()
        {
            if (GUILayout.Button("Generate Unity Folders!"))
            {
                CreateUnitysFolders();
                this.Close();
            }
            if (GUILayout.Button("Generate Nastya Folders!"))
            {
                CreateNastyaFolders();
                this.Close();
            }
        }
    }
}
