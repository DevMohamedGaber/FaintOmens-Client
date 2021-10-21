using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    [System.Serializable]
    public struct Status_Info
    {
        public RTLTextMeshPro nameTxt;
        public TMP_Text idTxt;
        public RTLTextMeshPro classTxt;
        public RTLTextMeshPro guildTxt;
        public RTLTextMeshPro spouseTxt;
        public TMP_Text honorTxt;
        public TMP_Text brTxt;
        public UIProgressBar expBar;
        public TMP_Text expTxt;
        public TMP_Text lvlTxt;
        public TMP_Text vipTxt;
        public Image avatar;
        public Image title;
    }
}