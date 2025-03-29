using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Entity.Interface;
using Game.Manager;
using Game.Utils;
using UnityEngine;

namespace Game.Entity
{
    public class ChaGun : Weapon
    {
        public override void Excute(WeaponDataScriptable data, Vector2 direction, Vector2 spawnPos)
        {
            for (int i = 0; i < data.BulletCount; i++)
            {
                Bullet bullet = ManagerTable.ObjectPool.InstantiateT<Bullet>(data.Bullet.gameObject, spawnPos, Quaternion.identity);
                float rndAngle = Random.Range(-10f, 10f);
                Vector2 bulletDir = Util.GetAngleVector(rndAngle, direction);
                float rndSpeed = Random.Range(20f, 25f);

                bullet.Init(bulletDir.normalized, data.Damage, rndSpeed);
            }
        }
    }
}