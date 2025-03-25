using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Util;
using UnityEngine;

namespace Game.Entity
{
    public class ZombieMelee : Monster
    {
        [Header("Model")]
        [SerializeField] private SpriteRenderer leftArm;
        [SerializeField] private SpriteRenderer rightArm;
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer head;
        [SerializeField] private SpriteRenderer leftLeg;
        [SerializeField] private SpriteRenderer rightLeg;

        private Vector2 direction = Vector2.zero;

        private Vector2 startPos = Vector2.zero;
        private Vector2 targetPos = Vector2.zero;
        private float time = 0f;
        private float speed = 0f;

        private Monster collisionMonster = null;

        public override void Init(MonsterDataScriptable data, Transform target)
        {
            base.Init(data, target);
            direction = Vector2.left;
            speed = data.Speed;
        }

        protected override void Update()
        {
            if (isJump)
            {
                Jump();
                return;
            }

            if (collisionMonster != null && !collisionMonster.IsJump)
            {
                if (!isOnMonster && Mathf.Abs(transform.position.y - collisionMonster.transform.position.y) > 0.1f && transform.position.y < collisionMonster.transform.position.y)
                {
                    isOnMonster = true;
                    direction = Vector2.right;
                    speed /= 2f;
                }

                if (isOnMonster && Mathf.Abs(transform.position.x - collisionMonster.transform.position.x) > 0.8f)
                {
                    collisionMonster = null;
                    isOnMonster = false;
                    direction = Vector2.left;
                    speed = data.Speed;
                }

            }

            base.Update();
        }

        protected override void Attack()
        {
            // TODO 타워 HP 생기면 기능 추가
            Debug.Log("ATTACK!");
        }

        protected override void Move()
        {
            Vector2 velo = new Vector2(direction.x * speed, rigid.velocity.y);
            rigid.velocity = velo;
        }

        protected override bool RangeCheck()
        {
            float distance = Mathf.Abs(target.position.x - transform.position.x);
            return distance <= data.Range;
        }

        protected override void UpdateModel(MonsterDataScriptable data)
        {
            if (!(data is ZombieDataScriptable))
            {
                return;
            }

            leftArm.sprite = (data as ZombieDataScriptable).LeftArm;
            rightArm.sprite = (data as ZombieDataScriptable).RightArm;
            body.sprite = (data as ZombieDataScriptable).Body;
            head.sprite = (data as ZombieDataScriptable).Head;
            leftLeg.sprite = (data as ZombieDataScriptable).LeftLeg;
            rightLeg.sprite = (data as ZombieDataScriptable).RightLeg;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isJump || collisionMonster != null)
            {
                return;
            }

            if (collision.transform.TryGetComponent(out collisionMonster))
            {
                if (!isOnMonster)
                {
                    rigid.velocity = Vector2.zero;
                }

                if (collisionMonster.IsJump || collisionMonster.IsOnMonster)
                {
                    return;
                }

                if (transform.position.x > collisionMonster.transform.position.x && Mathf.Abs(transform.position.y - collisionMonster.transform.position.y) < 0.1f)
                {
                    startPos = transform.position;
                    targetPos = collisionMonster.transform.position;
                    targetPos.y += 1f;
                    rigid.velocity = Vector2.zero;
                    isJump = true;
                }




                // if (transform.position.y < monster.transform.position.x)
                // {
                //     direction = Vector2.right;
                // }

                // if (transform.position.y > monster.transform.position.y)
                // {
                //     isJump = false;
                // }
            }
        }

        // private void OnCollisionExit2D(Collision2D collision)
        // {
        //     if (collision.transform.TryGetComponent(out Monster monster))
        //     {
        //         if (collisionMonster == null || monster.GetComponentIndex() != collisionMonster.GetComponentIndex())
        //         {
        //             return;
        //         }

        //         direction = Vector2.left;
        //         collisionMonster = null;
        //     }
        // }

        private void Jump()
        {
            if (targetPos == Vector2.zero)
            {
                isJump = false;
                time = 0f;
                return;
            }

            time += Time.deltaTime * 0.8f;

            if (time > 1f)
            {
                time = 0f;
                isJump = false;
                return;
            }
            rigid.MovePosition(CalculateBezier(time, 1f));
        }

        private Vector3 CalculateBezier(float time, float height)
        {
            Vector3 p0 = startPos;
            Vector3 p2 = targetPos;
            Vector3 p1 = (p0 + p2) / 2f;
            p1.y += height;

            Vector3 lerp1 = Vector3.Lerp(p0, p1, time);
            Vector3 lerp2 = Vector3.Lerp(p1, p2, time);

            Vector3 result = Vector3.Lerp(lerp1, lerp2, time);

            return result;
        }
    }
}