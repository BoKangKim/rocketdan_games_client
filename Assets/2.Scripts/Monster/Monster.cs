using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Manager;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Entity
{
    // Animation 이름
    [Serializable]
    public class AnimationKey
    {
        [SerializeField] private string move;
        [SerializeField] private string attack;
        [SerializeField] private string dead;

        public string Move => move;
        public string Attack => attack;
        public string Dead => dead;
    }

    public abstract class Monster : MonoBehaviour
    {
        [SerializeField] protected Animator ani;
        [SerializeField] protected AnimationKey aniKey;
        [SerializeField] protected Rigidbody2D rigid;
        public Rigidbody2D Rigid => rigid;

        // 몬스터의 정보
        protected MonsterDataScriptable data = null;
        protected Tower target = null;

        protected Monster collisionMonster = null;

        public bool CanCollision => collisionMonster == null;
        protected bool isOnFloor = false;
        protected bool isRight = false;

        protected virtual void OnDisable()
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

            bool isAttack = RangeCheck();
            Attack(isAttack);
            Move();
        }

        // 리깅이 비슷한 것들로 묶어서 하나의 프리팹으로 모델 런타입 업뎃
        protected abstract void UpdateModel(MonsterDataScriptable data);

        // 몬스터 데이터 토대로 초기화
        public virtual void Init(MonsterDataScriptable data, int layer)
        {
            this.data = data;
            this.target = ManagerTable.FlowManager.Tower;
            UpdateModel(data);
            rigid.velocity = Vector2.left * 3f;
        }

        // 한 쪽에서 세팅을 했기 때문에 리셋도 한 쪽에서 모두 함
        public void ResetCollision()
        {
            collisionMonster = null;
        }

        // 충돌체 두 개를 Pair로 보기위해 충돌했을 때 다른쪽에서 세팅을 함
        public void ForceSet(Monster monster, bool isRight)
        {
            this.collisionMonster = monster;
            this.isRight = isRight;
        }

        // 움직임
        protected abstract void Move();
        // 범위 체크 - 거의 비슷할 것으로 예상되지만 혹시 몰라 자식의 구현으로 남겨둠
        protected abstract bool RangeCheck();
        // 공격
        protected abstract void Attack(bool isAttack);
    }
}