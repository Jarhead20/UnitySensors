using UnityEngine;
using UnitySensors.Attribute;

namespace UnitySensors.Sensor.Encoder
{
    public class EncoderSensor : UnitySensor
    {
        [SerializeField, ReadOnly]
        private float _totalRotations = 0f;

        // Axis selection
        public Axis axis = Axis.Z;
        [SerializeField]
        public enum Axis
        {
            X,
            Y,
            Z
        }

        private Rigidbody rb;

        public float totalRotations { get => _totalRotations; }

        protected override void Init()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("No Rigidbody component found. Please add a Rigidbody component.");
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
            if (rb == null) return;

            // Get angular velocity around the selected axis
            float angularVelocity = 0f;
            switch (axis)
            {
                case Axis.X:
                    angularVelocity = rb.angularVelocity.x;
                    break;
                case Axis.Y:
                    angularVelocity = rb.angularVelocity.y;
                    break;
                case Axis.Z:
                    angularVelocity = rb.angularVelocity.z;
                    break;
            }

            // Calculate rotation delta in degrees
            float rotationDelta = angularVelocity * Mathf.Rad2Deg * Time.deltaTime;

            Debug.Log("Rotation delta: " + rotationDelta);
            Debug.Log("Angular velocity: " + angularVelocity);

            // Update total rotations
            _totalRotations += rotationDelta;
        }

        protected override void OnSensorDestroy()
        {
            // Cleanup code if needed
        }
    }
}
