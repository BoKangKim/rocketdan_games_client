using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
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

        public override void Init(MonsterDataScriptable data, Transform target)
        {
            base.Init(data, target);
        }

        protected override void Attack()
        {
            // TODO 타워 HP 생기면 기능 추가
            Debug.Log("ATTACK!");
        }

        protected override void Move()
        {
            Climb();

            if (!isOnFloor && collisionMonster == null)
            {
                rigid.velocity = Vector2.left * 2f + Vector2.down;
            }
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
            CollisionEnter(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CollisionExit(collision);
        }

    }
}