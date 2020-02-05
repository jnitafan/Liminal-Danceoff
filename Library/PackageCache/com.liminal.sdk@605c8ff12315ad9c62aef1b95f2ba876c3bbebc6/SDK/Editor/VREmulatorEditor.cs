using Liminal.SDK.VR.Devices.Emulator;
using UnityEditor;
using UnityEngine;

namespace Liminal.SDK.VR.Emulator.Editor
{
    [CustomEditor(typeof(VREmulator))]
    public class VREmulatorEditor : UnityEditor.Editor
    {
        private static string[] _excludeProps = new[] { "m_Script", "m_EmulatorDevice" };

        private SerializedProperty m_EmulatorDevice;

        #region Editor

        private void OnEnable()
        {
            m_EmulatorDevice = serializedObject.FindProperty("m_EmulatorDevice");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_EmulatorDevice, new GUIContent("Device", m_EmulatorDevice.tooltip), includeChildren: true);
            DrawPlatformWarningForEmulatorDevice((VREmulatorDevice)m_EmulatorDevice.enumValueIndex);
            EditorGUILayout.Space();

            DrawPropertiesExcluding(serializedObject, _excludeProps);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        private void DrawPlatformWarningForEmulatorDevice(VREmulatorDevice device)
        {
            var buildTarget = GetBuildTargetForEmulatorDevice(device);
            if (buildTarget == BuildTargetGroup.Unknown)
                return;

            // Build targets match, we're ok...
            if (buildTarget == EditorUserBuildSettings.selectedBuildTargetGroup)
                return;

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                string.Format("This emulation device requires the build target to be set to '{0}'", buildTarget),
                MessageType.Warning
                );
        }

        private BuildTargetGroup GetBuildTargetForEmulatorDevice(VREmulatorDevice device)
        {
            switch (device)
            {
                case VREmulatorDevice.Daydream:
                case VREmulatorDevice.GearVR:
                    return BuildTargetGroup.Android;
                    
                default:
                    return BuildTargetGroup.Unknown;
            }
        }
    }
}
