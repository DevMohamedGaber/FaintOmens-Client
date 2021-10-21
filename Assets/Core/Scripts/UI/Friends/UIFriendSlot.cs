using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;

namespace Game.UI
{
    public class UIFriendSlot : MonoBehaviour {
        public Image avatar;
        public Image tribeFlag;
        public Image classIcon;
        public RTLTextMeshPro name;
        public RTLTextMeshPro level;
        public RTLTextMeshPro status;
        public GameObject overlay;
        public Button button;
        public void Set(Friend info) {
            avatar.sprite = UIManager.data.assets.avatars[info.avatar];
            tribeFlag.sprite = ScriptableTribe.dict[info.tribe].flag;
            classIcon.sprite = info.classInfo.data.icon;
            name.text = info.name;
            level.text = $"{LanguageManger.GetWord(26)}: {info.level}";
            status.text = !info.IsOnline() ? UIUtils.PrettySeconds(info.lastOnline) : "";
            overlay.SetActive(info.lastOnline > 0);
        }
    }
}