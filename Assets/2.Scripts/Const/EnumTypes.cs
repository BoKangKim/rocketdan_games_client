using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public enum MonsterType
    {
        ZombieMelee
    }

    public enum DataType
    {
        Stage,
        Monster,
        Box,
        Weapon
    }

    public enum EventType
    {
        PointerDown,
        PointerUp,
        Drag
    }
}