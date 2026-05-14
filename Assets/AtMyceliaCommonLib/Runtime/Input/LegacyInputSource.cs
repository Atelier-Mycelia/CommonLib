using UnityEngine;

namespace AtMycelia.Common.InputUtils
{
    [CreateAssetMenu(fileName = "LegacyInputSource",
        menuName = "Atelier Mycelia/Input/Legacy Input Source",
        order = 0)]
    public class LegacyInputSource : InputSource
    {
        [SerializeField] private string[] _axes = new string[] { };

        public override void Update()
        {
            foreach (var axis in _axes)
            {
                if (Input.GetButtonDown(axis))
                {
                    RaiseOnButtonDown();
                    break;
                }
            }
        }
    }
}