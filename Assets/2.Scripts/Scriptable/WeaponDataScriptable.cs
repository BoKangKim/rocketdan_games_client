using System.Collections;
using System.Collections.Generic;
using Game.Entity;
using Game.Entity.Interface;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(WeaponDataScriptable), menuName = "Data/" + nameof(WeaponDataScriptable))]
    public class WeaponDataScriptable : GameDataScriptable
    {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Sprite model;
        [SerializeField] private float interval;
        [SerializeField] private int damage;
        [SerializeField] private Bullet bulletObject;
        [SerializeField] private int bulletCount;

        public Weapon Weapon => weapon;
        public float Interval => interval;
        public int Damage => damage;
        public Sprite Model => model;
        public Bullet Bullet => bulletObject;
        public int BulletCount => bulletCount;
    }
}
