using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
namespace Game.UI
{
    public class UISkillSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image icon;
        [SerializeField] GameObject lockedObj;
        [SerializeField] GameObject cooldownObj;
        [SerializeField] Image cooldownOverlay;
        [SerializeField] TMP_Text cooldownTxt;
        public OnClickEvent onClick = new OnClickEvent();
        public OnDoubleClickEvent onDoubleClick = new OnDoubleClickEvent();
        public Skill data;
        public int index = -1;
        public bool IsAssigned() => data.id > 0;
        public bool isLocked => lockedObj.activeSelf;
        public float cooldown => data.CooldownRemaining();
        void Update() {
            if(cooldownObj != null)
            {
                bool hasCooldown = cooldown > 0;
                cooldownObj.SetActive(hasCooldown);
                if(hasCooldown)
                {
                    if(cooldownTxt)
                    {
                        cooldownTxt.text = cooldown.ToString("F0") + LanguageManger.Decide("s", "ث");
                    }
                    if(cooldownOverlay != null)
                    {
                        cooldownOverlay.fillAmount = cooldown / data.cooldown;
                    }
                }
            }
        }
        public virtual void Assign(Skill skill, int index = -1)
        {
            data = skill;
            this.index = index;
            if(icon)
            {
                icon.sprite = data.image;
            }
        }
        public virtual void Unassign()
        {
            if(icon)
            {
                icon.sprite = null;
            }
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.clickCount == 1 && onClick != null)
            {
                onClick.Invoke(this);
            }
            else if(eventData.clickCount == 2 && onDoubleClick != null)
            {
                eventData.clickCount = 0;
                onDoubleClick.Invoke(this);
            }
        }
        [System.Serializable] public class OnClickEvent : UnityEvent<UISkillSlot> {}
        [System.Serializable] public class OnDoubleClickEvent : UnityEvent<UISkillSlot> { }
    }
}