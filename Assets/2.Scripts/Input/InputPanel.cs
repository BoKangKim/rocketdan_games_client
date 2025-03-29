using System.Collections;
using System.Collections.Generic;
using Game.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    // 클릭 감지 패널
    public class InputPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            ManagerTable.InputManager.PlayEvent(Game.Data.EventType.Drag, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ManagerTable.InputManager.PlayEvent(Game.Data.EventType.PointerDown, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ManagerTable.InputManager.PlayEvent(Game.Data.EventType.PointerUp, eventData);
        }
    }
}

