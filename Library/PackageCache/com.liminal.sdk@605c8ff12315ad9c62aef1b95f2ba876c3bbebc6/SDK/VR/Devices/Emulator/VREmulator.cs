using Liminal.SDK.Core;
using Liminal.SDK.VR.Devices.Emulator;
using System;
using UnityEngine;

namespace Liminal.SDK.VR
{
    /// <summary>
    /// The entry component for initializing the <see cref="VRDevice"/> system using emulation for the editor.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("VR/Emulator Setup")]
    public class VREmulator : MonoBehaviour, IVRDeviceInitializer
    {
        [Tooltip("The device to emulate during development. NOTE: You may need to change Player Settings to correctly emulate some devices.")]
        [SerializeField] private VREmulatorDevice m_EmulatorDevice = VREmulatorDevice.Daydream;
        [Tooltip("When building an app directly to a device for testing, you can specify which device will be used. Leave as 'None' to have the emulator attempt to automatically detect the device.")]
        [SerializeField] private VREmulatorDevice m_BuildDevice = VREmulatorDevice.GearVR;

        #region Properties

        /// <summary>
        /// Gets the device type the emulator will attempt emulate.
        /// </summary>
        public VREmulatorDevice EmulatorDevice
        {
            get { return m_EmulatorDevice; }
        }

        /// <summary>
        /// Gets or sets the device type that will be used when the application is built.
        /// </summary>
        public VREmulatorDevice BuildDevice
        {
            get { return m_BuildDevice; }
            set { m_BuildDevice = value; }
        }

        #endregion
        
        /// <summary>
        /// Creates a new <see cref="IVRDevice"/> and returns it.
        /// </summary>
        /// <returns>The <see cref="IVRDevice"/> that was created.</returns>
        public IVRDevice CreateDevice()
        {
            if (!ExperienceApp.IsEmulator)
                throw new InvalidOperationException("ExperienceApp must be in Emulation mode to create an emulator device.");

#if UNITY_EDITOR
            // Always use the emulator device in the editor
            return new EmulatorDevice(m_EmulatorDevice);
#else
            // A BuildDevice value of NONE means we should auto-detect the device
            if (m_BuildDevice == VREmulatorDevice.None)
                return AutoCreateDevice();

            // A specific BuildDevice was specified
            switch (m_BuildDevice)
            {
                case VREmulatorDevice.Daydream:
                    return new Devices.DaydreamView.DaydreamViewDevice();

                case VREmulatorDevice.GearVR:
                    return new Devices.GearVR.GearVRDevice();

                default:
                    Debug.Log("[VREmulator] Unsupported build device specified.");
                    return new EmulatorDevice(m_EmulatorDevice);

            }
#endif
        }

        private IVRDevice AutoCreateDevice()
        {
            Debug.LogFormat("[VREmulator] XRDevice.model={0}", UnityEngine.XR.XRDevice.model);

            var model = UnityEngine.XR.XRDevice.model;
            if (ModelCheck(model, "daydream")) // Daydream
            {
                return new Devices.DaydreamView.DaydreamViewDevice();
            }
            else if (
                ModelCheck(model, "galaxy") || // GearVR
                ModelCheck(model, "pacific")   // Oculus Go
            )
            {
                return new Devices.GearVR.GearVRDevice();
            }
            else
            {
                Debug.Log("[VREmulator] Unknown device detected");
                return new EmulatorDevice(m_EmulatorDevice);
            }
        }

        private bool ModelCheck(string model, string keyword)
        {
            return model.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}
