using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIEvents : MonoBehaviour {
        [SerializeField] float updateInterval = 1f;
        [SerializeField] GameObject listObj;
        [SerializeField] GameObject[] list; // only Dynamic items
        [SerializeField] UIFlippable button;
        Player player => Player.localPlayer;
        void UpdateData() {
            list[0].SetActive(player.classInfo.data.CanShowPromote(player.level));
        }
        public void OnShowHide() {
            if(listObj.activeSelf) {
                listObj.SetActive(false);
                //button.horizontal = true;
                button.vertical = true;
            } else {
                listObj.SetActive(true);
                //button.horizontal = false;
                button.vertical = false;
            }
        }
        void OnEnable() {
            if(player != null) {
                InvokeRepeating("UpdateData", updateInterval, updateInterval);
                // 1st 7days
                bool in7Days = System.DateTime.FromOADate(player.own.createdAt).AddDays(7).Day >= Server.time.Day;
                list[1].SetActive(in7Days);
                //list[2].SetActive(in7Days);
            }
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            CancelInvoke("UpdateData");
        }
    }
    /* - Dynamic Items List -
        [0] => character promotion
        [1] => 1st 7Days signup
        [2] => 1st 7Days recharge
    */
}