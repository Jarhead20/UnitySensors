using UnityEngine;
using UnitySensors.Attribute;

namespace UnitySensors.Sensor.Encoder
{
    public class EncoderSensor : UnitySensor
    {
        [SerializeField, ReadOnly]
        private float _totalRotations = 0f;



        private WheelCollider wheelCollider;

        public float totalRotations { get => _totalRotations; }

        protected override void Init()
        {
            wheelCollider = GetComponent<WheelCollider>();
            if (wheelCollider == null)
            {
                Debug.LogError("No WheelCollider component found. Please add a WheelCollider component.");
            }
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
            if (wheelCollider == null) return;

            // Get the rotational speed in RPM
            float rpm = wheelCollider.rpm;

            // Convert RPM to angular velocity in revolutions per second
            float angularVelocity = rpm / 60.0f;

            // Calculate rotation delta in degrees
            float rotationDelta = angularVelocity * dt;

            // Update total rotations
            _totalRotations += rotationDelta;

        }

        protected override void OnSensorDestroy()
        {
            // Cleanup code if needed
        }
    }
}
