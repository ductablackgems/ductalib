using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _0.DucLib.Scripts.Editor
{
    public class LoadSceneTool
    {
        
        [MenuItem("Tools/Load Splash")]
        public static void Load1()
        {
            OpenScene("Splash");
        }
        [MenuItem("Tools/Load Menu")]
        public static void Load2()
        {
            OpenScene("MainMenu");
        }
        [MenuItem("Tools/Load Game")]
        public static void Load6()
        {
            OpenScene("GamePlay");
        }
        
        private static void OpenScene(string sceneName)
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    string currentSceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);

                    if (currentSceneName == sceneName)
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scene.path);
                            return;
                        }
                    }
                }
            }
        }
    }
}