using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIGuildSkillRow : MonoBehaviour {
        [SerializeField] TMP_Text lvlTxt;
        [SerializeField] TMP_Text currentPoints;
        [SerializeField] TMP_Text nextPoints;
        [SerializeField] GameObject upgradeable;
        [SerializeField] GameObject maxed;
        [SerializeField] GameObject guildLvl;
        [SerializeField] TMP_Text upgradeCost;
        [SerializeField] TMP_Text gLvlTxt;
        public void Set(int sId, byte sLvl, byte gLvl, uint myGP) {
            bool isUpgradable = sLvl < Storage.data.guild.maxLevel && gLvl >= sLvl + 1;
            bool isGLevel = sLvl < Storage.data.guild.maxLevel && gLvl < sLvl + 1;

            maxed.SetActive(sLvl >= Storage.data.guild.maxLevel);
            upgradeable.SetActive(isUpgradable);
            guildLvl.SetActive(isGLevel);

            lvlTxt.text = sLvl.ToString();

            ScriptableGuildSkill skill = ScriptableGuildSkill.dict[sId];
            currentPoints.text = "+" + skill.Get(sLvl);
            if(isUpgradable) {
                upgradeCost.text = skill.cost[sLvl].ToString();
                upgradeCost.color = myGP >= skill.cost[sLvl] ? Color.white : Color.red;
                nextPoints.text = "+" + skill.Get((byte)(sLvl + 1));
            }

            if(isGLevel) gLvlTxt.text = (sLvl + 1).ToString();
        }
    }
}