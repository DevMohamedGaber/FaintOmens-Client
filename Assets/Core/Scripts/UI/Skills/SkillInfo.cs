using UnityEngine;
using TMPro;
using RTLTMPro;
namespace Game.UI
{
    [System.Serializable]
    public struct SkillInfo
    {
        [Header("Current")]
        [SerializeField] GameObject defaultObj;
        [SerializeField] UnityEngine.UI.Image icon;
        [SerializeField] RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text cooldownTxt;
        [SerializeField] TMP_Text mpTxt;
        [SerializeField] RTLTextMeshPro descTxt;
        [SerializeField] UIProgressBar progressBar;
        [SerializeField] TMP_Text progressTxt;
        [Header("Next")]
        [SerializeField] GameObject nextLevelInfoObj;
        [SerializeField] GameObject maxLevelObj;
        [SerializeField] TMP_Text nextMPTxt;
        [SerializeField] TMP_Text nextCDTxt;
        [SerializeField] RTLTextMeshPro nextDescTxt;

        public void Set(ScriptableSkill skillData, int index)
        {
            defaultObj.SetActive(skillData.learnDefault);
            icon.sprite = skillData.image;
            if(index != -1)
            {
                Skill skill = Player.localPlayer.skills[index];
                
                nameTxt.text = $"{skill.Name} / {skillData.maxLevel}";
                cooldownTxt.text = skill.cooldown.ToString("F0");
                mpTxt.text = skill.manaCosts.ToString();

                bool isMax = (int)skill.level == skillData.maxLevel;
                progressBar.gameObject.SetActive(!isMax);
                nextLevelInfoObj.SetActive(!isMax);
                maxLevelObj.SetActive(isMax);

                if(!isMax)
                {
                    progressBar.fillAmount = (float)(skill.experience / skill.expMax) * 100f;
                    progressTxt.text = $"{skill.experience} / {skill.expMax}";
                    int nextLevel = (int)skill.level + 1;
                    nextDescTxt.text = skillData.GetDescription(nextLevel);
                    nextMPTxt.text = skillData.manaCosts.Get(nextLevel).ToString();
                    nextCDTxt.text = skillData.cooldown.Get(nextLevel).ToString();
                }
            }
            else
            {
                nameTxt.text = $"{skillData.Name} {LanguageManger.Decide("Lvl.", "مستوي")} / {skillData.maxLevel}";
                cooldownTxt.text = skillData.cooldown.Get(1).ToString("F0");
                mpTxt.text = skillData.manaCosts.Get(1).ToString();
                progressBar.gameObject.SetActive(false);

                bool isMax = skillData.maxLevel == 1;
                nextLevelInfoObj.SetActive(!isMax);
                maxLevelObj.SetActive(isMax);

                if(!isMax)
                {
                    nextMPTxt.text = skillData.manaCosts.Get(2).ToString();
                    nextCDTxt.text = skillData.cooldown.Get(2).ToString();
                    nextDescTxt.text = skillData.GetDescription(2);
                }
            }
        }
    }
}