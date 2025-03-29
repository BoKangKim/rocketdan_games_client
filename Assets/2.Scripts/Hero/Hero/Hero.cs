using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.UI;


namespace Game.Entity
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private Transform weaponPivot;
        [SerializeField] private Transform shotPos;
        [SerializeField] private SpriteRenderer weaponModel;
        [SerializeField] private LineRenderer arrageView;
        [SerializeField] private HPPanel hpView;

        private WeaponDataScriptable weaponData;

        private Camera mainCam = null;
        private Vector2 mousePos = Vector2.zero;

        private float time = 0f;

        private TweenerCore<Vector3, Vector3, VectorOptions> tweener = null;
        private TweenerCore<Vector3, Vector3, VectorOptions> MoveDowntweener = null;
        private Vector2 initialPos = Vector2.zero;

        private int hp = 0;

        // 데이터 초기화
        private void Awake()
        {
            // 일단 총 데이터가 1개밖에 없어서 ID 고정해서 불러오기
            weaponData = ManagerTable.DataContainer.GetData<WeaponDataScriptable>(DataType.Weapon, 1001);
            mousePos = transform.right;
            mousePos.y = transform.position.y;

            weaponModel.sprite = weaponData.Model;

            ManagerTable.InputManager.AddEvent(Data.EventType.PointerDown, OnClick);
            ManagerTable.InputManager.AddEvent(Data.EventType.Drag, OnClick);

            initialPos = transform.position;
            mainCam = Camera.main;

            this.hp = PlayData.PlayerData.HP;
            hpView.UpdateView(hp, PlayData.PlayerData.HP);

            UpdateAngle();
        }

        // Weapon 데이터 토대로 총을 쏘는 로직직
        private void Update()
        {
            if (time < weaponData.Interval)
            {
                time += Time.deltaTime;
                return;
            }

            Vector2 dir = (mousePos - (Vector2)shotPos.position).normalized;
            weaponData.Weapon.Excute(weaponData, dir, shotPos.position);

            if (tweener == null || MoveDowntweener != null)
            {
                Vector2 pos = transform.position;
                pos.x += Vector2.left.x * 0.3f;
                tweener = transform.DOMove(pos, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo).SetAutoKill(true).OnUpdate(() =>
                {
                    arrageView.SetPosition(1, shotPos.position);
                }).OnComplete(() =>
                {
                    tweener = null;
                });
            }

            time = 0f;
        }

        // 화면을 터치, 클릭했을 때 해당 좌표 변환
        private void OnClick(PointerEventData eventData)
        {
            mousePos = mainCam.ScreenToWorldPoint(eventData.position);
            if (mousePos.x < transform.position.x)
            {
                mousePos.x = transform.position.x;
            }
            UpdateAngle();
        }

        // 좌표를 토대로 Hero의 총 각도 및 Indicator 각도 조정
        private void UpdateAngle()
        {
            Vector2 dir = (mousePos - (Vector2)shotPos.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            weaponPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            Vector2 firstPos = (Vector2)shotPos.position + Utils.Util.GetAngleVector(10f, dir).normalized * 3f;
            Vector2 secondPos = (Vector2)shotPos.position + Utils.Util.GetAngleVector(-10f, dir).normalized * 3f;

            arrageView.SetPosition(0, firstPos);
            arrageView.SetPosition(1, shotPos.position);
            arrageView.SetPosition(2, secondPos);
        }

        // 박스 부서졌을 때 움직임
        public void MoveToLastBox(Vector2 boxPos)
        {
            if (tweener != null)
            {
                tweener = null;
            }

            Vector2 targetPos = boxPos;
            targetPos.y += 1.5f;
            targetPos.x = initialPos.x;

            MoveDowntweener = transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutBack).SetAutoKill(true).OnComplete(() => { MoveDowntweener = null; });
        }

        // 데미지 입었을 때 데이터 처리 및 HP View 업데이트
        public void Damage(int damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                gameObject.SetActive(false);
                return;
            }

            if (!hpView.gameObject.activeSelf)
            {
                hpView.gameObject.SetActive(true);
            }

            hpView.UpdateView(hp, PlayData.PlayerData.HP);
        }
    }
}