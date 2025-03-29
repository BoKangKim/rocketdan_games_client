using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Entity.Interface
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Excute(WeaponDataScriptable data, Vector2 direction, Vector2 spawnPos);
    }
}
