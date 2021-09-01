using UnityEngine;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIToggleGroup : MonoBehaviour
    {
        [SerializeField] int defaultIndex = -1;
        [SerializeField] List<UIToggle> m_list = new List<UIToggle>();
        [SerializeField] bool updateOnAwake = true;
        public int currentIndex = -1;
        public List<UIToggle> list => m_list;
        public UIToggle current => currentIndex > -1 ? m_list[currentIndex] : null;
        public void UpdateTogglesList()
        {
            Clear();
            if(transform.childCount > 0)
            {
                //Debug.Log(m_list.Count);
                for (int i = 0; i < transform.childCount; i++)
                {
                    UIToggle child = transform.GetChild(i).GetComponent<UIToggle>();
                    if(child != null)
                    {
                        //Debug.Log(child);
                        m_list.Add(child);
                        child.index = i;
                    }
                }
                //Debug.Log(m_list.Count);
                if(m_list.Count > 0 && defaultIndex > -1)
                {
                    Debug.Log("Selected");
                    Select(defaultIndex);
                }
            }
        }
        public void Clear()
        {
            if(m_list.Count > 0)
            {
                m_list.RemoveRange(0, m_list.Count);
            }
        }
        public void Select(int index)
        {
            if(index > -1 && index < m_list.Count && m_list.Count > 0)
            {
                for(int i = 0; i < m_list.Count; i++)
                {
                    m_list[i].SetOn(i == index);
                }
                currentIndex = index;
            }
        }
        public void ResetAll()
        {
            if(m_list.Count > 0)
            {
                for(int i = 0; i < m_list.Count; i++)
                {
                    m_list[i].SetOn(i == defaultIndex);
                }
                currentIndex = defaultIndex;
            }
        }
        private void Awake()
        {
            if(updateOnAwake)
            {
                UpdateTogglesList();
            }
        }
    }
}