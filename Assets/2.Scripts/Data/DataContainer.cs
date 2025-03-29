using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class Data
    {
        [SerializeField] private DataType dataType;
        [SerializeField] private List<GameDataScriptable> dataList;

        public DataType DataType => dataType;

        public GameDataScriptable GetData(int id)
        {
            return dataList.Find((value) => value.ID == id);
        }
    }

    // Scriptable 오브젝트들을 관리하는 클래스
    public class DataContainer : MonoBehaviour
    {
        [SerializeField] private List<Data> dataList;

        public T GetData<T>(DataType dataType, int id) where T : GameDataScriptable
        {
            Data targetDataList = dataList.Find((value) => value.DataType == dataType);
            GameDataScriptable targetData = targetDataList.GetData(id);

            if (targetData == null || !(targetData is T))
            {
                return null;
            }

            return targetData as T;
        }
    }
}