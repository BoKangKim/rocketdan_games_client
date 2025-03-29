using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Manager;
using UnityEngine;

namespace Game.Entity
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPosArr;
        // TODO FLOW 기능 완성 시 FlowManager에서 가져오기
        [SerializeField] private StageDataScriptable curStageData;
        [SerializeField] private Transform tower;

        private Camera mainCam = null;
        private bool spawn = true;
        private float intervalTimer = 0f;
        private float curInterval = 0f;

        private int count = 0;

        private void Awake()
        {
            intervalTimer = 0f;
            curInterval = curStageData.MaxSpawnInterval;
        }

        private void Start()
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            if (!spawn)
            {
                return;
            }

            intervalTimer -= Time.deltaTime;

            if (intervalTimer <= 0f)
            {
                curInterval -= curStageData.DecreaseScale;
                if (curInterval <= curStageData.MinSpawnInterval)
                {
                    curInterval = curStageData.MinSpawnInterval;
                }

                intervalTimer = curInterval;
                RandomSpawn();
                count++;
            }
        }

        private void RandomSpawn()
        {
            var monsterGroup = curStageData.GetRandomMonsterGroup();
            MonsterDataScriptable monsterData = monsterGroup.GetRandomMonster();

            Vector2 spawnPos = tower.transform.position;
            float cameraHeight = mainCam.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCam.aspect;
            spawnPos.x = cameraWidth / 2f + 5f;

            int rndLayer = Random.Range(0, 3);
            spawnPos.y = spawnPosArr[rndLayer].position.y;

            if (spawnPos.x >= transform.position.x)
            {
                spawnPos = transform.position;
            }

            var monster = ManagerTable.ObjectPool.InstantiateT<Monster>(monsterGroup.Prefab, spawnPos, Quaternion.identity);

            monster.Init(monsterData, rndLayer);
        }

        public void StartSpawn()
        {
            spawn = true;
        }

        public void StopSpawn()
        {
            spawn = false;
        }

        public void Reset()
        {
            intervalTimer = curStageData.MaxSpawnInterval;
            curInterval = curStageData.MaxSpawnInterval;
        }
    }
}