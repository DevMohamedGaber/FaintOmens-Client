using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] GameObject overlay;
        public int id;
        public int index = -1;
        public void Set(ScriptableSkill skill)
        {
            id = skill.name;
            if(icon != null)
            {
                icon.sprite = skill.image;
            }
            if(nameTxt != null)
            {
                nameTxt.text = skill.Name;
            }
        }
        public void Refresh()
        {
            if(id != 0)
            {
                if(index == -1)
                {
                    index = Player.localPlayer.skills.IndexOf(id);
                }
                if(index != 0)
                {
                    nameTxt.text = Player.localPlayer.skills[index].Name;
                }
                overlay.SetActive(index == -1);
            }
        }
        public void ResetData()
        {
            id = 0;
            index = -1;
            icon.sprite = null;
            nameTxt.text = string.Empty;
            overlay.SetActive(true);
        }
    }
}