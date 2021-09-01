using System.Collections.Generic;
using UnityEngine;
namespace Game.UI
{
    public class UIEventCounter : MonoBehaviour {
        [SerializeField] RTLTMPro.RTLTextMeshPro msgTxt;
        [SerializeField] TMPro.TMP_Text counterTxt;
        public bool isWorking = false;
        WaitForSeconds updateInterval = new WaitForSeconds(1);
        int s, m, current = 0;
        public void StartCounter(string enMsg, string arMsg) {
            if(isWorking)
                StopCoroutine(Counter());
            msgTxt.text = LanguageManger.Decide(enMsg, arMsg);
            counterTxt.text = "00:00";
            gameObject.SetActive(true);
            StartCoroutine(Counter());
            isWorking = true;
        }
        public void StopCounter() {
            if(isWorking) {
                gameObject.SetActive(false);
                current = 0;
                StopCoroutine(Counter());
                isWorking = false;
            }
        }
        IEnumerator<WaitForSeconds> Counter() {
            yield return updateInterval;

            current++;

            m = (int)Mathf.Floor(current / 60);
            s = current % 60;

            counterTxt.text = $"{(m < 10 ? "0"+m.ToString() : m.ToString())}:{(s < 10 ? "0"+s.ToString() : s.ToString())}";
        }
    }
}