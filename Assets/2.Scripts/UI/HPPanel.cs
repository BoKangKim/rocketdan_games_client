using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    // HP Slider
    public class HPPanel : MonoBehaviour
    {
        [SerializeField] private Slider hpSlide;

        public void UpdateView(int value, int maxValue)
        {
            float result = (float)value / (float)maxValue;
            hpSlide.value = result;
        }
    }
}

