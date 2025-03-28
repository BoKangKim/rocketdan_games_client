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
        private bool isRight = false;

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

        public void ForceSet(Monster monster, bool isRight)
        {
            this.collisionMonster = monster;
            this.isRight = isRight;
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
                if (!collisionMonster.CanCollision || Mathf.Abs(collisionMonster.transform.position.y - transform.position.y) > 0.3f)
                {
                    collisionMonster = null;
                    return;
                }

                isRight = collisionMonster.transform.position.x <= transform.position.x;
                collisionMonster.ForceSet(this, !isRight);
                force = 1f;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.transform.tag.Equals("Monster"))
            {
                if (transform.position.y + 0.7f <= collision.transform.position.y)
                {
                    rigid.velocity = Vector2.right * 1.2f;
                }
            }
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
            if (collisionMonster == null || !isRight)
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
                rigid.AddForce((Vector2.left + Vector2.up) * 20f);
                return;
            }

            collisionMonster.ResetCollision();
            collisionMonster = null;
        }
    }
}