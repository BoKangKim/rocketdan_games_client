using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Manager
{
    public class ManagerTable : MonoBehaviour
    {
        public static GameStateManager GameStateManager
        {
            get
            {
                if (gameStateManager == null)
                {
                    gameStateManager = FindObjectOfType<GameStateManager>(true);
                }

                return gameStateManager;
            }
        }
        private static GameStateManager gameStateManager = null;
    }
}