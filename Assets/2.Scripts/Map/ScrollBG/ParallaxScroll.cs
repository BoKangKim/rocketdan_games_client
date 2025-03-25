using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Map
{
    public class ParallaxScroll : MonoBehaviour
    {
        [SerializeField] private Transform backBG;
        [SerializeField] private Transform frontBG;
        [SerializeField] private float parallax = 0.7f;

        private bool scroll = true;

        private void Update()
        {
            if (!scroll)
            {
                return;
            }

            Vector2 frontPos = frontBG.position;
            Vector2 backPos = backBG.position;

            frontPos.x -= Time.deltaTime * PlayData.PlayerData.Speed;
            backPos.x -= Time.deltaTime * PlayData.PlayerData.Speed * parallax;

            frontBG.transform.position = frontPos;
            backBG.transform.position = backPos;
        }

        public void Stop()
        {
            scroll = false;
        }

        public void Scroll()
        {
            scroll = true;
        }
    }
}
