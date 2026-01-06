using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace AtMycelia.Common.InputUtils
{
    [CreateAssetMenu(fileName = "ModernInputSource",
        menuName = "Atelier Mycelia/Input/Modern Input Source",
        order = 1)]
    public class ModernInputSource : InputSource
    {
        // So that we can package this with the main utils without having to create a separate 
        // Integrations folder.
#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputActionReference[] _actions = new InputActionReference[0];

        public override void Init()
        {
            base.Init();
            ToggleSubs(true);
        }

        protected virtual void ToggleSubs(bool on)
        {
            foreach (var actionRef in _actions)
            {
                if (on)
                {
                    actionRef.action.Enable();
                    actionRef.action.performed += OnActionPerformed;
                }
                else
                {
                    actionRef.action.performed -= OnActionPerformed;
                }
            }
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            RaiseOnButtonDown();
        }

        public override void Deinit()
        {
            base.Deinit();
            ToggleSubs(false);
        }
#endif
    }
}