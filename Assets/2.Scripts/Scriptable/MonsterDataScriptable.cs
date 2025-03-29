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
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private float attackPower;

        public int MaxHP => maxHp;
        public float Speed => speed;
        public float Range => range;
        public float AttackPower => attackPower;
    }
}

