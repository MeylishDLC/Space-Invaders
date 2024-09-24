using System;
using Input;
using UnityEngine;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        private InputListener _inputListener;
        private Rigidbody2D _rb;
        private Vector2 _direction;

        [Inject]
        public void Initialize(InputListener inputListener)
        {
            _inputListener = inputListener;
            _rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            _direction = _inputListener.GetMoveInput();
            
            _rb.velocity = new Vector2(_direction.x, _direction.y).normalized * moveSpeed;
        }
        
    }
}
