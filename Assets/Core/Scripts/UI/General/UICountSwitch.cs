using UnityEngine;
using System;
using TMPro;
namespace Game.UI
{
    public class UICountSwitch : MonoBehaviour {
        [SerializeField] TMP_InputField input;
        [SerializeField] GameObject prevButton;
        [SerializeField] GameObject nextButton;
        [SerializeField] GameObject minButton;
        [SerializeField] GameObject maxButton;
        [SerializeField] uint min = 1;
        [SerializeField] uint max = 1;
        [SerializeField] uint m_count = 1;
        public uint count => m_count;
        public int index = -1;
        public void OnIncrease()
        {
            if(m_count > 1)
            {
                m_count--;
            }
            UpdateInfo();
        }
        public void OnDecrease()
        {
            if(m_count < max)
            {
                m_count++;
            }
            UpdateInfo();
        }
        public void OnMin()
        {
            if(min > 0)
            {
                m_count = min;
            }
            UpdateInfo();
        }
        public void OnMax()
        {
            if(max > 0)
            {
                m_count = max;
            }
            UpdateInfo();
        }
        public void OnInputEditEnd(string textValue)
        {
            uint v = Convert.ToUInt32(textValue);
            if(v < 1)
            {
                m_count = min > 0 ? min : 1;
            } 
            else if(max > 0)
            {
                m_count = v > max ? max : v;
            } 
            else
            {
                m_count = v;
            }
        }
        public void Limits(uint min, uint max)
        {
            this.min = m_count = min;
            this.max = max;
            UpdateInfo();
        }
        public bool IsValid()
        {
            return m_count >= min && m_count <= max;
        }
        public void Show(int index = -1)
        {
            this.index = index;
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        void UpdateInfo()
        {
            input.text = m_count.ToString();
            prevButton.SetActive(m_count > min);
            minButton.SetActive(m_count > min);
            nextButton.SetActive(m_count < max);
            maxButton.SetActive(m_count < max);
        }
        void OnDisable()
        {
            index = -1;
            input.text = "1";
        }
    }
}