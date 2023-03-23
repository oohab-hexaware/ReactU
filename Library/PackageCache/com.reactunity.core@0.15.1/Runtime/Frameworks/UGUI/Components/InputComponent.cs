using System;
using Facebook.Yoga;

using ReactUnity.Helpers;
using ReactUnity.Styling.Converters;
using ReactUnity.UGUI.Behaviours;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ReactUnity.UGUI
{
    public class InputComponent : UGUIComponent, IInputComponent
    {
        public string Value
        {
            get => InputField.text;
            set
            {
                InputField.SetTextWithoutNotify(value);
                MarkForStyleResolvingWithSiblings(true);
            }
        }

        public bool Disabled
        {
            get => !InputField.interactable;
            set
            {
                InputField.interactable = !value;
                MarkForStyleResolvingWithSiblings(true);
            }
        }

        public bool ReadOnly
        {
            get => InputField.readOnly;
            set
            {
                InputField.readOnly = value;
                MarkForStyleResolvingWithSiblings(true);
            }
        }

        private string placeholder;
        public string Placeholder
        {
            get => placeholder;
            set
            {
                placeholder = value;
                PlaceholderComponent.SetText(placeholder);
                MarkForStyleResolvingWithSiblings(true);
            }
        }

        public bool PlaceholderShown => string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(Placeholder);

        public TMP_InputField InputField { get; private set; }
        public ContainerComponent TextViewport { get; set; }
        public TextComponent TextComponent { get; set; }
        public TextComponent PlaceholderComponent { get; set; }
        public ScrollbarComponent VerticalScrollbar { get; private set; }

        public InputComponent(string text, UGUIContext context) : base(context, "input")
        {
            AddComponent<ScrollEventBubbling>();
            InputField = AddComponent<TMP_InputField>();
            SetupContents();
            SetText(text);
        }

        private void SetupContents()
        {
            // Input field's properties must be fully assigned before OnEnable is called
            GameObject.SetActive(false);
            TextViewport = Context.CreateDefaultComponent("_viewport", null) as ContainerComponent;
            TextViewport.IsPseudoElement = true;
            TextViewport.Name = "[Text Viewport]";
            TextViewport.SetParent(this);
            TextViewport.GetOrAddComponent<RectMask2D>();


            PlaceholderComponent = Context.CreateText("_placeholder") as TextComponent;
            PlaceholderComponent.IsPseudoElement = true;
            PlaceholderComponent.Name = "[Placeholder]";
            PlaceholderComponent.SetParent(TextViewport);
            PlaceholderComponent.Component.enabled = false;
            var phRect = PlaceholderComponent.RectTransform;
            phRect.pivot = Vector2.one / 2;
            phRect.anchorMin = Vector2.zero;
            phRect.anchorMax = Vector2.one;
            phRect.sizeDelta = Vector2.zero;
            phRect.offsetMin = Vector2.zero;
            phRect.offsetMax = Vector2.zero;



            TextComponent = Context.CreateText("_value") as TextComponent;
            TextComponent.IsPseudoElement = true;
            TextComponent.Name = "[Text]";
            TextComponent.SetParent(TextViewport);
            TextComponent.Component.enabled = false;
            var textRect = TextComponent.RectTransform;
            textRect.pivot = Vector2.one / 2;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;


            InputField.textViewport = TextViewport.RectTransform;
            InputField.textComponent = TextComponent.Text;
            InputField.placeholder = PlaceholderComponent.Text;
            InputField.fontAsset = TextComponent.Text.font;

            GameObject.SetActive(true);
        }

        public void SetText(string text)
        {
            InputField.text = text;
        }

        protected override void ApplyLayoutStylesSelf()
        {
            base.ApplyLayoutStylesSelf();
            if (Layout.Overflow == YogaOverflow.Scroll)
            {
                if (VerticalScrollbar == null) VerticalScrollbar = CreateScrollbar();
            }
            else
            {
                if (VerticalScrollbar != null)
                {
                    VerticalScrollbar.Destroy();
                    VerticalScrollbar = null;
                    InputField.verticalScrollbar = null;
                }
            }
        }

        private ScrollbarComponent CreateScrollbar()
        {
            var sc = Context.CreateComponentWithPool("_scrollbar", null, (tag, text) => new ScrollbarComponent(Context));
            sc.Horizontal = false;
            sc.Inverted = true;
            sc.SetParent(this);

            InputField.verticalScrollbar = sc.Scrollbar;

            return sc;
        }

        protected override void ApplyStylesSelf()
        {
            base.ApplyStylesSelf();
            InputField.pointSize = ComputedStyle.fontSize;
        }

        public void Focus()
        {
            InputField.Select();
        }

        public void Activate()
        {
            Focus();
        }

        public override Action AddEventListener(string eventName, Callback callback)
        {
            switch (eventName)
            {
                case "onEndEdit":
                    var l1 = new UnityAction<string>(x => callback.CallWithPriority(EventPriority.Discrete, x, this));
                    InputField.onEndEdit.AddListener(l1);
                    return () => InputField.onEndEdit.RemoveListener(l1);
                case "onReturn":
                    var l2 = new UnityAction<string>(x => callback.CallWithPriority(EventPriority.Discrete, x, this));
                    InputField.onSubmit.AddListener(l2);
                    return () => InputField.onSubmit.RemoveListener(l2);
                case "onChange":
                    var l3 = new UnityAction<string>(x => callback.CallWithPriority(EventPriority.Discrete, x, this));
                    InputField.onValueChanged.AddListener(l3);
                    return () => InputField.onValueChanged.RemoveListener(l3);
                case "onTextSelection":
                    var l4 = new UnityAction<string, int, int>((x, i, j) => callback.CallWithPriority(EventPriority.Discrete, x, i, j, this));
                    InputField.onTextSelection.AddListener(l4);
                    return () => InputField.onTextSelection.RemoveListener(l4);
                case "onEndTextSelection":
                    var l5 = new UnityAction<string, int, int>((x, i, j) => callback.CallWithPriority(EventPriority.Discrete, x, i, j, this));
                    InputField.onEndTextSelection.AddListener(l5);
                    return () => InputField.onEndTextSelection.RemoveListener(l5);
                default:
                    return base.AddEventListener(eventName, callback);
            }
        }

        public override void SetProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "placeholder":
                    Placeholder = value?.ToString() ?? "";
                    return;
                case "value":
                    Value = value?.ToString() ?? "";
                    return;
                case "disabled":
                    Disabled = Convert.ToBoolean(value);
                    return;
                case "characterLimit":
                    InputField.characterLimit = Convert.ToInt32(value);
                    return;
                case "lineLimit":
                    InputField.lineLimit = Convert.ToInt32(value);
                    return;
                case "readonly":
                    InputField.readOnly = Convert.ToBoolean(value);
                    return;
                case "richText":
                    InputField.richText = Convert.ToBoolean(value);
                    return;
                case "contentType":
                    var val = AllConverters.Get<TMP_InputField.ContentType>().TryGetConstantValue(value, TMP_InputField.ContentType.Standard);
                    InputField.contentType = val;
                    Value = Value; // Workaround to fix password switching bug
                    return;
                case "keyboardType":
                    var val2 = AllConverters.Get<TouchScreenKeyboardType>().TryGetConstantValue(value, TouchScreenKeyboardType.Default);
                    InputField.keyboardType = val2;
                    return;
                case "lineType":
                    var val3 = AllConverters.Get<TMP_InputField.LineType>().TryGetConstantValue(value, TMP_InputField.LineType.SingleLine);
                    InputField.lineType = val3;
                    return;
                case "validation":
                    var val4 = AllConverters.Get<TMP_InputField.CharacterValidation>().TryGetConstantValue(value, TMP_InputField.CharacterValidation.None);
                    InputField.characterValidation = val4;
                    return;
                default:
                    base.SetProperty(propertyName, value);
                    break;
            }
        }

        public override bool Pool()
        {
            if (!base.Pool()) return false;
            VerticalScrollbar = null;

            return true;
        }

        public override bool Revive()
        {
            if (!base.Revive()) return false;

            SetupContents();
            Value = string.Empty;
            Placeholder = string.Empty;
            Disabled = false;
            ReadOnly = false;

            return true;
        }
    }
}
