using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputListener : MonoBehaviour
    {
        private Vector2 _moveInput = Vector2.zero;
        private bool _firePressed;
        private bool _restartPressed;
        
        public void GetMovePressed(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                _moveInput = context.ReadValue<Vector2>();
            }
        }
        public void GetFirePressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _firePressed = true;
            }
            else if (context.canceled)
            {
                _firePressed = false;
            }
        }

        public void GetRestartPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _restartPressed = true;
            }
            else if (context.canceled)
            {
                _restartPressed = false;
            }
        }
        public Vector2 GetMoveInput()
        {
            return _moveInput;
        }
        public bool IsFirePressed()
        {
            var result = _firePressed;
            _firePressed = false;
            return result;
        }
        public bool IsRestartPressed()
        {
            var result = _restartPressed;
            _restartPressed = false;
            return result;
        }
    }
}
