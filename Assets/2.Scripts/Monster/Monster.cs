using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Data;
using Game.Manager;
using Game.UI;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        [SerializeField] protected HPPanel hpView;
        [SerializeField] protected float hpViewTimer;
        [SerializeField] protected TextMeshPro damageText;
        public Rigidbody2D Rigid => rigid;

        // 몬스터의 정보
        protected MonsterDataScriptable data = null;
        protected Tower target = null;

        protected Monster collisionMonster = null;

        public bool CanCollision => collisionMonster == null;
        protected bool isOnFloor = false;
        protected bool isRight = false;

        protected int hp = 0;
        public int HP => hp;

        private float time = 0f;
        private Coroutine timerCor = null;

        protected virtual void Awake()
        {
            hpView.gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            data = null;
            target = null;
            collisionMonster = null;
            isOnFloor = false;
            isRight = false;
            hpView.gameObject.SetActive(false);
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

            this.hp = data.MaxHP;
            hpView.UpdateView(hp, data.MaxHP);
        }

        public virtual void Damage(int damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                ani.SetBool(aniKey.Move, false);
                ani.SetBool(aniKey.Attack, false);
                ani.SetBool(aniKey.Dead, true);

                ManagerTable.ObjectPool.DestroyPoolObject(gameObject);
                return;
            }

            time = 0f;

            if (!hpView.gameObject.activeSelf)
            {
                hpView.gameObject.SetActive(true);
                if (timerCor == null)
                {
                    timerCor = StartCoroutine(Timer());
                }
            }

            // View Update
            hpView.UpdateView(hp, data.MaxHP);

            Vector2 spawnPos = transform.position;
            spawnPos.y += 1f;

            TextMeshPro textInst = ManagerTable.ObjectPool.InstantiateT<TextMeshPro>(damageText.gameObject, spawnPos, Quaternion.identity);

            textInst.transform.position = transform.position;
            textInst.text = damage.ToString();
            textInst.gameObject.SetActive(true);

            textInst.transform.DOMove(spawnPos + (Vector2.up * 0.3f), 0.3f).SetAutoKill(true).OnComplete(() =>
            {
                damageText.gameObject.SetActive(false);
                ManagerTable.ObjectPool.DestroyPoolObject(textInst.gameObject);
            });
        }

        protected virtual IEnumerator Timer()
        {
            while (true)
            {
                if (time < hpViewTimer)
                {
                    time += Time.deltaTime;
                }
                else
                {
                    break;
                }
                yield return null;
            }

            hpView.gameObject.SetActive(false);
            timerCor = null;
            yield break;
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
        protected abstract void OnAttack();
    }
}