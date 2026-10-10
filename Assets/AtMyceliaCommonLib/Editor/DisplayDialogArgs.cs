namespace AtMycelia.EditorExt
{
    public struct DisplayDialogArgs
    {
        public string title, message;

        /// <summary>
        /// Text displayed as the label or message for an acceptance control, such as an OK button.
        /// </summary>
        public string acceptanceText;

        /// <summary>
        /// Text displayed as the label or message for a denial control, such as a Cancel button.
        /// </summary>
        /// <remarks>Intended for display to end users; may be null or empty.</remarks>
        public string denialText;
    }
}