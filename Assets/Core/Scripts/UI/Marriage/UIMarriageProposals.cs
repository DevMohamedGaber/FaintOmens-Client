using UnityEngine;
namespace Game.UI
{
    public class UIMarriageProposals : MonoBehaviour {
        [SerializeField] float updateInterval = .5f;
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        [SerializeField] GameObject noProps;
        Player player => Player.localPlayer;
        void UpdateData() {
            UIUtils.BalancePrefabs(prefab, player.own.marriageProposals.Count, content);
            if(player.own.marriageProposals.Count > 0) {
                for(int i = 0; i < player.own.marriageProposals.Count; i++)
                    content.GetChild(i).GetComponent<UIMarriageProposal>().Set(i);
            }
            noProps.SetActive(player.own.marriageProposals.Count < 1);
        }
        void OnEnable() {
            if(player && !player.IsMarried()) {
                InvokeRepeating("UpdateData", 0, updateInterval);
            }
            else this.gameObject.SetActive(false);
        }
        void OnDisable() => CancelInvoke("UpdateData");
        //void Update() => Debug.Log(System.DateTime.Now); // test
    }
}