using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Entity;
using UnityEngine;

namespace Game.Manager
{
    public class FlowManager : MonoBehaviour
    {
        [SerializeField] private Tower tower;
        public Tower Tower => tower;

    }
}