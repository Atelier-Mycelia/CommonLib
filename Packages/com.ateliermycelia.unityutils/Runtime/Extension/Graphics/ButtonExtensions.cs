using UnityEngine.UI;

namespace AtMycelia.Common.UI
{
    public static class ButtonExtensions
    {
        /// <summary>
        /// Sets whether the button and its target graphic respond to pointer events. Click,
        /// hover, and so on
        /// </summary>
        public static void SetRespondsToPointer(this Button button, bool responds)
        {
            button.interactable = button.targetGraphic.raycastTarget = responds;
        }
    }
}