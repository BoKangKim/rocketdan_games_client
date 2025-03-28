using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(StageDataScriptable), menuName = "Data/" + nameof(StageDataScriptable))]
    public class StageDataScriptable : ScriptableObject
    {
        [Serializable]
        public class MonsterGroup
        {
            [SerializeField] private MonsterType type;
            [SerializeField] private GameObject originPrefab;
            [SerializeField] private List<MonsterDataScriptable> mosterDataList;

            public GameObject Prefab => originPrefab;

            public MonsterDataScriptable GetRandomMonster()
            {
                int rnd = UnityEngine.Random.Range(0, mosterDataList.Count);

                return mosterDataList[rnd];
            }
        }

        [SerializeField] private int index;
        [SerializeField] private List<MonsterGroup> appearMonsterTypeList;

        [SerializeField] public float minSpawnInterval;
        [SerializeField] public float maxSpawnInterval;
        [SerializeField] public float decreaseScale;

        public int Index => index;

        public float MinSpawnInterval => minSpawnInterval;
        public float MaxSpawnInterval => maxSpawnInterval;
        public float DecreaseScale => decreaseScale;

        public MonsterGroup GetRandomMonsterGroup()
        {
            int rnd = UnityEngine.Random.Range(0, appearMonsterTypeList.Count);

            return appearMonsterTypeList[rnd];
        }
    }
}

