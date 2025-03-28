using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Entity
{
    public class ZombieMelee : MonsterMelee
    {
        [Header("Model")]
        [SerializeField] private SpriteRenderer leftArm;
        [SerializeField] private SpriteRenderer rightArm;
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer head;
        [SerializeField] private SpriteRenderer leftLeg;
        [SerializeField] private SpriteRenderer rightLeg;

        protected override void UpdateModel(MonsterDataScriptable data)
        {
            if (!(data is ZombieDataScriptable))
            {
                return;
            }

            leftArm.sprite = (data as ZombieDataScriptable).LeftArm;
            rightArm.sprite = (data as ZombieDataScriptable).RightArm;
            body.sprite = (data as ZombieDataScriptable).Body;
            head.sprite = (data as ZombieDataScriptable).Head;
            leftLeg.sprite = (data as ZombieDataScriptable).LeftLeg;
            rightLeg.sprite = (data as ZombieDataScriptable).RightLeg;
        }
    }
}