using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UICharacterCreate_TribeToggle : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        public UIToggle toggle;
        public void Set(ScriptableTribe tribe)
        {
            icon.sprite = tribe.flag;
            nameTxt.text = tribe.Name;
        } 
    }
}