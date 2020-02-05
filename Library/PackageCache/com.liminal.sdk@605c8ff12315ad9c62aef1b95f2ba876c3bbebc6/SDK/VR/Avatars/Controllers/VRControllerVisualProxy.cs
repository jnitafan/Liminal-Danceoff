using Liminal.SDK.Extensions;
using Liminal.SDK.VR.Pointers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Liminal.SDK.VR.Avatars.Controllers
{
    /// <summary>
    /// Creates a visual representing of a controller assigned to a specific limb, based on the connected device. This is useful for creating instances of a controller visual
    /// that is not connected to a tracked controller, or the VRAvatar (eg. A tutorial for the controller displayed in front of the user). In edit mode, a preview can be displayed
    /// based on the current emulation settings.
    /// </summary>
    [AddComponentMenu("VR/Avatar/Controller Visual Proxy")]
    [SelectionBase]
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class VRControllerVisualProxy : MonoBehaviour, IVRControllerVisual
    {
        private IVRControllerVisual mImpl;

        [Tooltip("The limb to instantiate the controller for. This is useful in cases where different limbs have different controller visuals.")]
        [SerializeField] private VRAvatarLimbAlias m_Limb = VRAvatarLimbAlias.PrimaryHand;
        [Tooltip("If checked, a preview of the controller will be displayed in the scene, based on the current emulator device on the VREmulator component of ExperienceApp.")]
        [SerializeField] private bool m_ShowPreview = true;

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="IVRPointerVisual"/> for the controller.
        /// </summary>
        public IVRPointerVisual PointerVisual
        {
            get
            {
                if (mImpl == null)
                    return null;

                return mImpl.PointerVisual;
            }
        }

        /// <summary>
        /// Gets the enumerable collection of controller nodes belonging to the visual.
        /// </summary>
        public IEnumerable<VRControllerNode> Nodes
        {
            get
            {
                if (mImpl == null)
                    return Enumerable.Empty<VRControllerNode>();

                return mImpl.Nodes;
            }
        }

        /// <summary>
        /// Gets the enumerable collection of controller input visuals belonging to the visual.
        /// </summary>
        public IEnumerable<VRControllerInputVisual> Inputs
        {
            get
            {
                if (mImpl == null)
                    return Enumerable.Empty<VRControllerInputVisual>();

                return mImpl.Inputs;
            }
        }

        public VRAvatarLimbAlias Limb
        {
            get { return m_Limb; }
            set
            {
                if (value == m_Limb)
                    return;

                m_Limb = value;
                RequestCreateImplementation();
#if UNITY_EDITOR
                UpdatePreviewState();
#endif
            }
        }

        /// <summary>
        /// Determines if the controller preview is displayed in the editor.
        /// </summary>
        public bool ShowPreview
        {
            get { return m_ShowPreview; }
            set
            {
#if UNITY_EDITOR
                if (value == m_ShowPreview)
                    return;

                m_ShowPreview = value;
                UpdatePreviewState();
#endif
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
#if UNITY_EDITOR
            UpdatePreviewState();

            if (!Application.isPlaying)
                return;
#endif
            RequestCreateImplementation();
        }

        private void OnDestroy()
        {
            VRAvatar.AvatarChanged -= OnVRAvatarChanged;
            if (mImpl != null)
            {
#if UNITY_EDITOR
                if (!ReferenceEquals(mImpl, mPreviewProxy))
                {
                    // Implementation is the proxy
                    Destroy(mImpl.gameObject);
                }
#else
                Destroy(mImpl.gameObject);
#endif
                mImpl = null;
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdatePreviewState();
        }
#endif

        #endregion

        /// <summary>
        /// Gets the the <see cref="VRControllerNode"/> with the name specified by <paramref name="nodeName"/>.
        /// </summary>
        /// <param name="nodeName">The name of the node.</param>
        /// <returns>The <see cref="VRControllerNode"/> with the name specified by <paramref name="nodeName"/>.</returns>
        public VRControllerNode GetNode(string nodeName)
        {
            if (mImpl == null)
                return null;

            return mImpl.GetNode(nodeName);
        }

        /// <summary>
        /// Gets the the <see cref="VRControllerInputVisual"/> with the name specified by <paramref name="inputName"/>.
        /// </summary>
        /// <param name="inputName">The name of the input component.</param>
        /// <returns>The <see cref="VRControllerInputVisual"/> with the name specified by <paramref name="inputName"/>.</returns>
        public VRControllerInputVisual GetInput(string inputName)
        {
            if (mImpl == null)
                return null;

            return mImpl.GetInput(inputName);
        }

        private void RequestCreateImplementation()
        {
            if (!Application.isPlaying)
                return;

            if (VRAvatar.Active == null)
            {
                // No Avatar is active yet, so listen for one becoming active
                VRAvatar.AvatarChanged -= OnVRAvatarChanged;
                VRAvatar.AvatarChanged += OnVRAvatarChanged;
            }
            else
            {
                CreateImplementation();
            }
        }

        private void CreateImplementation()
        {
            DestroyImplementation();

            var avatar = VRAvatar.Active;
            if (avatar == null)
                return;

            var limb = avatar.GetLimb(m_Limb);
            if (limb == null)
                return;

            mImpl = limb.InstantiateControllerVisual();
            if (mImpl == null)
                return;

            mImpl.transform.SetParentAndIdentity(transform);
        }

        private void DestroyImplementation()
        {
            if (mImpl != null)
            {
                Destroy(mImpl.gameObject);
                mImpl = null;
            }
        }

        #region Event Handlers

        private void OnVRAvatarChanged(IVRAvatar avatar)
        {
            CreateImplementation();
        }

        #endregion

        #region Preview
#if UNITY_EDITOR

        private Devices.Emulator.Avatar.Controllers.EmulatorControllerVisualProxy mPreviewProxy;

        private void UpdatePreviewState()
        {
            // No previews while running...
            if (Application.isPlaying)
                return;

            if (m_ShowPreview)
            {
                mPreviewProxy = gameObject.GetOrAddComponent<Devices.Emulator.Avatar.Controllers.EmulatorControllerVisualProxy>();
                mPreviewProxy.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                mImpl = mPreviewProxy;
            }
            else
            {
                // NOTE: Preview proxy and implmentation are the same, we only want to delete the *component*
                DestroyImmediate(mPreviewProxy);
                mPreviewProxy = null;
                mImpl = null;
            }
        }

#endif
        #endregion
    }
}
