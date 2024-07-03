using UnityEngine;
using UnitySensors.Attribute;

namespace UnitySensors.Sensor.IMU
{
    public class IMUSensor : UnitySensor
    {
        private Transform _transform;

        [SerializeField, ReadOnly]
        private Vector3 _position;
        [SerializeField, ReadOnly]
        private Vector3 _velocity;
        [SerializeField, ReadOnly]
        private Vector3 _acceleration;
        [SerializeField, ReadOnly]
        private Quaternion _rotation;
        [SerializeField, ReadOnly]
        private Vector3 _angularVelocity;

        private Vector3 _position_tmp;
        private Vector3 _velocity_tmp;
        private Vector3 _acceleration_tmp;
        private Quaternion _rotation_tmp;
        private Vector3 _angularVelocity_tmp;

        private Vector3 _position_last;
        private Vector3 _velocity_last;
        private Quaternion _rotation_last;

        private Quaternion _initialRotation; // To store the initial rotation
        private Vector3 _initialPosition; // To store the initial position

        public Vector3 position { get => _position; }
        public Vector3 velocity { get => _velocity; }
        public Vector3 acceleration { get => _acceleration; }
        public Quaternion rotation { get => _rotation; }
        public Vector3 angularVelocity { get => _angularVelocity; }

        public Vector3 localVelocity { get => _transform.InverseTransformDirection(_velocity); }
        public Vector3 localAcceleration { get => _transform.InverseTransformDirection(_acceleration.normalized) * _acceleration.magnitude; }

        [SerializeField]
        private bool useGravity = true; // Option to enable or disable gravity
        [SerializeField]
        private bool useAbsolute = true; // Option to use absolute or relative rotation and acceleration

        private Vector3 _gravityDirection;
        private float _gravityMagnitude;
        private float _time_last;

        protected override void Init()
        {
            _transform = this.transform;
            _gravityDirection = Physics.gravity.normalized;
            _gravityMagnitude = Physics.gravity.magnitude;

            // Store the initial rotation and position
            _initialRotation = _transform.rotation;
            _initialPosition = _transform.position;

            _position_last = _initialPosition;
            _velocity_last = Vector3.zero;
            _rotation_last = Quaternion.identity;
        }

        protected override void Update()
        {
            float dt = Time.deltaTime;

            _position_tmp = _transform.position;
            _velocity_tmp = (_position_tmp - _position_last) / dt;
            _acceleration_tmp = (_velocity_tmp - _velocity_last) / dt;

            // Apply gravity in the correct axis (typically downward along the y-axis) if gravity is enabled
            if (useGravity)
            {
                _acceleration_tmp += _gravityDirection * _gravityMagnitude;
            }

            // Adjust acceleration based on absolute or relative setting
            if (!useAbsolute)
            {
                _acceleration_tmp = _transform.InverseTransformDirection(_acceleration_tmp);
            }

            _rotation_tmp = _transform.rotation;

            // Calculate relative rotation if useAbsolute is false
            if (!useAbsolute)
            {
                _rotation_tmp = Quaternion.Inverse(_initialRotation) * _rotation_tmp;
            }

            Quaternion rotation_delta = Quaternion.Inverse(_rotation_last) * _rotation_tmp;
            rotation_delta.ToAngleAxis(out float angle, out Vector3 axis);
            float angularSpeed = (angle * Mathf.Deg2Rad) / dt;
            _angularVelocity_tmp = axis * angularSpeed;

            _position_last = _position_tmp;
            _velocity_last = _velocity_tmp;
            _rotation_last = _rotation_tmp;

            base.Update();
        }

        protected override void UpdateSensor()
        {
            _position = _position_tmp;
            _velocity = _velocity_tmp;
            _acceleration = _acceleration_tmp;

            _rotation = _rotation_tmp;
            _angularVelocity = _angularVelocity_tmp;

            if (onSensorUpdated != null)
                onSensorUpdated.Invoke();
        }

        protected override void OnSensorDestroy()
        {
        }
    }
}
