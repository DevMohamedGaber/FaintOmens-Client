using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RTLTMPro;
namespace Game.UI
{
    public class Skills : WindowWithBasicCurrencies
    {
        [Header("List")]
        [SerializeField] GameObject skillBtnPrefab;
        [SerializeField] Transform activeContent;
        [SerializeField] Transform passiveContent;
        [SerializeField] UIToggleGroup group;
        [SerializeField] SkillInfo info;
        ScriptableSkill[] skills => player.skillTemplates;
        SkillButton[] skillButtons;
        void OnSelect(int index)
        {
            if(index > 0 && index < skillButtons.Length && skillButtons[index].id > 0 &&
                ScriptableSkill.dict.TryGetValue(skillButtons[index].id, out ScriptableSkill skillData))
            {
                info.Set(skillData, skillButtons[index].index);
            }
        }
        void RefreshButtons()
        {
            if(skillButtons.Length > 0)
            {
                for (int i = 0; i < skillButtons.Length; i++)
                {
                    skillButtons[i].Refresh();
                }
            }
        }
        void AddSkill(int index, Transform content)
        {
            if(index > 0 && index < skills.Length && content != null)
            {
                GameObject obj = Instantiate(skillBtnPrefab, content, false);
                SkillButton btn = obj.GetComponent<SkillButton>();
                if(btn != null)
                {
                    btn.Set(skills[index]);
                    skillButtons[index] = btn;
                    UIToggle toggle = obj.GetComponent<UIToggle>();
                    if(toggle != null)
                    {
                        toggle.onSelect = () => OnSelect(index);
                    }
                }
            }
        }
        void Awake()
        {
            skillButtons = new SkillButton[skills.Length];
            if(skills.Length > 0)
            {
                for (int i = 0; i < skills.Length; i++)
                {
                    if(skills[i] is PassiveSkill)
                    {
                        AddSkill(i, passiveContent);
                    }
                    else
                    {
                        AddSkill(i, activeContent);
                    }
                }
                group.UpdateTogglesList();
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            RefreshButtons();
        }
    }
}