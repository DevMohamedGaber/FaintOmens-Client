using UnityEngine;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    [System.Serializable]
    public struct GuildHome_Info
    {
        public RTLTextMeshPro nameTxt;
        public RTLTextMeshPro masterName;
        public TMP_Text level;
        public TMP_Text id;
        public TMP_Text membersCount;
        public TMP_Text brTxt;
        public RTLTextMeshPro notice;
    }
}