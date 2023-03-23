using System;
using UnityEngine.UIElements;

namespace ReactUnity.UIToolkit
{
    public class BaseSliderComponent<S, TValueType> : BaseFieldComponent<S, TValueType> where S : BaseSlider<TValueType>, new() where TValueType : System.IComparable<TValueType>
    {
        public BaseSliderComponent(UIToolkitContext context, string tag) : base(context, tag)
        { }

        public override void SetProperty(string property, object value)
        {
            switch (property)
            {
                case "vertical":
                    Element.direction = Convert.ToBoolean(value) ? SliderDirection.Vertical : SliderDirection.Horizontal;
                    break;
                case "inverted":
#if UNITY_2021_1_OR_NEWER
                    Element.inverted = Convert.ToBoolean(value);
#endif
                    break;
                case "showInput":
#if UNITY_2020_1_OR_NEWER
                    Element.showInputField = Convert.ToBoolean(value);
#endif
                    break;
                case "step":
                    Element.pageSize = Convert.ToSingle(value);
                    break;
                case "min":
                    Element.lowValue = (TValueType) Convert.ChangeType(value, typeof(TValueType));
                    break;
                case "max":
                    Element.highValue = (TValueType) Convert.ChangeType(value, typeof(TValueType));
                    break;
                default:
                    base.SetProperty(property, value);
                    break;
            }
        }
    }
}
