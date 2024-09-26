using System;
using UnityEngine;

namespace Combat
{
    public abstract class Bullet: MonoBehaviour
    {
        [Range(1, 10)] [SerializeField] protected float speed;
        [SerializeField] protected float lifetime;
        [SerializeField] protected int damage = 1;

        protected Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifetime);
        }
        protected abstract void FixedUpdate();
        protected abstract void OnTriggerEnter2D(Collider2D other);
    }
}