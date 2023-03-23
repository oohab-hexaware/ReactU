using System;
using ReactUnity.Helpers;
using UnityEngine.UIElements;

namespace ReactUnity.UIToolkit
{
    public class ValueComponent<TElementType, TValueType> : BindableComponent<TElementType>, IActivatableComponent where TElementType : VisualElement, IBindable, INotifyValueChanged<TValueType>, new()
    {
        public TValueType Value
        {
            get => Element.value;
            set => Element.SetValueWithoutNotify(value);
        }

        public ValueComponent(UIToolkitContext context, string tag) : base(context, tag)
        {
        }

        public override Action AddEventListener(string eventName, Callback callback)
        {
            switch (eventName)
            {
                case "onChange":
                    EventCallback<ChangeEvent<TValueType>> listener = (ev) => callback.CallWithPriority(EventPriority.Discrete, ev, this);
                    Element.RegisterValueChangedCallback(listener);
                    return () => Element.UnregisterValueChangedCallback(listener);
                default:
                    return base.AddEventListener(eventName, callback);
            }
        }

        public override void SetProperty(string property, object value)
        {
            if (property == "value") Value = ConvertValue(value);
            else base.SetProperty(property, value);
        }

        public TValueType ConvertValue(object value)
        {
            if (value == null) return default;
            if (value is TValueType val) return val;
            return (TValueType) Convert.ChangeType(value, typeof(TValueType));
        }

        public void SetValue(TValueType value)
        {
            Element.value = value;
        }

        public void SetValueWithoutNotify(TValueType value)
        {
            Element.SetValueWithoutNotify(value);
        }

        public override void Activate()
        {
            Element.Focus();
        }
    }
}
