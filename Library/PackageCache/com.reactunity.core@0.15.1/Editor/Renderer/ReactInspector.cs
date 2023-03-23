using ReactUnity.Editor.UIToolkit;
using ReactUnity.Helpers;
using ReactUnity.Styling.Rules;
using UnityEngine.UIElements;

namespace ReactUnity.Editor.Renderer
{
    public abstract class ReactInspector : UnityEditor.Editor
    {
#if REACT_UNITY_DEVELOPER
        protected bool DevServerEnabled
        {
            get
            {
                return UnityEditor.EditorPrefs.GetBool($"ReactUnity.Editor.ReactInspector.{GetType().Name}.DevServerEnabled");
            }
            set
            {
                UnityEditor.EditorPrefs.SetBool($"ReactUnity.Editor.ReactInspector.{GetType().Name}.DevServerEnabled", value);
            }
        }
#endif

        public override VisualElement CreateInspectorGUI()
        {
            var el = new ReactUnityEditorElement(GetScript(), GetGlobals(), null, DefaultMediaProvider.CreateMediaProvider("inspector", "uitoolkit", true));
            el.Inspector = this;
            el.Run();
            return el;
        }

        protected abstract ScriptSource GetScript();

        protected virtual SerializableDictionary GetGlobals()
        {
            return new SerializableDictionary()
            {
                { "Inspector", this },
            };
        }
    }
}
