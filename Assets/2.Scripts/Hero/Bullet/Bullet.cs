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

        // 총알의 방향 등 초기화
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

        // 설정한 방향대로 이동
        private void Update()
        {
            if (!isShot)
            {
                return;
            }

            transform.Translate(direction * Time.deltaTime * speed);
        }

        // 땅에 부딪히거나 몬스터에 부딪히면 사라짐
        // 몬스터에 부딪히면 데미지
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