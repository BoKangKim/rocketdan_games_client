using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    // Enum 타입 모음
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