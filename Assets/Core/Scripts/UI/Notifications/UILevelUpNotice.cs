using UnityEngine;
using System.Collections.Generic;
namespace Game.UI
{
    public class UILevelUpNotice : MonoBehaviour {
        [SerializeField] TMPro.TMP_Text level;
        public void Show() {
            level.text = Player.localPlayer.level.ToString();
            gameObject.SetActive(true);
            StartCoroutine(ShowMsg());
        }
        IEnumerator<WaitForSeconds> ShowMsg() {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }
    }
}