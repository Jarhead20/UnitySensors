using UnityEngine;

using RosMessageTypes.Sensor;
using UnitySensors.Data.Texture;
using UnitySensors.Sensor.Camera;

namespace UnitySensors.ROS.Serializer.Mask
{
    [System.Serializable]
    public class MaskMsgSerializer<T> : RosMsgSerializer<T, CompressedImageMsg> where T : HSVCameraSensor, ITextureInterface
    {
        [SerializeField]
        private HeaderSerializer _header;
        [SerializeField, Range(1, 100)]
        private int quality = 75;

        public override void Init(T sensor)
        {
            base.Init(sensor);
            _header.Init(sensor);

            _msg.format = "jpeg";
        }

        public override CompressedImageMsg Serialize()
        {
            _msg.header = _header.Serialize();
            _msg.data = sensor.mask.EncodeToJPG(quality);
            return _msg;
        }
    }
}
