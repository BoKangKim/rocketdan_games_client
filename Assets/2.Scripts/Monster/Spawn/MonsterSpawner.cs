using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Manager;
using UnityEngine;

namespace Game.Entity
{
    public class MonsterSpawner : MonoBehaviour
    {
        // TODO FLOW 기능 완성 시 FlowManager에서 가져오기
        [SerializeField] private StageDataScriptable curStageData;
        [SerializeField] private Transform tower;

        private Camera mainCam = null;
        private bool spawn = true;
        private float intervalTimer = 0f;
        private float curInterval = 0f;

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
            }
        }

        private void RandomSpawn()
        {
            var monsterGroup = curStageData.GetRandomMonsterGroup();
            MonsterDataScriptable monsterData = monsterGroup.GetRandomMonster();

            Vector2 spawnPos = tower.transform.position;
            float cameraHeight = mainCam.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCam.aspect;
            spawnPos.x = cameraWidth / 2f + 10f;
            spawnPos.y = transform.position.y;

            if (spawnPos.x >= transform.position.x)
            {
                spawnPos = transform.position;
            }

            var monster = ManagerTable.ObjectPool.InstantiateT<Monster>(monsterGroup.Prefab, spawnPos, Quaternion.identity);
            monster.Init(monsterData, tower);
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