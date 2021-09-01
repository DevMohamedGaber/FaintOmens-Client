using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Game.UI
{
    public class UIToggle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] UIToggleGroup group;
        [SerializeField] bool isOn;
        [SerializeField] GameObject selectedObj;
        [SerializeField] UnityEvent action;
        public int index = -1;
        public UnityAction onSelect
        {
            set
            {
                if(value == null)
                {
                    action.RemoveAllListeners();
                }
                else
                {
                    action.SetListener(value);
                }
            }
        }
        public void Select()
        {
            if(group != null)
            {
                group.Select(index);
            }
            if(action != null)
            {
                action.Invoke();
            }
        }
        public void SetOn(bool isOn)
        {
            this.isOn = isOn;
            if(selectedObj != null)
                selectedObj.SetActive(isOn);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Select();
        }
        public void ForceOnSelect()
        {
            if(action != null)
            {
                action.Invoke();
            }
        }
        void Awake() {
            if(group == null)
            {
                UIToggleGroup groupObj = transform.parent.GetComponent<UIToggleGroup>();
                if(groupObj != null)
                {
                    group = groupObj;
                }
            }
        }
    }
}