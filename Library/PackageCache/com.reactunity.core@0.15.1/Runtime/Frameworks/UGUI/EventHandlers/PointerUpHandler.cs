using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReactUnity.UGUI.EventHandlers
{
    [EventHandlerPriority(EventPriority.Discrete)]
    public class PointerUpHandler : MonoBehaviour, IPointerUpHandler, IEventHandler
    {
        public event Action<BaseEventData> OnEvent = default;

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEvent?.Invoke(eventData);
        }

        public void ClearListeners()
        {
            OnEvent = null;
        }
    }
}
