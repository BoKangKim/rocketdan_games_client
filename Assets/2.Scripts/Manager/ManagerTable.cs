using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Manager
{
    // Manager급 스크립트 관리
    public class ManagerTable : MonoBehaviour
    {
        public static ObjectPool ObjectPool
        {
            get
            {
                if (objectPool == null)
                {
                    objectPool = FindObjectOfType<ObjectPool>(true);
                }

                return objectPool;
            }
        }

        public static FlowManager FlowManager
        {
            get
            {
                if (flowManager == null)
                {
                    flowManager = FindObjectOfType<FlowManager>(true);
                }

                return flowManager;
            }
        }

        public static DataContainer DataContainer
        {
            get
            {
                if (dataContainer == null)
                {
                    dataContainer = FindObjectOfType<DataContainer>(true);
                }

                return dataContainer;
            }
        }

        public static InputManager InputManager
        {
            get
            {
                if (inputManager == null)
                {
                    inputManager = FindObjectOfType<InputManager>(true);
                }

                return inputManager;
            }
        }

        private static ObjectPool objectPool = null;
        private static FlowManager flowManager = null;
        private static DataContainer dataContainer = null;
        private static InputManager inputManager = null;
    }
}