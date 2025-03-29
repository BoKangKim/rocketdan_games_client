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
        [SerializeField] private Canvas hpPanelCanvas;

        private int order = 0;

        protected override void OnDisable()
        {
            leftArm.sortingOrder -= (order * 10);
            rightArm.sortingOrder -= (order * 10);
            body.sortingOrder -= (order * 10);
            head.sortingOrder -= (order * 10);
            leftLeg.sortingOrder -= (order * 10);
            rightLeg.sortingOrder -= (order * 10);
            hpPanelCanvas.sortingOrder += (order * 10);
            base.OnDisable();
        }

        public override void Init(MonsterDataScriptable data, int layer)
        {
            base.Init(data, layer);
            this.order = layer;

            leftArm.sortingOrder += (order * 10);
            rightArm.sortingOrder += (order * 10);
            body.sortingOrder += (order * 10);
            head.sortingOrder += (order * 10);
            leftLeg.sortingOrder += (order * 10);
            rightLeg.sortingOrder += (order * 10);
            hpPanelCanvas.sortingOrder += (order * 10);
        }

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