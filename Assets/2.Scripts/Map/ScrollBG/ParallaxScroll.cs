using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Manager;
using UnityEngine;

namespace Game.Map
{
    public class ParallaxScroll : MonoBehaviour
    {
        [SerializeField] private Transform backBGPrefab;
        [SerializeField] private Transform frontBGPrefab;
        [SerializeField] private float parallax = 0.7f;
        [SerializeField] private float targetXPos;

        private List<Transform> backBGList = new List<Transform>();
        private List<Transform> frontBGList = new List<Transform>();

        private Transform backBGCenter;
        private Transform frontBGCenter;

        private bool scroll = true;

        private void Awake()
        {
            // Pool Object로 등록하기 위해 초기화 해두는 것이 아닌 시작하자마자 생성으로 변경
            backBGCenter = ManagerTable.ObjectPool.InstantiateT<Transform>(backBGPrefab.gameObject, new Vector3(0f, 0.7f, 0f), Quaternion.identity, transform);
            frontBGCenter = ManagerTable.ObjectPool.InstantiateT<Transform>(frontBGPrefab.gameObject, new Vector3(0f, 0.1f, 0f), Quaternion.identity, transform);

            backBGList.Add(backBGCenter);
            frontBGList.Add(frontBGCenter);
        }

        private void Update()
        {
            if (!scroll || backBGList.Count == 0 || frontBGList.Count == 0)
            {
                return;
            }

            // 왼쪽으로 움직이기
            foreach (var backBG in backBGList)
            {
                Vector2 backPos = backBG.position;
                backPos.x -= Time.deltaTime * PlayData.PlayerData.Speed * parallax;
                backBG.transform.position = backPos;
            }

            foreach (var frontBG in frontBGList)
            {
                Vector2 frontPos = frontBG.position;
                frontPos.x -= Time.deltaTime * PlayData.PlayerData.Speed;
                frontBG.transform.position = frontPos; ;
            }

            // 일정 포지션 지나면 오른쪽에 새로 생성
            if (backBGCenter.position.x <= -targetXPos)
            {
                backBGCenter = ManagerTable.ObjectPool.InstantiateT<Transform>(backBGPrefab.gameObject, new Vector3(targetXPos, 0.7f, 0f), Quaternion.identity, transform);
                backBGList.Add(backBGCenter);
            }

            if (frontBGCenter.position.x <= -targetXPos)
            {
                frontBGCenter = ManagerTable.ObjectPool.InstantiateT<Transform>(frontBGPrefab.gameObject, new Vector3(targetXPos, 0.1f, 0f), Quaternion.identity, transform);
                frontBGList.Add(frontBGCenter);
            }

            // 화면에 안보일 정도가 되면 삭제 해줌
            if (backBGList[0].position.x <= -targetXPos * 2f)
            {
                Transform left = backBGList[0];
                backBGList.RemoveAt(0);
                ManagerTable.ObjectPool.DestroyPoolObject(left.gameObject);
            }

            if (frontBGList[0].position.x <= -targetXPos * 2f)
            {
                Transform left = frontBGList[0];
                frontBGList.RemoveAt(0);
                ManagerTable.ObjectPool.DestroyPoolObject(left.gameObject);
            }
        }

        public void Stop()
        {
            scroll = false;
        }

        public void Scroll()
        {
            scroll = true;
        }
    }
}
