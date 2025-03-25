using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    // 전역적으로 사용하는 데이터를 ScriptableObjec로 관리
    [CreateAssetMenu(fileName = nameof(GlobalConfig), menuName = "Data/" + nameof(GlobalConfig))]
    public class GlobalConfig : SingletonScriptableObject<GlobalConfig>
    {
        [Header("Player Data")]
        [SerializeField] public float speed;
    }

    // Global Confgi Data 분류
    public static class PlayData
    {
        private static GlobalConfig globalConfig => GlobalConfig.Instance;

        public static class PlayerData
        {
            public static float Speed => globalConfig.speed;
        }
    }
}

