using UnityEngine;
using TMPro;

namespace AtMycelia.Common.UI
{
    public class CharaCountDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _charaCountDisplay;
        [SerializeField] private TMP_InputField _inputField;

        protected virtual void OnEnable()
        {
            UpdateCharaCountDisplay(_inputField.text);
            ToggleSubs(true);
        }

        private void UpdateCharaCountDisplay(string text)
        {
            _charaCountDisplay.text = $"{text.Length}/{_inputField.characterLimit}";
        }

        protected virtual void ToggleSubs(bool on)
        {
            if (on)
            {
                _inputField.onValueChanged.AddListener(UpdateCharaCountDisplay);
            }
            else
            {
                _inputField.onValueChanged.RemoveListener(UpdateCharaCountDisplay);
            }
        }

        protected virtual void OnDisable()
        {
            ToggleSubs(false);
        }

    }
}