using UnityEngine;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class GuildJoinInfoRow : MonoBehaviour
    {
        [SerializeField] RTLTextMeshPro nameTxt;
        [SerializeField] RTLTextMeshPro masterName;
        [SerializeField] TMP_Text level;
        [SerializeField] TMP_Text reqLevel;
        [SerializeField] TMP_Text members;
        [SerializeField] TMP_Text br;
        [SerializeField] GameObject autoAccept;
        [SerializeField] UIToggle toggle;
        public void Set(int index, GuildJoinOrCreate window)
        {
            GuildJoinInfo info = window.data[index];
            if(nameTxt != null)
            {
                nameTxt.text = info.name;
            }
            if(masterName != null)
            {
                masterName.text = info.masterName;
            }
            if(level != null)
            {
                level.text = info.level.ToString();
            }
            if(reqLevel)
            {
                reqLevel.text = info.requiredLevel > 0 ? info.requiredLevel.ToString() : Storage.data.guild.minJoinLevel.ToString();
            }
            if(members != null)
            {
                members.text = info.membersCount + " / " + info.capacity;
            }
            if(br != null)
            {
                br.text = info.br.ToString();
            }
            if(autoAccept != null)
            {
                autoAccept.SetActive(info.autoAccept);
            }
            if(toggle != null)
            {
                toggle.onSelect = () => window.SelectChanged();
            }
        }
    }
}