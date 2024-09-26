﻿using UnityEngine;

namespace Combat
{
    public class PlayerBullet: Bullet
    {
        protected override void FixedUpdate()
        {
            _rb.velocity = transform.up * speed;
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Destroy(gameObject);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //todo speed up all enemies 
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}