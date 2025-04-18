using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(MonsterDataScriptable), menuName = "Data/" + nameof(MonsterDataScriptable))]
    public class MonsterDataScriptable : GameDataScriptable
    {
        [Header("Data")]
        [SerializeField] private int maxHp;
        [SerializeField] private float range;
        [SerializeField] private int attackPower;

        public int MaxHP => maxHp;
        public float Range => range;
        public int AttackPower => attackPower;
    }
}

