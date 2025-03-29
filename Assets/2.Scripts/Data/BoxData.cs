using System.Collections;
using System.Collections.Generic;
using Game.Manager;
using UnityEngine;

namespace Game.Data
{
    // Box HP 등 데이터
    public class BoxData
    {
        private BoxDataScriptable boxData = null;
        private int level = 1;

        public BoxData(int level)
        {
            this.level = level;

            boxData = ManagerTable.DataContainer.GetData<BoxDataScriptable>(DataType.Box, level);
        }

        public Sprite GetModel() => boxData.Model;
        public int GetHP() => boxData.HP;
    }
}
