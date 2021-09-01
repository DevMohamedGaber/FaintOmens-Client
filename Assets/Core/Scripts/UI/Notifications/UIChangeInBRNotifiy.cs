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
        void UpdateData() {
            if(player != null && player.battlepower != oldBR)
            {
                if(oldBR != 0)
                {
                    brText.gameObject.SetActive(true);
                    StopCoroutine(Hide());
                    StartCoroutine(Hide());
                    bool isIncrese = player.battlepower > oldBR;
                    brText.text = isIncrese ? $"BR +{player.battlepower - oldBR}" : $"BR -{oldBR - player.battlepower}";
                    brText.color = isIncrese ? increseColor : decreseColor;
                }
                oldBR = player.battlepower;
                UILocalPlayerInfo.singleton.UpdateBR();
            }
        }
        //public void Show() => gameObject.SetActive(true);
        void OnEnable()
        {
            if(player != null)
            {
                oldBR = player.battlepower;
                //StartCo
                InvokeRepeating(nameof(UpdateData), updateInterval, updateInterval);
            }
        }
        IEnumerator<WaitForSeconds> Hide() {
            yield return new WaitForSeconds(hideInSec);
            brText.gameObject.SetActive(false);
        }
        private void OnDisable() {
            CancelInvoke(nameof(UpdateData));
        }
    }
}