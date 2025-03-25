using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Manager
{
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

        private static ObjectPool objectPool = null;
        private static FlowManager flowManager = null;
    }
}