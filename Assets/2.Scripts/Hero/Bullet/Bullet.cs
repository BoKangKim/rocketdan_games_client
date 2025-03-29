using System.Collections;
using System.Collections.Generic;
using Game.Manager;
using UnityEngine;

namespace Game.Entity
{
    public class Bullet : MonoBehaviour
    {
        private Vector2 direction;
        private bool isShot = false;
        private bool isCollision = false;
        private int damage = 0;
        private float speed = 0f;

        private WaitForSeconds wait = new WaitForSeconds(3f);
        private Coroutine timer = null;

        public void Init(Vector2 direction, int damage, float speed)
        {
            this.direction = direction;
            this.damage = damage;
            this.speed = speed;

            isShot = true;
            isCollision = false;

            if (timer != null)
            {
                StopAllCoroutines();
                timer = null;
            }

            timer = StartCoroutine(Timer());
        }

        private void OnDisable()
        {
            isShot = false;
            isCollision = false;
            StopAllCoroutines();
            timer = null;
        }

        private void Update()
        {
            if (!isShot)
            {
                return;
            }

            transform.Translate(direction * Time.deltaTime * speed);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isCollision)
            {
                return;
            }

            if (other.tag.Equals("Floor"))
            {
                ManagerTable.ObjectPool.DestroyPoolObject(gameObject);
                isCollision = true;
                return;
            }

            Monster monster = other.GetComponentInParent<Monster>();

            if (monster != null)
            {
                if (!monster.gameObject.activeSelf)
                {
                    return;
                }

                monster.Damage(damage);
                ManagerTable.ObjectPool.DestroyPoolObject(gameObject);
                isCollision = true;
            }
        }

        private IEnumerator Timer()
        {
            yield return wait;
            timer = null;
            ManagerTable.ObjectPool.DestroyPoolObject(gameObject);
        }
    }
}