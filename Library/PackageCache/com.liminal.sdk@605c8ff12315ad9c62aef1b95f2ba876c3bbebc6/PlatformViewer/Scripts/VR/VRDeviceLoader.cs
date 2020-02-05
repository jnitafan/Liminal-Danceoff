using System;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Devices.Emulator;
using Liminal.SDK.VR.Devices.GearVR;
using UnityEngine;
using UnityEngine.XR;

namespace Liminal.Platform.Experimental.VR
{
    public class VRDeviceLoader : IVRDeviceInitializer
    {
        [Tooltip("The device to emulate during development. NOTE: You may need to change Player Settings to correctly emulate some devices.")]
        [SerializeField] private VREmulatorDevice m_EmulatorDevice = VREmulatorDevice.Daydream;

        #region Properties

        /// <summary>
        /// Gets the device type the emulator will attempt emulate.
        /// </summary>
        public VREmulatorDevice EmulatorDevice
        {
            get { return m_EmulatorDevice; }
        }

        #endregion

        public VRDeviceLoader()
        {
            if (VRDevice.Device == null)
            {
                var device = CreateDevice();
                VRDevice.Initialize(device);
            }
        }

        public IVRDevice CreateDevice()
        {
            // Make sure XR is enabled...
            XRSettings.enabled = true;

            Debug.LogFormat("[VRDevice] XRDevice.isPresent={0}", XRDevice.isPresent);
            if (XRDevice.isPresent)
            {
                Debug.LogFormat("[VRDevice] XRDevice.model={0}", XRDevice.model);
            }

            if (!Application.isEditor)
            {
                return new GearVRDevice();
            }

            // Fallback to the emulator device
            return new EmulatorDevice(m_EmulatorDevice);
        }

        private static IVRDevice FindConnectedDeviceByModel(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
                return null;

            throw new NotImplementedException("Device support is not implemented yet");
        }
    }
}
