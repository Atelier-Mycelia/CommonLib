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
        /// <summary>
        /// Initializes internal references, subscribes to required events, and marks the instance as not disposed.
        /// The shouldBringUp function is optional and can be used to decide whether or not to bring up the dialog
        /// in response to the button-click. If not provided, it defaults to always returning true.
        /// </summary>
        /// <remarks>Subscribes to button events (SetSubs(true)) and clears the disposal state.</remarks>
        /// <param name="reinitButton">Button that triggers reinitialization and whose event subscriptions will be managed.</param>
        /// <param name="onConfirm">Callback invoked when the reinitialization is confirmed.</param>
        /// <param name="dialogArgs">Optional dialog configuration values for the display logic.</param>
        /// <exception cref="ArgumentNullException">Thrown when reinitButton or onConfirm is null.</exception>
        public void Init(Button reinitButton, Action onConfirm,
            DisplayDialogArgs dialogArgs, Func<bool> shouldBringUp = null)
        {
            _reinitButton = reinitButton ?? throw new ArgumentNullException(nameof(reinitButton));
            _onConfirm = onConfirm ?? throw new ArgumentNullException(nameof(onConfirm));
            _dialogArgs = dialogArgs;
            _shouldBringUp = shouldBringUp ?? (() => true);
            SetSubs(true);
            _isDisposed = false;
        }

        private Button _reinitButton;
        private Action _onConfirm;
        private bool _isDisposed = true;
        private DisplayDialogArgs _dialogArgs;
        private Func<bool> _shouldBringUp;

        private void SetSubs(bool on)
        {
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
            if (!_shouldBringUp())
            {
                return;
            }

            if (!ConfirmSelection())
            {
                return;
            }

            _onConfirm.Invoke();
        }

        /// <summary>
        /// Isolated so it's easy to override behavior (e.g. in tests) without
        /// needing to mock out EditorUtility.
        /// </summary>
        private bool ConfirmSelection()
        {
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
            _onConfirm = null;
            _isDisposed = true;
        }
    }

}
