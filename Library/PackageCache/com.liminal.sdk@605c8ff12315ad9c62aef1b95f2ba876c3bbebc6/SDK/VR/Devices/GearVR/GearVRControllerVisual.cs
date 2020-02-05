using Liminal.SDK.VR.Avatars.Controllers;
using UnityEngine;

namespace Liminal.SDK.VR.Devices.GearVR
{
    public class GearVRControllerVisual : VRControllerVisual
    {
        [SerializeField] private OVRControllerHelper trackedRemote = null;
        private bool isOculusGo;

        protected override void Awake()
        {
            isOculusGo = GearVRDevice.IsOculusGo;
            base.Awake();
        }
    }
}
