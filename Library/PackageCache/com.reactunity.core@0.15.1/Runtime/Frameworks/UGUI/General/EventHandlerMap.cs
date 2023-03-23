using System;
using System.Collections.Generic;
using ReactUnity.UGUI.EventHandlers;

namespace ReactUnity.UGUI
{
    public static class EventHandlerMap
    {
        static Dictionary<string, Type> EventMap = new Dictionary<string, Type>
        {
            { "onPointerClick", typeof(PointerClickHandler) },
            { "onPointerUp", typeof(PointerUpHandler) },
            { "onPointerDown", typeof(PointerDownHandler) },
            { "onPointerEnter", typeof(PointerEnterHandler) },
            { "onPointerExit", typeof(PointerExitHandler) },
            { "onPointerMove", typeof(PointerMoveHandler) },
            { "onSubmit", typeof(SubmitHandler) },
            { "onCancel", typeof(CancelHandler) },
            { "onSelect", typeof(SelectHandler) },
            { "onDeselect", typeof(DeselectHandler) },
            { "onMove", typeof(MoveHandler) },
            { "onUpdateSelected", typeof(UpdateSelectedHandler) },
            { "onScroll", typeof(ScrollHandler) },
            { "onDrag", typeof(DragHandler) },
            { "onBeginDrag", typeof(BeginDragHandler) },
            { "onEndDrag", typeof(EndDragHandler) },
            { "onPotentialDrag", typeof(PotentialDragHandler) },
            { "onDrop", typeof(DropHandler) },
            { "onKeyDown", typeof(KeyDownHandler) },
            { "onResize", typeof(ResizeHandler) },

            // Custom events
            { "onDoubleClick", typeof(DoubleClickHandler) },
            { "onContextMenu", typeof(ContextMenuHandler) },

            // Aliases
            { "onClick", typeof(PointerClickHandler) },
            { "onMouseUp", typeof(PointerUpHandler) },
            { "onMouseDown", typeof(PointerDownHandler) },
            { "onMouseEnter", typeof(PointerEnterHandler) },
            { "onMouseLeave", typeof(PointerExitHandler) },
            { "onMouseOver", typeof(PointerEnterHandler) },
            { "onMouseOut", typeof(PointerExitHandler) },
            { "onMouseMove", typeof(PointerMoveHandler) },
            { "onFocus", typeof(SelectHandler) },
            { "onBlur", typeof(DeselectHandler) },
        };

        public static Type GetEventType(string eventName)
        {
            if (EventMap.TryGetValue(eventName, out var res)) return res;
            return null;
        }
    }
}
