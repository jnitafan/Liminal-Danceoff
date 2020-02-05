using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Liminal.SDK.Build
{
    /// <summary>
    /// A window for developers to configure and build for Limapps
    /// </summary>
    public class BuildSettingsWindow : EditorWindow
    {
        private const int _width = 500;
        private const int _height = 300;

        public static EditorWindow Window;
        public static Dictionary<BuildSettingMenus, BaseWindowDrawer> BuildSettingLookup = new Dictionary<BuildSettingMenus, BaseWindowDrawer>();

        private BuildSettingMenus _selectedMenu = BuildSettingMenus.Setup;
        private BuildWindowConfig _windowConfig = new BuildWindowConfig();

        public int SelectedMenuIndex { get { return (int)_selectedMenu; } }

        [MenuItem("Liminal/Build Window")]
        public static void OpenBuildWindow()
        {
            Window = GetWindow(typeof(BuildSettingsWindow), true, "Build Settings");

            Window.minSize = new Vector2(_width, _height);

            Window.Show();
        }

        [MenuItem("Liminal/Update Package")]
        public static void RefreshPackage()
        {
            File.WriteAllText(UnityPackageManagerUtils.ManifestPath, UnityPackageManagerUtils.ManifestWithoutLock);
            AssetDatabase.Refresh();
        }

        private void OnEnable()
        {
            var fileExists = Directory.Exists(BuildWindowConsts.ConfigFolderPath) || File.Exists(BuildWindowConsts.BuildWindowConfigPath);

            if (fileExists)
            {
                var json = File.ReadAllText(BuildWindowConsts.BuildWindowConfigPath);
                _windowConfig = JsonUtility.FromJson<BuildWindowConfig>(json);
                AssetDatabase.Refresh();
            }

            SetupFolderPaths();
            SetupPreviewScene();

            SetupMenuWindows();
        }

        private void OnGUI()
        {
            var tabs = Enum.GetNames(typeof(BuildSettingMenus));
            _selectedMenu = (BuildSettingMenus) GUILayout.Toolbar(SelectedMenuIndex, tabs);

            var activeWindow = BuildSettingLookup[_selectedMenu];
            activeWindow.Draw(_windowConfig);

            if (!Directory.Exists(BuildWindowConsts.ConfigFolderPath))
            {
                Directory.CreateDirectory(BuildWindowConsts.ConfigFolderPath);
            }

            // boolean true is used to format the resulting string for maximum readability. False would format it for minimum size.
            var configJson = JsonUtility.ToJson(_windowConfig, true);
            File.WriteAllText(BuildWindowConsts.BuildWindowConfigPath, configJson);
        }

        private void SetupMenuWindows()
        {
            BuildSettingLookup.AddSafe(BuildSettingMenus.Build, new BuildWindow());
            BuildSettingLookup.AddSafe(BuildSettingMenus.Publishing, new PublishConfigurationWindow());
            BuildSettingLookup.AddSafe(BuildSettingMenus.Setup, new SetupWindow());
            BuildSettingLookup.AddSafe(BuildSettingMenus.Preview, new AppPreviewWindow());
            BuildSettingLookup.AddSafe(BuildSettingMenus.Settings, new SettingsWindow());
        }

        private void SetupFolderPaths()
        {
            if (!Directory.Exists(BuildWindowConsts.PlatformSceneFolderPath))
            {
                Directory.CreateDirectory(BuildWindowConsts.PlatformSceneFolderPath);
            }

            if (!Directory.Exists(BuildWindowConsts.BuildPath))
            {
                Directory.CreateDirectory(BuildWindowConsts.BuildPath);
            }

            AssetDatabase.Refresh();
        }

        private void SetupPreviewScene()
        {
            var sceneExists = File.Exists(BuildWindowConsts.PreviewAppScenePath);
            if (!sceneExists)
            {
                var scenePath = $"{UnityPackageManagerUtils.FullPackageLocation}/{BuildWindowConsts.PackagePreviewAppScenePath}";
                Debug.Log(scenePath);
                File.Copy(scenePath, BuildWindowConsts.PreviewAppScenePath);
                AssetDatabase.Refresh();
            }
        }
    }
}
