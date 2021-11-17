using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIChangeInBRNotifiy : MonoBehaviour
    {
        [SerializeField] float updateInterval = 2f;
        [SerializeField] Text brText;
        [SerializeField] Color increseColor;
        [SerializeField] Color decreseColor;
        [SerializeField] float hideInSec;
        uint oldBR;
        Player player => Player.localPlayer;
        void UpdateData()
        {
            if(player != null && player.battlepower != oldBR)
            {
                uint currentBR = 0;
                if(oldBR != 0)
                {
                    brText.gameObject.SetActive(true);
                    StopCoroutine(Hide());
                    StartCoroutine(Hide());
                    currentBR = player.stats.GetBattleRate().Result;
                    bool isIncrese = currentBR > oldBR;
                    brText.text = isIncrese ? $"BR +{currentBR - oldBR}" : $"BR -{oldBR - currentBR}";
                    brText.color = isIncrese ? increseColor : decreseColor;
                }
                oldBR = currentBR;
                UILocalPlayerInfo.singleton.UpdateBR();
            }
        }
        //public void Show() => gameObject.SetActive(true);
        void OnEnable()
        {
            if(player != null)
            {
                oldBR = player.stats.GetBattleRate().Result;
                //StartCo
                InvokeRepeating(nameof(UpdateData), updateInterval, updateInterval);
            }
        }
        IEnumerator<WaitForSeconds> Hide()
        {
            yield return new WaitForSeconds(hideInSec);
            brText.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            CancelInvoke(nameof(UpdateData));
        }
    }
}