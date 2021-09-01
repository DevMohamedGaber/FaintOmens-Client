using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILoot : MonoBehaviour {
        [SerializeField] float updateInterval = .2f;
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        public static UILoot singleton;
        public Loot loot;
        Player player => Player.localPlayer;
        void UpdateData() {
            if(loot == null) return;
            if(Vector3.Distance(player.transform.position, loot.transform.position) > player.interactionRange)
                gameObject.SetActive(false);
        }
        public void Show(Loot loot) {
            this.loot = loot;
            gameObject.SetActive(true);
        }
        void OnEnable() {
            if(player) {
                InvokeRepeating("UpdateData", 0, updateInterval);
            }
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            loot = null;
            CancelInvoke("UpdateData");
        }
        void Awake() {
            if(singleton == null)
                singleton = this;
        }
    }
}