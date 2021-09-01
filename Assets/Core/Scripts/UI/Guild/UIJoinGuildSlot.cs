using UnityEngine;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIJoinGuildSlot : MonoBehaviour {
        public RTLTextMeshPro Name;
        public RTLTextMeshPro master;
        public TMP_Text level;
        public TMP_Text reqLevel;
        public TMP_Text members;
        public GameObject autoAccept;
        uint id;
        public void Set(GuildJoinInfo info) {
            id = info.id;
            Name.text = info.name;
            master.text = info.masterName;
            level.text = info.level.ToString();
            reqLevel.text = info.requiredLevel > 0 ? info.requiredLevel.ToString() : LanguageManger.Decide("Any Level", "اي مستوي");
            members.text = info.membersCount + " / " + info.capacity;
            autoAccept.SetActive(info.autoAccept);
        }
        public void OnJoin() {
            if(id != 0) Player.localPlayer.CmdSendJoinRequestToGuild(id);
            else Notify.SomethingWentWrong();
        }
    }
}