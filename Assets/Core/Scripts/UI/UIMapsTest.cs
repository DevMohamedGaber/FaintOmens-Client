using UnityEngine;
using UnityEngine.UI;
using Mirror;
namespace Game.UI
{
    public class UIMapsTest : MonoBehaviour {
        public Transform Content;
        public Button prefab;
        private void Awake() {
            UIUtils.BalancePrefabs(prefab.gameObject, ScribtableCity.dict.Count, Content);
            int i = 0;
            foreach(ScribtableCity city in ScribtableCity.dict.Values) {
                Button btn = Content.GetChild(i).GetComponent<Button>();
                btn.transform.GetComponentInChildren<Text>().text = city.prefab.name;
                btn.onClick.SetListener(() => {
                    Player.localPlayer.CmdTeleportTo((byte)city.name, Vector3.zero);
                    gameObject.SetActive(false);
                });
                i++;
            }
        }
        public void Show() => gameObject.SetActive(true);
    }
}