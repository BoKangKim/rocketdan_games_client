using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Manager
{
    // 터치 이벤트를 여기에 등록해서 사용
    public class InputManager : MonoBehaviour
    {
        private Action<PointerEventData> onPointerUpEvent = null;
        private Action<PointerEventData> onPointerDownEvent = null;
        private Action<PointerEventData> onDragEvent = null;

        public void AddEvent(Game.Data.EventType eventType, Action<PointerEventData> callback)
        {
            switch (eventType)
            {
                case Game.Data.EventType.PointerDown:
                    onPointerDownEvent += callback;
                    break;
                case Game.Data.EventType.PointerUp:
                    onPointerUpEvent += callback;
                    break;
                case Game.Data.EventType.Drag:
                    onDragEvent += callback;
                    break;
            }
        }

        public void SubEvent(Game.Data.EventType eventType, Action<PointerEventData> callback)
        {
            switch (eventType)
            {
                case Game.Data.EventType.PointerDown:
                    onPointerDownEvent -= callback;
                    break;
                case Game.Data.EventType.PointerUp:
                    onPointerUpEvent -= callback;
                    break;
                case Game.Data.EventType.Drag:
                    onDragEvent -= callback;
                    break;
            }
        }

        public void ClearEvent(Game.Data.EventType eventType)
        {
            switch (eventType)
            {
                case Game.Data.EventType.PointerDown:
                    onPointerDownEvent = null;
                    break;
                case Game.Data.EventType.PointerUp:
                    onPointerUpEvent = null;
                    break;
                case Game.Data.EventType.Drag:
                    onDragEvent = null;
                    break;
            }
        }

        public void PlayEvent(Game.Data.EventType eventType, PointerEventData eventData)
        {
            switch (eventType)
            {
                case Game.Data.EventType.PointerDown:
                    onPointerDownEvent?.Invoke(eventData);
                    break;
                case Game.Data.EventType.PointerUp:
                    onPointerUpEvent?.Invoke(eventData);
                    break;
                case Game.Data.EventType.Drag:
                    onDragEvent?.Invoke(eventData);
                    break;
            }
        }
    }
}

