using UnityEngine;

namespace AtMycelia.Common.InputUtils
{
    /// <summary>
    /// Makes it so that input-handling will work regardless of whether the user
    /// has the new input system installed.
    /// </summary>
    public abstract class InputSource : ScriptableObject
    {
        // Override the public funcs in subclases to decide how the inputs are listened for.
        public virtual void Init()
        {
            _active = true;
        }

        protected bool _active;

        public virtual void Deinit()
        {
            _active = false;
        }

        public virtual void Update()
        {
        }

        /// <summary>
        /// Silently returns if this instance is not active.
        /// </summary>
        protected virtual void RaiseOnButtonDown()
        {
            if (!_active)
            {
                return;
            }
            ButtonDown.Invoke();
        }

        public event System.Action ButtonDown = delegate { };
    }
}