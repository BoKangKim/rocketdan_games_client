using UnityEngine;

namespace Game.Data
{
    // GlobalConfig
    public class SingletonScriptableObject<TScriptableObject> : ScriptableObject
        where TScriptableObject : ScriptableObject
    {
        private static TScriptableObject instance;
        public static TScriptableObject Instance
        {
            get
            {
                if (instance == null)
                {
                    string path = $"Data/{typeof(TScriptableObject).Name}";
                    //Utils.MakeLog($"Path : {path}");
                    instance = Resources.Load<TScriptableObject>(path);
                }

                return instance;
            }
        }
    }
}