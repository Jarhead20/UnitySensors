using UnityEngine;
using RosMessageTypes.Std;
using UnitySensors.Sensor.Encoder;
using UnitySensors.ROS.Serializer.Encoder;

namespace UnitySensors.ROS.Publisher.Encoder
{
    [RequireComponent(typeof(EncoderSensor))]
    public class EncoderMsgPublisher : RosMsgPublisher<EncoderSensor, EncoderMsgSerializer, Float32Msg>
    {
    }
}
