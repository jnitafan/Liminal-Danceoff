using Liminal.SDK.Tools;
using UnityEngine;

namespace Liminal.SDK.Build
{
    /// <summary>
    /// A helper place for setting up the scene
    /// </summary>
    public class SetupWindow : BaseWindowDrawer
    {
        public override void Draw(BuildWindowConfig config)
        {
            GUILayout.Label(
                "In order to build for the Liminal Platform, you need to set up the app scene" +
                "\nCurrently, we only support 1 Scene");

            EditorGUIHelper.DrawTitle("Scene Setup");

            GUILayout.Label("1. Open the scene you want to build" +
                            "\n2. Click Setup App Scene");

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Setup Scene"))
            {
                AppTools.SetupAppScene();
            }
        }
    }
}