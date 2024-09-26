using Input;
using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        private readonly float _moveSpeed;
        private readonly InputListener _inputListener;
        private readonly Rigidbody2D _rb;
        
        private Vector2 _direction;
        public PlayerMovement(float moveSpeed, InputListener inputListener, Rigidbody2D rb)
        {
            _moveSpeed = moveSpeed;
            _inputListener = inputListener;
            _rb = rb;
        }

        public void HandleMovement()
        {
            _direction = _inputListener.GetMoveInput();
            
            _rb.velocity = new Vector2(_direction.x, _direction.y).normalized * _moveSpeed;
        }
    }
}