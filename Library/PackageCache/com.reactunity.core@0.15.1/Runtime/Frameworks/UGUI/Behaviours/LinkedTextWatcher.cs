using UnityEngine;

namespace ReactUnity.UGUI.Behaviours
{
    public class LinkedTextWatcher : MonoBehaviour
    {
        public TextComponent WatchedText { get; internal set; }
        public TextComponent LinkedText { get; internal set; }

        void Update()
        {
            var enableLink = WatchedText.ComputedStyle.textOverflow == TMPro.TextOverflowModes.Linked &&
                WatchedText.Text.isTextOverflowing &&
                WatchedText.Text.firstOverflowCharacterIndex > 6;
            // The threshold (6) prevents an endless loop bug in TMPro

            if (enableLink && LinkedText == null)
            {
                LinkedText = new TextComponent(WatchedText);
                WatchedText.Text.linkedTextComponent = LinkedText.Text;
            }
            else if (!enableLink && LinkedText != null)
            {
                LinkedText.Destroy(false);
                LinkedText = null;
                WatchedText.Text.linkedTextComponent = null;
            }
        }
    }
}
