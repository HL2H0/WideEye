using BoneLib;
using UnityEngine;
using WideEye.UI;
using WideEye.Utilities;

namespace WideEye.Behaviors
{
    public class FreeCam : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float fastMoveSpeed = 7f;
        public float sensitivity = 3f;
        public float smoothSpeed = 10f;

        public float scrollSensitivity = 15f;
        public float scrollSmoothing = 10f;

        private bool _isLooking;
        private bool _enableFreeCam = true;
        private Vector3 _targetPosition;
        private float _currentScrollValue;
        private float _targetScrollValue;

        private void Start()
        {
            _targetPosition = transform.position;
            _targetScrollValue = ModMenu.FOVSlider.Value;
        }

        private void Update()
        {
            if (!_enableFreeCam) return;
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _isLooking = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                _isLooking = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            
            if (!_isLooking) return;

            var fastMove = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            var speed = fastMove ? fastMoveSpeed : moveSpeed;
            var scrollInput = Input.GetAxis("Mouse ScrollWheel");
            
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.A)) moveDirection += -transform.right;
            if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;
            if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
            if (Input.GetKey(KeyCode.S)) moveDirection += -transform.forward;

            if (Input.GetKey(KeyCode.Q)) moveDirection += -transform.up;
            if (Input.GetKey(KeyCode.E)) moveDirection += transform.up;

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = Player.Head.position;
                _targetPosition = transform.position;
                _targetScrollValue = ModMenu.FOVSlider.Value;
                _currentScrollValue = _targetScrollValue;
            }

            _targetScrollValue += scrollInput * scrollSensitivity;
            
            _targetScrollValue = MathF.Floor(_targetScrollValue);
            _currentScrollValue = Mathf.Lerp(_currentScrollValue, _targetScrollValue, scrollSmoothing * Time.deltaTime);
            
            transform.position = Vector3.Lerp(transform.position, _targetPosition, smoothSpeed * Time.deltaTime);
            SettingsApplier.ApplyFOV(_currentScrollValue, true, ModMenu.FOVSlider);
            
            _targetPosition += moveDirection.normalized * (speed * Time.deltaTime);
            
            if (_isLooking)
            {
                var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
                var rotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;

                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0f);
            }
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isLooking = false;
            _enableFreeCam = false;
        }

        private void OnEnable()
        {
            _enableFreeCam = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _targetPosition = transform.position;
            _targetScrollValue = ModMenu.FOVSlider.Value;
        }
    }
}