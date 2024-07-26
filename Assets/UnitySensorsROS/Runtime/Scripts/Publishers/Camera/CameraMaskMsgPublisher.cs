using UnityEngine;
using UnitySensors.Sensor.Camera;

namespace UnitySensors.ROS.Publisher.Mask
{
    [RequireComponent(typeof(HSVCameraSensor))]
    public class CameraMaskMsgPublisher : MaskMsgPublisher<HSVCameraSensor>
    {
    }
}
