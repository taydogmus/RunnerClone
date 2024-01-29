using UnityEngine;

namespace Taydogmus
{
    public class InputHandler : MonoBehaviour
    {
        private Joystick _joystick;
        private Vector3 _moveDirectionClamped;
        private Vector3 _moveDirection;
        private bool _takeMovementInput;
        private bool _isInitiated;

        private void Awake()
        {
            _isInitiated = false;
        }

        private void Start()
        {
            GetJoystick();
        }

        private void Update()
        {
            SeekFirstInput();
            HandleInputMoveJoystick();
        }
        
        private void GetJoystick()
        {
            _joystick = CanvasManager.Instance.GamePanel.Joystick;
        }

        private void SeekFirstInput()
        {
            if(_isInitiated) return;
            if (Input.anyKey)
            {
                _isInitiated = true;
                _takeMovementInput = true;
                EventManager.FirstInput();
            }
        }

        private void HandleInputMoveJoystick()
        {
            if (_takeMovementInput)
            {
                var inputVector = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
                _moveDirection = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
                _moveDirectionClamped = Vector3.ClampMagnitude(inputVector, 1);
            }
            else
            {
                _moveDirectionClamped = Vector3.zero;
            }
        }

        public Vector3 GetMovementVectorClamped()
        {
            return _moveDirectionClamped;
        }
        
        public Vector3 GetMovementVector()
        {
            return _moveDirection;
        }
    }
}
