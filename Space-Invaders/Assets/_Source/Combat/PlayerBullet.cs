using UnityEngine;

namespace Combat
{
    public class PlayerBullet: MonoBehaviour
    {
        [Range(1, 10)] [SerializeField] private float speed;
        [SerializeField] private float lifetime;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifetime);
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.up * speed;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //todo destroy enemy
                Destroy(gameObject);
            }
        }
    }
}