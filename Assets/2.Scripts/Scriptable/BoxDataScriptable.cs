using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = nameof(BoxDataScriptable), menuName = "Data/" + nameof(BoxDataScriptable))]
    public class BoxDataScriptable : GameDataScriptable
    {
        [SerializeField] private Sprite model;
        [SerializeField] private int hp;

        public Sprite Model => model;
        public int HP => hp;
    }
}