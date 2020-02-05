using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInputExample : MonoBehaviour
{
    public Text InputText; 

    private void Update()
    {
        var device = VRDevice.Device;
        if (device != null)
        {
            StringBuilder inputStringBuilder = new StringBuilder("");

            AppendDeviceInput(inputStringBuilder, device.PrimaryInputDevice, "Primary");
            AppendDeviceInput(inputStringBuilder, device.SecondaryInputDevice, "Secondary");

            InputText.text = inputStringBuilder.ToString();
        }
    }

    public void AppendDeviceInput(StringBuilder builder, IVRInputDevice inputDevice, string deviceName)
    {
        if (inputDevice == null)
            return;

        builder.AppendLine($"{deviceName} Back: {inputDevice.GetButton(VRButton.Back)}");
        builder.AppendLine($"{deviceName} Button One: {inputDevice.GetButton(VRButton.Primary)}");
        builder.AppendLine($"{deviceName} Touch Pad Touching: {inputDevice.IsTouching}");
        builder.AppendLine($"{deviceName} Axis2D-One: {inputDevice.GetAxis2D(VRAxis.One)}");
        builder.AppendLine($"{deviceName} Axis2D-OneRaw: {inputDevice.GetAxis2D(VRAxis.OneRaw)}");
    }
}
