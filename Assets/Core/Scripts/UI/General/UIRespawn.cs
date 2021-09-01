using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIRespawn : MonoBehaviour
    {
        [SerializeField] TMP_Text coolDownTxt;
        [SerializeField] GameObject canReviveObj;
        [SerializeField] GameObject freeSpawnHereObj;
        [SerializeField] TMP_Text freeSpawnHereTxt;
        Player player => Player.localPlayer;
        int timeLeft;
        public void Show(byte AvailableFreeRespawn)
        {
            if(player != null && player.health == 0)
            {
                // counter
                timeLeft = Storage.data.player.respawnTime;
                InvokeRepeating("CountDown", 0, 1);
                // free spawn Here
                freeSpawnHereObj.SetActive(AvailableFreeRespawn > 0);
                if(AvailableFreeRespawn > 0) {
                    freeSpawnHereTxt.text = AvailableFreeRespawn.ToString();
                }
                // show object
                canReviveObj.SetActive(false);
                coolDownTxt.gameObject.SetActive(true);
                gameObject.SetActive(true);
            }
            else
            {
                Hide();
            }
        }
        void CountDown()
        {
            if(timeLeft > 0)
            {
                timeLeft--;
                coolDownTxt.text = $"({timeLeft.ToString()})";
            }
            else
            {
                CancelInvoke("CountDown");
                coolDownTxt.gameObject.SetActive(false);
                canReviveObj.SetActive(true);
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void OnRespawn(int type)
        {
            if(player.health == 0 && type > 0 && type <= 2)
            {
                player.CmdRespawn((byte)type);
            }
        }
    }
}