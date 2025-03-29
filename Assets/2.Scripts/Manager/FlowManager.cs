using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Entity;
using UnityEngine;

namespace Game.Manager
{
    // Game의 Flow(시작 전, 시작 , 게임 종료 등)을 관리하려고 만든 스크립트
    public class FlowManager : MonoBehaviour
    {
        [SerializeField] private Tower tower;
        public Tower Tower => tower;

    }
}