using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Entity
{
    public class MonsterMelee : Monster
    {
        [SerializeField] private Collider2D col;
        public float Height => col.bounds.size.y;
        public float Width => col.bounds.size.x;

        protected override void Move()
        {
            // 등반도 하나의 움직임 이므로 Move로 묶음
            Climb();

            // 좀비가 바닥에 있지 않다면 바닥으로 내려오게 하기위해 강제로 바닥쪽으로 힘을 줌
            // 중력이 (-2, -9.8)로 설정되어있기 때문에 강제로 설정하지 않는다면 왼쪽으로 자동으로 움직임
            if (!isOnFloor && collisionMonster == null)
            {
                rigid.velocity = Vector2.left * 2f + Vector2.down;
            }
        }

        protected override void Attack(bool isAttack)
        {
            // Controll Ani
            if (isAttack != ani.GetBool(aniKey.Attack))
            {
                ani.SetBool(aniKey.Attack, isAttack);
            }

            if (!isAttack)
            {
                return;
            }

            // Logic
        }

        protected override bool RangeCheck()
        {
            float distance = Mathf.Abs(target.position.x - transform.position.x);
            return distance <= Width + data.Range;
        }

        protected override void UpdateModel(MonsterDataScriptable data)
        {
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
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
                // 이미 위에 있다면 리턴
                // 체크하지 않으면 자연스럽지 않게 몬스터가 쌓임
                if (!collisionMonster.CanCollision || Mathf.Abs(collisionMonster.transform.position.y - transform.position.y) > 0.1f)
                {
                    collisionMonster = null;
                    return;
                }

                // 등반하는 몬스터인지 확인하기 위해
                isRight = collisionMonster.transform.position.x <= transform.position.x;
                collisionMonster.ForceSet(this, !isRight);
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            // 위에 있는 몬스터만 강제로 아래로 내리려 하면 몬스터 떨림, 겹침 등 
            // 부자연스러운 물리 연산을 최소화 하기 위해 아래에 있는 몬스터를 오른쪽으로 밀어줌
            if (collision.transform.tag.Equals("Monster"))
            {
                if (transform.position.y + Height - 0.1f <= collision.transform.position.y)
                {
                    rigid.velocity = Vector2.right * 1.2f;
                }
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
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

            // Pair인 몬스터와 너무 멀어지면 강제로 리셋
            float yDistance = Mathf.Abs(collisionMonster.transform.position.y - transform.position.y);
            if (yDistance > Height + 0.2f)
            {
                collisionMonster.ResetCollision();
                collisionMonster = null;
                return;
            }

            // 왼쪽위로 힘을 계속줘서 비비면서 올라감 -> 등반하는 느낌
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