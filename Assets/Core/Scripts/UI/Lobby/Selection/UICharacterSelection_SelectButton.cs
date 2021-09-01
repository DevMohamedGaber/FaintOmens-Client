using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UICharacterSelection_SelectButton : MonoBehaviour
    {
        [SerializeField] Image avatar;
        [SerializeField] Image classIcon;
        [SerializeField] RTLTMPro.RTLTextMeshPro Name;
        [SerializeField] TMPro.TMP_Text level;
        [SerializeField] UIToggle button;
        public void Set(Network.Messages.CharactersAvailable.CharacterPreview info, int index)
        {
            classIcon.sprite = info.classInfo.data.icon;
            avatar.sprite = Storage.data.avatars[(int)info.avatar];
            Name.text = info.name;
            level.text = info.level.ToString();
            button.onSelect = () => UIManager.data.lobby.select.OnSelect(index);
        }
    }
}