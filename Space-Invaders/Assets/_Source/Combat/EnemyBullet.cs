using Player;
using UnityEngine;

namespace Combat
{
    public class EnemyBullet: Bullet
    {
        protected override void FixedUpdate()
        {
            _rb.velocity = -transform.up * speed;
        }
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                other.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }
}