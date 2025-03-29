using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class Util
    {
        public static Vector2 GetAngleVector(float angle, Vector2 standardVector)
        {
            float radian = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);

            return new Vector2(standardVector.x * cos - standardVector.y * sin,
                standardVector.x * sin + standardVector.y * cos);
        }
    }

}
