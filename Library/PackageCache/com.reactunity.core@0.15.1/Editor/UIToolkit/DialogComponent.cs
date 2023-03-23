using System;

using ReactUnity.Editor.Renderer;
using ReactUnity.Styling.Converters;
using ReactUnity.UIToolkit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReactUnity.Editor.UIToolkit
{
    public class DialogComponent : UIToolkitComponent<DialogElement>
    {
        protected override string DefaultName => null;

        public override VisualElement TargetElement => Element.contentContainer;

        public DialogComponent(EditorContext context, string tag = "dialog") : base(context, tag)
        {
            Element.OnOpen += OnOpen;
            Element.OnClose += OnClose;
            Element.OnSelectionChanged += OnSelectionChange;
            Element.OnFocusChange += OnFocusChange;
            Element.OnVisibilityChange += OnVisibilityChange;
            Element.Context = context;
        }

        private void OnVisibilityChange(DialogWindow window, bool visible) => FireEvent("onVisibilityChange", visible);
        private void OnFocusChange(DialogWindow window, bool focused) => FireEvent("onFocusChange", focused);
        private void OnSelectionChange(DialogWindow window) => FireEvent("onSelectionChange", window);
        private void OnClose(DialogWindow window) => FireEvent("onClose", window);
        private void OnOpen(DialogWindow window) => FireEvent("onOpen", window);

        public void Open() => Element.Shown = true;
        public void Close() => Element.Shown = false;

        public override void SetProperty(string property, object value)
        {
            if (property == "show") Element.Shown = Convert.ToBoolean(value);
            else if (property == "title") Element.Title = Convert.ToString(value);
            else if (property == "maximized") Element.Maximized = Convert.ToBoolean(value);
            else if (property == "type")
            {
                var cv = AllConverters.Get<DialogType>().TryGetConstantValue(value, DialogType.Default);
                Element.Type = cv;
            }
            else base.SetProperty(property, value);
        }

        protected override void DestroySelf()
        {
            base.DestroySelf();
            Element.Close();
        }

        protected override void ApplyLayoutStylesSelf()
        {
            base.ApplyLayoutStylesSelf();
            Element.ResolveStyle();
        }
    }

    public enum DialogType
    {
        Default = 0,
        Modal = 1,
        Utility = 2,
        ModalUtility = 3,
        Aux = 4,
        Popup = 5,
        Tab = 6,
    }

    public class DialogWindow : EditorWindow
    {
        public event Action<DialogWindow> OnOpen;
        public event Action<DialogWindow> OnClose;
        public event Action<DialogWindow, bool> OnFocusChange;
        public event Action<DialogWindow> OnSelectionChanged;
        public event Action<DialogWindow, bool> OnVisibilityChange;

        public static DialogWindow Create()
        {
            return CreateWindow<DialogWindow>();
        }

        private void Awake()
        {
            OnOpen?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnClose?.Invoke(this);
        }

        private void OnFocus()
        {
            OnFocusChange?.Invoke(this, true);
        }

        private void OnLostFocus()
        {
            OnFocusChange?.Invoke(this, false);
        }

        private void OnSelectionChange()
        {
            OnSelectionChanged?.Invoke(this);
        }

        protected void OnBecameVisible()
        {
            OnVisibilityChange?.Invoke(this, true);
        }

        protected void OnBecameInvisible()
        {
            OnVisibilityChange?.Invoke(this, false);
        }
    }

    public class DialogElement : VisualElement
    {
        public static float DefaultMinWidth = 240;
        public static float DefaultMinHeight = 320;
        public static float DefaultMaxWidth = 10000;
        public static float DefaultMaxHeight = 10000;

        public event Action<DialogWindow> OnOpen;
        public event Action<DialogWindow> OnClose;
        public event Action<DialogWindow, bool> OnFocusChange;
        public event Action<DialogWindow> OnSelectionChanged;
        public event Action<DialogWindow, bool> OnVisibilityChange;

        public DialogWindow Window;

        public override VisualElement contentContainer { get; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                if (Window != null) Window.name = value;
            }
        }

        private bool maximized;
        public bool Maximized
        {
            get => maximized;
            set
            {
                maximized = value;
                if (Window != null) Window.maximized = value;
            }
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                if (Window != null) Window.titleContent = string.IsNullOrWhiteSpace(title) ? GUIContent.none : new GUIContent(title);
            }
        }

        private bool shown;
        public bool Shown
        {
            get => shown;
            set
            {
                var previousShown = shown;
                shown = value;
                if (value && !previousShown) ChangeType();
                else if (previousShown) Close();
            }
        }

        private DialogType type;
        public DialogType Type
        {
            get => type;
            set
            {
                var previousType = type;
                type = value;
                if (type != previousType) ChangeType();
            }
        }

        public EditorContext Context { get; internal set; }

        public DialogElement()
        {
            style.display = DisplayStyle.None;
            contentContainer = new VisualElement();

            RegisterCallback<DetachFromPanelEvent>(x => {
                Close();
            }, TrickleDown.TrickleDown);
        }

        private void CreateWindow()
        {
            var window = DialogWindow.Create();
            window.name = Name;
            window.titleContent = string.IsNullOrWhiteSpace(title) ? GUIContent.none : new GUIContent(title);
            window.maximized = Maximized;
            window.rootVisualElement.styleSheets.Add(ResourcesHelper.UtilityStylesheet);
            window.rootVisualElement.Add(contentContainer);

            window.OnOpen += (ev) => {
                shown = true;
                OnOpen?.Invoke(ev);
            };
            window.OnClose += (ev) => {
                shown = false;
                OnClose?.Invoke(ev);
            };
            window.OnSelectionChanged += (ev) => OnSelectionChanged?.Invoke(ev);
            window.OnFocusChange += (ev, val) => OnFocusChange?.Invoke(ev, val);
            window.OnVisibilityChange += (ev, val) => OnVisibilityChange?.Invoke(ev, val);

            Window = window;
            ResolveStyle();
        }

        public void Show(DialogType type)
        {
            if (contentContainer.parent != null) contentContainer.RemoveFromHierarchy();
            if (Window == null) CreateWindow();

            switch (type)
            {
                case DialogType.Modal:
                    Window.ShowModal();
                    break;
                case DialogType.Utility:
                    Window.ShowUtility();
                    break;
                case DialogType.ModalUtility:
                    Window.ShowModalUtility();
                    break;
                case DialogType.Aux:
                    Window.ShowAuxWindow();
                    break;
                case DialogType.Popup:
                    Window.ShowPopup();
                    break;
                case DialogType.Tab:
                    Window.ShowTab();
                    break;
                case DialogType.Default:
                default:
                    Window.Show();
                    break;
            }
        }

        public void Close()
        {
            if (contentContainer.parent != null) contentContainer.RemoveFromHierarchy();
            if (Window != null) Window.Close();
            Window = null;
        }

        private void ChangeType()
        {
            if (!shown) return;
            Show(Type);
        }

        public void ResolveStyle() => ResolveStyle(Context, Window, contentContainer.style);

        public static void ResolveStyle(EditorContext context, EditorWindow window, IStyle style)
        {
            if (!window) return;

            bool isDocked()
            {
#if UNITY_2020_1_OR_NEWER
                return window.docked;
#else
                var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                var method = typeof(EditorWindow).GetProperty("docked", flags).GetGetMethod(true);
                return (bool) method.Invoke(window, null);
#endif
            };

            if (!isDocked())
            {
                var scw = Screen.currentResolution.width / EditorGUIUtility.pixelsPerPoint;
                var sch = Screen.currentResolution.height / EditorGUIUtility.pixelsPerPoint;
                var xr = 0f;
                var yr = 0f;

                var isAbsolute = (style.position.keyword == StyleKeyword.Undefined && style.position.value == Position.Absolute)
                    || window == context.Window;

                if (!isAbsolute)
                {
                    if (context.Window)
                    {
                        var worldPos = context.Window.position;
                        scw = worldPos.width;
                        sch = worldPos.height;
                        xr = worldPos.x;
                        yr = worldPos.y;
                    }
                }

                // Calculate min and max sizes
                var minw = DefaultMinWidth;
                var minh = DefaultMinHeight;
                var maxw = DefaultMaxWidth;
                var maxh = DefaultMaxHeight;

                var minWidth = style.minWidth;
                var hasMinWidth = minWidth.keyword == StyleKeyword.Undefined;
                var minWidthVal = GetValueFromLength(minWidth, scw);
                if (hasMinWidth) minw = minWidthVal;

                var maxWidth = style.maxWidth;
                var hasMaxWidth = maxWidth.keyword == StyleKeyword.Undefined;
                var maxWidthVal = GetValueFromLength(maxWidth, scw);
                if (hasMaxWidth) maxw = maxWidthVal;

                var minHeight = style.minHeight;
                var hasMinHeight = minHeight.keyword == StyleKeyword.Undefined;
                var minHeightVal = GetValueFromLength(minHeight, sch);
                if (hasMinHeight) minh = minHeightVal;

                var maxHeight = style.maxHeight;
                var hasMaxHeight = maxHeight.keyword == StyleKeyword.Undefined;
                var maxHeightVal = GetValueFromLength(maxHeight, sch);
                if (hasMaxHeight) maxh = maxHeightVal;

                window.minSize = new Vector2(minw, minh);
                window.maxSize = new Vector2(maxw, maxh);


                // Calculate position and size
                var width = style.width;
                var height = style.height;
                var hasWidth = width.keyword == StyleKeyword.Undefined;
                var hasHeight = height.keyword == StyleKeyword.Undefined;

                var left = style.left;
                var top = style.top;
                var bottom = style.bottom;
                var right = style.right;

                var hasLeft = left.keyword == StyleKeyword.Undefined;
                var hasTop = top.keyword == StyleKeyword.Undefined;
                var hasBottom = bottom.keyword == StyleKeyword.Undefined;
                var hasRight = right.keyword == StyleKeyword.Undefined;

                if (hasLeft || hasRight || hasTop || hasBottom || hasWidth || hasHeight)
                {
                    var w = window.position.width;
                    var h = window.position.height;

                    if (hasWidth) w = GetValueFromLength(width, scw);
                    else if (hasLeft && hasRight) w = Math.Max(0, scw - GetValueFromLength(left, scw) - GetValueFromLength(right, scw));

                    if (hasHeight) h = GetValueFromLength(height, sch);
                    else if (hasTop && hasBottom) h = Math.Max(0, sch - GetValueFromLength(top, sch) - GetValueFromLength(bottom, sch));

                    var x = hasLeft ? GetValueFromLength(left, scw) :
                        hasRight ? scw - GetValueFromLength(right, scw) - w
                        : window.position.x;

                    var y = hasTop ? GetValueFromLength(top, sch) :
                        hasBottom ? sch - GetValueFromLength(bottom, sch) - h
                        : window.position.y;

                    var pos = new Vector2(x + xr, y + yr);
                    var size = new Vector2(w, h);
                    window.position = new Rect(pos, size);
                }
            }

            style.top = StyleKeyword.Initial;
            style.right = StyleKeyword.Initial;
            style.bottom = StyleKeyword.Initial;
            style.left = StyleKeyword.Initial;
            style.width = StyleKeyword.Initial;
            style.height = StyleKeyword.Initial;
            style.minWidth = StyleKeyword.Initial;
            style.minHeight = StyleKeyword.Initial;
            style.maxWidth = StyleKeyword.Initial;
            style.maxHeight = StyleKeyword.Initial;
        }

        private static float GetValueFromLength(StyleLength len, float fullSize)
        {
            var val = len.value.value;
            if (len.value.unit == LengthUnit.Percent) val *= fullSize / 100;
            return val;
        }
    }
}
