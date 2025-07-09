using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _0.DucLib.Scripts.Editor
{
    public class CustomBuildWindowVer2 : EditorWindow
    {
        private string versionName = "";
        private int versionCode = 0;
        private string pass;
        private bool withDeveloperBuild;
        
        private List<BuildSymbol> buildSymbols = new List<BuildSymbol>
        {
            new BuildSymbol("ENABLE_CHEAT", "Cheat Mode"),
            new BuildSymbol("IGNORE_ADS", "Ignore Ads"),
            new BuildSymbol("USE_ADMOB_NATIVE", "Use Admob Native"),
            new BuildSymbol("USE_ADMOB_MEDIATION", "Use Admob Mediation"),
            new BuildSymbol("USE_MAX_MEDIATION", "Use Max Mediation"),
        };

        [MenuItem("CUSTOM BUILD/CUSTOM BUILD SETTINGS VER 2  %#&C")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CustomBuildWindowVer2));
        }

        private void OnGUI()
        {
            DrawVersionSettings();
            DrawPassSetting();
            DrawBuildSymbols();
            DrawDeveloperBuild();
            DrawBuildButtons();
        }

        private void DrawVersionSettings()
        {
            EditorGUILayout.BeginHorizontal();
            versionName = EditorPrefs.GetString($"{Application.productName}_gameVersion", Application.version);
            versionName = EditorGUILayout.TextField("Version", versionName);
            EditorPrefs.SetString($"{Application.productName}_gameVersion", versionName);
            if (GUILayout.Button("+", GUILayout.Width(50))) IncVersionName();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            versionCode = EditorPrefs.GetInt($"{Application.productName}_VersionCode",
                PlayerSettings.Android.bundleVersionCode);
            versionCode = int.Parse(EditorGUILayout.TextField("Version Code", versionCode.ToString()));
            EditorPrefs.SetInt($"{Application.productName}_VersionCode", versionCode);
            if (GUILayout.Button("+", GUILayout.Width(50))) IncVersionCode();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPassSetting()
        {
            EditorGUILayout.BeginHorizontal();
            pass = EditorPrefs.GetString($"{Application.productName}_PassBuild", "");
            pass = EditorGUILayout.TextField("PassBuild", pass);
            EditorPrefs.SetString($"{Application.productName}_PassBuild", pass);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawBuildSymbols()
        {
            foreach (var symbol in buildSymbols)
            {
                EditorGUILayout.BeginHorizontal();

                symbol.IsEnabled = HasSymbol(symbol.Key);
                GUIStyle labelStyle = GetTextStyle(symbol.IsEnabled);
                string statusText = symbol.IsEnabled ? "[Enabled]" : "[Disabled]";


                GUIContent icon = GetIcon(symbol.IsEnabled);

                EditorGUILayout.LabelField(icon, GUILayout.Width(20));
                EditorGUILayout.LabelField($"{symbol.DisplayName} {statusText}", labelStyle);

                GUILayout.FlexibleSpace();

                string buttonText = symbol.IsEnabled ? "Disable" : "Enable";
                if (GUILayout.Button(buttonText, GUILayout.Width(100)))
                {
                    ToggleSymbol(symbol);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawDeveloperBuild()
        {
            EditorGUILayout.BeginHorizontal();

            withDeveloperBuild = EditorPrefs.GetInt("withDeveloperBuild", 1) == 1;
            GUIStyle devStyle = GetTextStyle(withDeveloperBuild);
            string statusText = withDeveloperBuild ? "[Enabled]" : "[Disabled]";

            GUIContent icon = GetIcon(withDeveloperBuild);

            EditorGUILayout.LabelField(icon, GUILayout.Width(20)); 
            EditorGUILayout.LabelField($"Developer Build {statusText}", devStyle);

            GUILayout.FlexibleSpace();

            string buttonText = withDeveloperBuild ? "Disable" : "Enable";
            if (GUILayout.Button(buttonText, GUILayout.Width(100)))
            {
                withDeveloperBuild = !withDeveloperBuild;
                EditorPrefs.SetInt("withDeveloperBuild", withDeveloperBuild ? 1 : 0);
            }

            EditorGUILayout.EndHorizontal();
        }

        private GUIContent GetIcon(bool enabled)
        {
            return EditorGUIUtility.IconContent(enabled ? "TestPassed" : "TestFailed");
        }
        private GUIStyle GetTextStyle(bool isEnabled)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = !isEnabled ? Color.green : Color.yellow;
            style.fontStyle = FontStyle.Bold;
            return style;
        }


        private void DrawBuildButtons()
        {
            if (EditorApplication.isCompiling) return;
            if (GUILayout.Button("Build Apk")) BuildAPK();
            if (GUILayout.Button("Build Android App Bundle")) BuildAAB();
            if (GUILayout.Button("Build And Run Apk")) ConfirmBuild(BuildAPK);
            if (GUILayout.Button("Build And Run Aab")) ConfirmBuild(BuildAAB);
        }

        private void ToggleSymbol(BuildSymbol symbol)
        {
            if (symbol.Key == "") return;
            if (symbol.IsEnabled)
                RemoveSymbol(symbol.Key);
            else
                AddSymbol(symbol.Key);
        }


        private static bool HasSymbol(string symbol)
        {
            BuildTargetGroup btg = GetBuildTargetGroup();
            string[] define;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(btg, out define);
            return define.Contains(symbol);
        }

        private static void AddSymbol(string symbol)
        {
            BuildTargetGroup btg = GetBuildTargetGroup();
            string[] define;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(btg, out define);
            var listDefine = define.ToList();

            if (!listDefine.Contains(symbol))
            {
                Debug.Log($"Add Script Symbol: {symbol}");
                listDefine.Add(symbol);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, listDefine.ToArray());
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
        }

        private static void RemoveSymbol(string symbol)
        {
            BuildTargetGroup btg = GetBuildTargetGroup();
            string[] define;
            PlayerSettings.GetScriptingDefineSymbolsForGroup(btg, out define);
            var listDefine = define.ToList();

            if (listDefine.Contains(symbol))
            {
                listDefine.Remove(symbol);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, listDefine.ToArray());
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }
        }

        private void ConfirmBuild(System.Action<BuildOptions> buildMethod)
        {
            var build = EditorUtility.DisplayDialog("Build And Run", "Please Make Sure device is connected", "Ok",
                "Cancel");
            if (build)
            {
                buildMethod(BuildOptions.AutoRunPlayer);
            }
        }

        private static BuildTargetGroup GetBuildTargetGroup()
        {
#if UNITY_IOS
        return BuildTargetGroup.iOS;
#else
            return BuildTargetGroup.Android;
#endif
        }

        private void IncVersionName()
        {
            string[] versionSplit = versionName.Split('.');
            if (versionSplit.Length == 3)
            {
                int nextPatchVer = int.Parse(versionSplit[2]) + 1;
                versionName = $"{versionSplit[0]}.{versionSplit[1]}.{nextPatchVer}";
            }
            else
            {
                int nextPatchVer = int.Parse(versionName) + 1;
                versionName = nextPatchVer.ToString();
            }

            EditorPrefs.SetString($"{Application.productName}_gameVersion", versionName);
        }

        private void IncVersionCode()
        {
            versionCode++;
            EditorPrefs.SetInt($"{Application.productName}_VersionCode", versionCode);
        }

        #region Build

        private void BuildAAB(BuildOptions option = BuildOptions.None)
        {
            PlayerSettings.bundleVersion = versionName;
            PlayerSettings.Android.bundleVersionCode = versionCode;

            PlayerSettings.keystorePass = pass;
            PlayerSettings.keyaliasPass = pass;
            EditorUserBuildSettings.buildAppBundle = true;
            List<string> enabledSymbols = new List<string>();

            string toggleFlash = "ALPHA";
            foreach (var tg in buildSymbols)
            {
                if (tg.IsEnabled)
                {
                    enabledSymbols.Add(tg.Key);
                }
            }

            if (enabledSymbols.Count > 0)
            {
                toggleFlash = "BETA";
            }

            var fileName =
                $"{Application.productName}_ver{Application.version}_bundle{PlayerSettings.Android.bundleVersionCode}_{toggleFlash}";
            fileName = fileName.Replace(" ", "").Replace(":", "");
            var ext = "aab";

            string productName = Application.productName;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                productName = productName.Replace(c, '_');
            }

            var customDirectory = $@"C:\Users\Admin\Documents\{productName}\{PlayerSettings.Android.bundleVersionCode}";
            if (!Directory.Exists(customDirectory))
            {
                Directory.CreateDirectory(customDirectory);
            }

            if (withDeveloperBuild)
            {
                option |= BuildOptions.Development;
            }

            var path = EditorUtility.SaveFilePanel("Save Location", customDirectory, fileName, ext);
            if (string.IsNullOrEmpty(path)) return;
            BuildPipeline.BuildPlayer(GetScenePaths(), path, BuildTarget.Android, option);
        }

        private void BuildAPK(BuildOptions option = BuildOptions.None)
        {
            PlayerSettings.keystorePass = pass;
            PlayerSettings.keyaliasPass = pass;
            PlayerSettings.bundleVersion = versionName;
            PlayerSettings.Android.bundleVersionCode = versionCode;
            EditorUserBuildSettings.buildAppBundle = false;
            List<string> enabledSymbols = new List<string>();

            string toggleFlash = "ALPHA";
            foreach (var tg in buildSymbols)
            {
                if (tg.IsEnabled)
                {
                    enabledSymbols.Add(tg.Key);
                }
            }

            if (enabledSymbols.Count > 0)
            {
                toggleFlash = "BETA";
            }

            var fileName =
                $"{Application.productName}_ver{Application.version}_bundle{PlayerSettings.Android.bundleVersionCode}_{toggleFlash}";
            fileName = fileName.Replace(" ", "").Replace(":", "");
            var ext = "apk";

            string productName = Application.productName;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                productName = productName.Replace(c, '_');
            }

            var customDirectory = $@"C:\Users\Admin\Documents\{productName}\{PlayerSettings.Android.bundleVersionCode}";
            if (!Directory.Exists(customDirectory))
            {
                Directory.CreateDirectory(customDirectory);
            }

            if (withDeveloperBuild)
            {
                option |= BuildOptions.Development;
            }

            var path = EditorUtility.SaveFilePanel("Save Location", customDirectory, fileName, ext);
            if (string.IsNullOrEmpty(path)) return;
            BuildPipeline.BuildPlayer(GetScenePaths(), path, BuildTarget.Android, option);
        }

        static string[] GetScenePaths()
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }

            return scenes;
        }

        #endregion
    }

    public class BuildSymbol
    {
        public string Key;
        public string DisplayName;
        public bool IsEnabled;

        public BuildSymbol(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
            IsEnabled = false;
        }
    }
}