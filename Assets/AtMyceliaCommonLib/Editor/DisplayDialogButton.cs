using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace AtMycelia.EditorExt
{
    /// <summary>
    /// Handles wiring up a button, showing a Display Dialog prompt. Responses to the results of
    /// the user's choice in said prompt are handled via events. This class is disposable, and
    /// will unsubscribe from the button's clicked event when disposed.
    /// </summary>
    public sealed class DisplayDialogButton : IDisposable
    {
        /// <summary>
        /// Sets up this instance to listen for clicks on the given Button, and decide what to show in the
        /// DisplayDialog. The shouldBringUp field decides whether to show the dialog in response to the
        /// button being clicked. The default behavior is to always show the dialog.
        /// </summary>
        public void Init(Button toListenFor, DisplayDialogArgs dialogArgs,
            Func<bool> shouldBringUp = null)
        {
            _toListenFor = toListenFor ?? throw new ArgumentNullException(nameof(toListenFor));
            _dialogArgs = dialogArgs;
            _shouldBringUp = shouldBringUp ?? (() => true);
            SetSubs(true);
            _isDisposed = false;
        }

        private Button _toListenFor;
        private DisplayDialogArgs _dialogArgs;
        private Func<bool> _shouldBringUp;

        private void SetSubs(bool on)
        {
            if (on)
            {
                _toListenFor.clicked += OnButtonClicked;
            }
            else
            {
                _toListenFor.clicked -= OnButtonClicked;
            }
        }

        private void OnButtonClicked()
        {
            if (!_shouldBringUp())
            {
                BringUpDenied();
                return;
            }

            if (!ConfirmSelection())
            {
                PromptDenied();
                return;
            }

            PromptConfirmed();
        }

        /// <summary>
        /// Occurs when a bring-up request is denied.
        /// </summary>
        /// <remarks>Initialized to an empty delegate to allow invocation without null checks.</remarks>
        public event Action BringUpDenied = delegate { };

        /// <summary>
        /// Isolated so it's easy to override behavior (e.g. in tests) without
        /// needing to mock out EditorUtility.
        /// </summary>
        private bool ConfirmSelection()
        {
            return EditorUtility.DisplayDialog(_dialogArgs.title, _dialogArgs.message,
                _dialogArgs.acceptanceText, _dialogArgs.denialText);
        }

        /// <summary>
        /// Occurs when the user chooses the confirmation option in the prompt.
        /// </summary>
        public event Action PromptConfirmed = delegate { };

        /// <summary>
        /// Occurs when the user chooses the denial option in the prompt.
        /// </summary>
        public event Action PromptDenied = delegate { };

        private bool _isDisposed = true;
        //^ Defaults to true because this represents whether or not the object is
        // currently tied to a button. If it is not tied to a button, it is considered disposed.

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            SetSubs(false);
            if (ClearEventsOnDispose)
            {
                PromptConfirmed = delegate { };
                PromptDenied = delegate { };
                BringUpDenied = delegate { };
            }

            _toListenFor = null;
            _isDisposed = true;
        }

        public bool ClearEventsOnDispose { get; set; } = true;
    }

}

