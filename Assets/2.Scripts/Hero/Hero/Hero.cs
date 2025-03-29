using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


namespace Game.Entity
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer model;
        [SerializeField] private Transform weaponPivot;
        [SerializeField] private Transform shotPos;
        [SerializeField] private SpriteRenderer weaponModel;
        [SerializeField] private LineRenderer arrageView;

        private WeaponDataScriptable weaponData;

        private Camera mainCam = null;
        private Vector2 mousePos = Vector2.zero;

        private float time = 0f;

        private TweenerCore<Vector3, Vector3, VectorOptions> tweener = null;

        private void Awake()
        {
            // 일단 총 데이터가 1개밖에 없어서 ID 고정해서 불러오기
            weaponData = ManagerTable.DataContainer.GetData<WeaponDataScriptable>(DataType.Weapon, 1001);
            mousePos = transform.right;
            mousePos.y = transform.position.y;

            ManagerTable.InputManager.AddEvent(Data.EventType.PointerDown, OnClick);
            ManagerTable.InputManager.AddEvent(Data.EventType.Drag, OnClick);

            mainCam = Camera.main;

            UpdateAngle();
        }

        private void Update()
        {
            if (time < weaponData.Interval)
            {
                time += Time.deltaTime;
                return;
            }

            Vector2 dir = (mousePos - (Vector2)shotPos.position).normalized;
            weaponData.Weapon.Excute(weaponData, dir, shotPos.position);

            if (tweener == null)
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

        private void OnClick(PointerEventData eventData)
        {
            mousePos = mainCam.ScreenToWorldPoint(eventData.position);
            if (mousePos.x < transform.position.x)
            {
                mousePos.x = transform.position.x;
            }
            UpdateAngle();
        }

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
    }
}