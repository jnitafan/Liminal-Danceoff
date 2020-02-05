using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Liminal.SDK.Build
{
    public class SettingsWindow : BaseWindowDrawer
    {
        private UnityEditor.Editor _currentConfigEditor;
        private ExperienceProfile _limappConfig;
        private Vector2 _scrollPos;

        public override void Draw(BuildWindowConfig config)
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUIHelper.DrawTitle("Experience Settings");
                EditorGUILayout.LabelField("This page is used to set the various settings of the experience");
                EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

                GetConfig();

                if (_limappConfig == null)
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Create New Config File"))
                    {
                        CreateConfig();
                    }

                    return;
                }

                DrawFields(_limappConfig, "Limapp Config");

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Update Config From Project Settings"))
                {
                    _limappConfig.SaveProjectSettings();
                    Debug.Log("Config Updated!");
                }
            }
        }

        private void GetConfig()
        {
            if (_limappConfig == null)
            {
                _limappConfig = AssetDatabase.LoadAssetAtPath<ExperienceProfile>($"{SDKResourcesConsts.LiminalSettingsConfigPath}");
            }
        }

        private void CreateConfig()
        {
            if (File.Exists($"{SDKResourcesConsts.LiminalSettingsConfigPath}"))
                return;

            LiminalSDKResources.InitialiseSettingsConfig();
            _limappConfig = AssetDatabase.LoadAssetAtPath<ExperienceProfile>($"{SDKResourcesConsts.LiminalSettingsConfigPath}");
        }

        public void DrawFields(ExperienceProfile profile, string name)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(name, GUILayout.Width(Screen.width * 0.2F));
            }
            EditorGUILayout.EndHorizontal();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            var tmpEditor = UnityEditor.Editor.CreateEditor(profile);

            if (_currentConfigEditor != null)
            {
                Object.DestroyImmediate(_currentConfigEditor);
            }

            _currentConfigEditor = tmpEditor;

            if (_currentConfigEditor != null && _limappConfig != null)
            {
                _currentConfigEditor.OnInspectorGUI();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
