using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace AtMycelia.EditorExt
{
    /// <summary>
    /// Handles wiring up the a button, showing a Display Dialog and executing an action
    /// upon confirmation.
    /// </summary>
    public sealed class DisplayDialogButton : IDisposable
    {
        public void Init(Button reinitButton, Action onConfirm, DisplayDialogArgs dialogArgs)
        {
            _reinitButton = reinitButton ?? throw new ArgumentNullException(nameof(reinitButton));
            _onReinitConfirmed = onConfirm ?? throw new ArgumentNullException(nameof(onConfirm));
            _dialogArgs = dialogArgs;
            SetSubs(true);
            _isDisposed = false;
        }

        private Button _reinitButton;
        private Action _onReinitConfirmed;
        private string _dialogMessage;
        private bool _isDisposed = true;
        private DisplayDialogArgs _dialogArgs;

        private void SetSubs(bool on)
        {
            if (_reinitButton == null)
            {
                return;
            }

            if (on)
            {
                _reinitButton.clicked += OnButtonClicked;
            }
            else
            {
                _reinitButton.clicked -= OnButtonClicked;
            }
        }

        private void OnButtonClicked()
        {
            if (!ConfirmReinit())
            {
                return;
            }

            _onReinitConfirmed.Invoke();
        }

        /// <summary>
        /// Isolated so it's easy to override behavior (e.g. in tests) without
        /// needing to mock out EditorUtility.
        /// </summary>
        private bool ConfirmReinit()
        {
            //return EditorUtility.DisplayDialog(
            //    "Reinit All Entries",
            //    "This will reset every entry in this Control Panel back to its " +
            //    "startup state, discarding any unsaved runtime changes. Continue?",
            //    "Reinit",
            //    "Cancel");
            return EditorUtility.DisplayDialog(
                _dialogArgs.title,
                _dialogArgs.message,
                _dialogArgs.okText,
                _dialogArgs.cancelText);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            SetSubs(false);
            _reinitButton = null;
            _onReinitConfirmed = null;
            _isDisposed = true;
        }
    }

    
}
