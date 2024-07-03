using UnityEngine;
using UnitySensors.Attribute;

namespace UnitySensors.Sensor.Encoder
{
    public class EncoderSensor : UnitySensor
    {
        [SerializeField, ReadOnly]
        private float _totalRotations = 0f;
        [SerializeField, ReadOnly]
        private float _lastDelatRotation = 0f;
        
        public float totalRotations { get => _totalRotations; }

        private float lastRotation = 0f;

        protected override void Init()
        {
            _totalRotations = 0f;
        }

        protected override void UpdateSensor()
        {
            UpdateRotation();

            if (onSensorUpdated != null)
                onSensorUpdated.Invoke();
        }

        void UpdateRotation()
        {
            // Get current rotation
            float currentRotation = transform.localEulerAngles.z;

            // Calculate rotation delta
            float rotationDelta = CalculateRotationDelta(currentRotation, lastRotation);
            _lastDelatRotation = rotationDelta;

            // Update total rotations
            _totalRotations += rotationDelta;

            // Update last rotation
            lastRotation = currentRotation;
        }

        float CalculateRotationDelta(float currentRotation, float lastRotation)
        {
            // Calculate raw rotation delta
            float rotationDelta = currentRotation - lastRotation;

            // Handle wrap-around at -180/+180 degrees (or -π/+π radians)
            if (rotationDelta > 180f)
            {
                rotationDelta -= 360f;
            }
            else if (rotationDelta < -180f)
            {
                rotationDelta += 360f;
            }

            return rotationDelta;
        }

        protected override void OnSensorDestroy()
        {
            
        }
    }
}
