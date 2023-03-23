using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReactUnity.UGUI.EventHandlers
{
    [EventHandlerPriority(EventPriority.Continuous)]
    public class PointerEnterHandler : MonoBehaviour, IPointerEnterHandler, IEventHandler
    {
        public event Action<BaseEventData> OnEvent = default;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEvent?.Invoke(eventData);
        }

        public void ClearListeners()
        {
            OnEvent = null;
        }
    }
}
