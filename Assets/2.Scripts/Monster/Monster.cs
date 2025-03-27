using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Entity
{
    public abstract class Monster : MonoBehaviour
    {
        [SerializeField] protected Animator ani;
        [SerializeField] protected Rigidbody2D rigid;

        public Rigidbody2D Rigid => rigid;

        protected MonsterDataScriptable data = null;
        protected Transform target = null;

        protected bool isOnFloor = false;
        public bool IsOnFloor => isOnFloor;

        [SerializeField] protected Monster collisionMonster = null;
        public bool CanCollision => collisionMonster == null;

        protected Vector2 extraVelo = Vector2.zero;

        private float force = 1f;

        private void OnDisable()
        {
            data = null;
            target = null;
        }

        protected virtual void Update()
        {
            // 초기화 안됐으면 움직이지 않기
            if (data == null || target == null)
            {
                return;
            }

            if (RangeCheck())
            {
                Attack();
            }

            Move();
        }

        protected abstract void UpdateModel(MonsterDataScriptable data);

        public virtual void Init(MonsterDataScriptable data, Transform target)
        {
            this.data = data;
            this.target = target;
            UpdateModel(data);
            rigid.velocity = Vector2.left * 3f;
        }

        public void ResetCollision()
        {
            collisionMonster = null;
        }

        protected abstract void Move();
        protected abstract bool RangeCheck();
        protected abstract void Attack();

        protected virtual void CollisionEnter(Collision2D collision)
        {
            if (collision.transform.tag.Equals("Floor"))
            {
                isOnFloor = true;
                return;
            }

            if (!CanCollision)
            {
                return;
            }

            collisionMonster = collision.transform.GetComponent<Monster>();

            if (collisionMonster != null)
            {
                if (!collisionMonster.CanCollision)
                {
                    collisionMonster = null;
                    return;
                }

                collisionMonster.ForceSet(this);
                force = 1f;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Monster target))
            {
                if (transform.position.y + 0.7f <= collision.transform.position.y)
                {
                    // rigid.AddForce(Vector2.right * 0.5f, ForceMode2D.Impulse);
                    rigid.velocity = Vector2.right;
                }
            }
            // if (collision.transform.tag.Equals("Tower"))
            // {
            //     if (!isOnFloor)
            //     {
            //         rigid.velocity = (Vector2.left + Vector2.down) * 10f;
            //     }
            // }
        }

        public void ForceSet(Monster monster)
        {
            this.collisionMonster = monster;
        }

        protected virtual void CollisionExit(Collision2D collision)
        {
            if (collision.transform.tag.Equals("Floor"))
            {
                isOnFloor = false;
            }
        }

        protected virtual void Climb()
        {
            if (collisionMonster == null)
            {
                return;
            }

            // if (!collisionMonster.IsOnFloor)
            // {
            //     return;
            // }

            float yDistance = Mathf.Abs(collisionMonster.transform.position.y - transform.position.y);
            if (yDistance > 1f)
            {
                collisionMonster.ResetCollision();
                collisionMonster = null;
                return;
            }

            if (collisionMonster.transform.position.x <= transform.position.x)
            {
                // Vector2 climbDir = ;
                // climbDir = climbDir.normalized;
                // rigid.velocity = climbDir * 3f;


                rigid.AddForce((Vector2.left + Vector2.up) * 15f);

                return;
            }

            if (!isOnFloor)
            {
                collisionMonster.ResetCollision();
                collisionMonster = null;
            }
        }
    }
}