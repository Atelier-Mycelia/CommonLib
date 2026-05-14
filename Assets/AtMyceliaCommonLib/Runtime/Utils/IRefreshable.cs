namespace AtMycelia
{
    public interface IRefreshable
    {
        void Refresh();
    }

    public interface IOnPreCutHandler
    {
        /// <summary>
        /// Meant to be executed right before this instance is to be cut. This is useful for 
        /// performing any necessary cleanup before the instance is removed from the scene.
        /// </summary>
        void OnPreCut();
    }

    public interface IOnPostPasteHandler
    {
        /// <summary>
        /// Meant to execute right after this instance is pasted. This is useful for
        /// performing any necessary setup after the instance is added to the scene.
        /// 
        void OnPostPaste();
    }
}