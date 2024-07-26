using UnityEngine;
using RosMessageTypes.Sensor;
using UnitySensors.Data.Texture;
using UnitySensors.Sensor.Camera;
using UnitySensors.ROS.Serializer.Mask;

namespace UnitySensors.ROS.Publisher.Mask
{
    public class MaskMsgPublisher<T> : RosMsgPublisher<T, MaskMsgSerializer<T>, CompressedImageMsg> where T : HSVCameraSensor, ITextureInterface
    {
    }
}
