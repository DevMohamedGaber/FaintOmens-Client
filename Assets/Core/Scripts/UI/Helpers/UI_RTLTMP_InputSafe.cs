using UnityEngine;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UI_RTLTMP_InputSafe : MonoBehaviour
    {
        [SerializeField] RTLTextMeshPro text;
        [SerializeField] RTLTextMeshPro placeholder;
        void Awake()
        {
            TMP_InputField input = GetComponent<TMP_InputField>();
            if(input)
            {
                input.textComponent = text;
                input.placeholder = placeholder;
            }
        }
    }
}