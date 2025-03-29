using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(ZombieDataScriptable), menuName = "Data/" + nameof(ZombieDataScriptable))]
    public class ZombieDataScriptable : MonsterDataScriptable
    {
        [Header("Model")]
        [SerializeField] private Sprite leftArm;
        [SerializeField] private Sprite rightArm;
        [SerializeField] private Sprite body;
        [SerializeField] private Sprite head;
        [SerializeField] private Sprite leftLeg;
        [SerializeField] private Sprite rightLeg;

        public Sprite LeftArm => leftArm;
        public Sprite RightArm => rightArm;
        public Sprite Body => body;
        public Sprite Head => head;
        public Sprite LeftLeg => leftLeg;
        public Sprite RightLeg => rightLeg;
    }
}

