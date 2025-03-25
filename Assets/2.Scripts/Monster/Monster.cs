using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Entity
{
    public abstract class Monster : MonoBehaviour
    {
        [SerializeField] protected Animator ani;
        [SerializeField] protected Rigidbody2D rigid;

        public Rigidbody2D Rigid => rigid;

        protected MonsterDataScriptable data = null;
        protected Transform target = null;

        protected bool isJump = false;
        public bool IsJump => isJump;

        [SerializeField] protected bool isOnMonster = false;
        public bool IsOnMonster => isOnMonster;

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
        }

        protected abstract void Move();
        protected abstract bool RangeCheck();
        protected abstract void Attack();
    }
}