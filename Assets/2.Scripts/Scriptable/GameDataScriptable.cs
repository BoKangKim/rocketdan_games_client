using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class GameDataScriptable : ScriptableObject
    {
        [SerializeField] private int id;

        public int ID => id;
    }
}