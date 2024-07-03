using UnityEngine;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Std;
using UnitySensors.Sensor.Encoder;

namespace UnitySensors.ROS.Serializer.Encoder
{
    [System.Serializable]
    public class EncoderMsgSerializer : RosMsgSerializer<EncoderSensor, Float32Msg>
    {
        [SerializeField]
        private HeaderSerializer _header;

        public override void Init(EncoderSensor sensor)
        {
            base.Init(sensor);
            _header.Init(sensor);
        }

        public override Float32Msg Serialize()
        {
            _msg.data = sensor.totalRotations;
            return _msg;
        }
    }
}
