using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
namespace Game.UI
{
    public class UIArenaMatchNotifyOpponent : MonoBehaviour
    {
        [SerializeField] Image avatar;
        [SerializeField] Image tribeFlag;
        [SerializeField] RTLTextMeshPro nameTxt;
        [SerializeField] RTLTextMeshPro classTxt;
        public void Set(Player data)
        {
            avatar.sprite = UIManager.data.assets.avatars[data.avatar];

            nameTxt.text = $"{data.name} Lvl.{data.level}";

            if(ScriptableTribe.dict.TryGetValue(data.tribeId, out ScriptableTribe tribe))
            {
                tribeFlag.gameObject.SetActive(true);
                tribeFlag.sprite = tribe.flag;
            }
        }
    }
}