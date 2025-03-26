using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
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
                rigid.mass = 1;
                isOnFloor = true;
                return;
            }

            if (!CanCollision || !isOnFloor)
            {
                return;
            }

            collisionMonster = collision.transform.GetComponent<Monster>();

            if (collisionMonster != null)
            {
                if (!collisionMonster.IsOnFloor || !isOnFloor || !collisionMonster.CanCollision)
                {
                    collisionMonster = null;
                    return;
                }

                collisionMonster.ForceSet(this);
            }
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

            if (!collisionMonster.IsOnFloor)
            {
                return;
            }

            float yDistance = Mathf.Abs(collisionMonster.transform.position.y - transform.position.y);
            if (yDistance > 1f)
            {
                collisionMonster.ResetCollision();
                collisionMonster = null;
                return;
            }

            if (collisionMonster.transform.position.x <= transform.position.x)
            {
                Vector2 climbDir = Vector2.left + Vector2.up;
                climbDir = climbDir.normalized;
                rigid.velocity = climbDir * 3f;


                return;
            }

            if (!isOnFloor)
            {
                rigid.mass = 9f;
                // rigid.AddForce(Vector2.left + Vector2.down, ForceMode2D.Impulse);
                // collisionMonster.Rigid.AddForce(Vector2.right, ForceMode2D.Impulse);
                collisionMonster.ResetCollision();
                collisionMonster.Rigid.velocity = Vector2.left;
                collisionMonster = null;
            }
        }
    }
}