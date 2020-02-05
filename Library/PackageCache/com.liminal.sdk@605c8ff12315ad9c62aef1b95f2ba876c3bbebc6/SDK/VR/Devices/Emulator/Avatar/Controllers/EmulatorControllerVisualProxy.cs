using Liminal.SDK.Extensions;
using Liminal.SDK.VR.Avatars.Controllers;
using Liminal.SDK.VR.Pointers;
using System.Collections.Generic;
using UnityEngine;

namespace Liminal.SDK.VR.Devices.Emulator.Avatar.Controllers
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [SelectionBase]
    public class EmulatorControllerVisualProxy : MonoBehaviour, IVRControllerVisual
    {
        private VREmulator mEmulator;
        private VREmulatorDevice mPreviewDevice;
        private VRControllerVisual mPreviewVisual;

        #region Properties

        public IVRPointerVisual PointerVisual
        {
            get
            {
                if (mPreviewVisual == null)
                    return null;

                return mPreviewVisual.PointerVisual;
            }
        }

        public IEnumerable<VRControllerNode> Nodes
        {
            get
            {
                if (mPreviewVisual == null)
                    return null;

                return mPreviewVisual.Nodes;
            }
        }

        public IEnumerable<VRControllerInputVisual> Inputs
        {
            get
            {
                if (mPreviewVisual == null)
                    return null;

                return mPreviewVisual.Inputs;
            }
        }

        #endregion

        #region MonoBehaviour

        private void OnDestroy()
        {
            DestroyPreview();
        }

        private void Update()
        {
            UpdatePreview();
        }

        #endregion

        public VRControllerNode GetNode(string nodeName)
        {
            if (mPreviewVisual == null)
                return null;

            return mPreviewVisual.GetNode(nodeName);
        }

        public VRControllerInputVisual GetInput(string inputName)
        {
            if (mPreviewVisual == null)
                return null;

            return mPreviewVisual.GetInput(inputName);
        }
        
        private void UpdatePreview()
        {
            if (Application.isPlaying)
            {
                DestroyPreview();
                return;
            }

            if (mEmulator == null)
                mEmulator = FindObjectOfType<VREmulator>();
            
            if (mEmulator == null)
                return;

            // Do we need to create a new preview?
            // Only if th device has changed, or there is currently no preview visible
            if (mEmulator.EmulatorDevice == mPreviewDevice && mPreviewVisual != null)
                return;

            DestroyPreview();

            mPreviewDevice = mEmulator.EmulatorDevice;
            mPreviewVisual = CreateEditorPreivew(mPreviewDevice);
            
            if (mPreviewVisual != null)
            {
                mPreviewVisual.transform.SetParentAndIdentity(transform);
                mPreviewVisual.gameObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.NotEditable;
                mPreviewVisual.gameObject.SetActive(true);
                
                // Hide any pointer visuals, because they can be annoying!
                foreach (var ptrVis in mPreviewVisual.GetComponentsInChildren<LaserPointerVisual>(true))
                {
                    ptrVis.gameObject.SetActive(false);
                }
            }
        }

        private void DestroyPreview()
        {
            if (mPreviewVisual != null)
            {
                DestroyImmediate(mPreviewVisual.gameObject);
                mPreviewVisual = null;
            }
        }

        private VRControllerVisual CreateEditorPreivew(VREmulatorDevice device)
        {
            switch (device)
            {
                case VREmulatorDevice.Daydream:
                    return DaydreamView.DaydreamEditorUtils.CreatePreviewControllerVisual();

                case VREmulatorDevice.GearVR:
                    return GearVR.GearVREditorUtils.CreatePreviewControllerVisual();
            }

            return null;
        }
    }
#endif
}
