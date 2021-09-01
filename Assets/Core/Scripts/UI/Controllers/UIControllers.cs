using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Game.UI
{
    public class UIControllers : MonoBehaviour
    {
        [SerializeField] GameObject skillsObj;
        [SerializeField] GameObject shortcutsObj;
        [SerializeField] GameObject mountBtn;
        [SerializeField] GameObject flyBtn;
        [SerializeField] RTLTMPro.RTLTextMeshPro autoStatus;
        [SerializeField] KeyCode[] skillHotkeys;
        Player player => Player.localPlayer;
        bool shortcutsOpened;
        void Update() {
            if (Input.GetKeyDown(skillHotkeys[0]) && !UIUtils.AnyInputActive())
                    OnUseSkill(0);
            if (Input.GetKeyDown(skillHotkeys[1]) && !UIUtils.AnyInputActive())
                    OnUseSkill(1);
            if (Input.GetKeyDown(skillHotkeys[2]) && !UIUtils.AnyInputActive())
                    OnUseSkill(2);
            if (Input.GetKeyDown(skillHotkeys[3]) && !UIUtils.AnyInputActive())
                    OnUseSkill(3); 
            if (Input.GetKeyDown(skillHotkeys[4]) && !UIUtils.AnyInputActive())
                    OnUseSkill(4);
        }
        public void OnUseSkill(int skillIndex)
        {
            if(player != null && skillIndex > -1 && skillIndex < player.skillTemplates.Length)
            {
                player.UseSkill(skillIndex);
            }
            else
            {
                UINotifications.list.Add("Please select a skill", "برجاء اختيار مهارة");
            }
        }
        public void OnMount()
        {
            if(player.mount.canMount)
            {
                if(player.mount.mounted)
                {
                    player.CmdMountUnsummon();
                }
                else
                {
                    player.CmdMountSummon();
                }
            }
            else
            {
                UINotifications.list.Add("Select a mount first", "اختر راكب اولا");
            }
        }
        public void UpdateMountButton()
        {
            if(player)
            {
                mountBtn.SetActive(player.mount.canMount);
            }
        }
        public void UpdateFlyButton()
        {
            if(player)
            {
                flyBtn.SetActive(player.IsWinged());
            }
        }
        public void OnSwitch()
        {
            shortcutsOpened = !shortcutsOpened;
            if(shortcutsObj != null)
            {
                shortcutsObj.SetActive(shortcutsOpened);
            }
            if(skillsObj != null)
            {
                skillsObj.SetActive(!shortcutsOpened);
            }
        }
        public void UpdateAutoStatus()
        {
            if(player && autoStatus != null)
            {
                autoStatus.text = player.own.auto.on ? LanguageManger.Decide("On", "مفعل") : LanguageManger.Decide("Off", "غير مفعل");
            }
        }
        void OnEnable()
        {
            if(player != null)
            {
                if(player.skills.Count > 0)
                {
                    for(int i = 0; i < 5; i++)
                    {
                        UISkillSlot slot = skillsObj.transform.GetChild(i).GetComponent<UISkillSlot>();
                        if(slot != null)
                        {
                            slot.Assign(player.skills[i], i);
                            slot.onClick.SetListener((skillSlot) => OnUseSkill(skillSlot.index));
                            slot.onDoubleClick.SetListener((skillSlot) => UIManager.data.inScene.tooltip.Show(skillSlot.data));
                        }
                    }
                }
                UpdateMountButton();
            }
            else
            {
                UIManager.data.OnLocalPlayerNotFound();
            }
        }
    }
}