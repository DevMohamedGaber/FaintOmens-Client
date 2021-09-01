using UnityEngine;
using UnityEngine.UI;
/* TODO
** handle at max level
** add claim rewards
** preview claimable rewards
** define the rest of vip benfits (when it's finished)
*/
namespace Game.UI
{
    public partial class UIVIP : MonoBehaviour {
        /*public GameObject panel;
        // progress
        public Text CurrentLvlText;
        public Text NextLvlText;
        public Slider PointsBar;
        public Text currentPoints;
        public Button Parchase;
        public Button Test; // TODO: clearly must be removed in production

        // navigate
        public Transform VIPButtonsContent;
        public int currentVip = 0;
        public bool selecting = false;

        //Benfits
        public GameObject BonusHonor;
        public GameObject BonusQuests;

        void Update() {
            if(panel.activeSelf) {
                Player player = Player.localPlayer;
                if (player) {
                    VIP vip = player.VIP;
                    if(!selecting) currentVip = vip.level;
                    for(int i = 0; i < Player.localPlayer.VIP_List.Count; i++) {
                        Button button = VIPButtonsContent.GetChild(i).GetComponent<Button>();
                        int iCopy = i;
                        button.onClick.SetListener(() => {
                            currentVip = iCopy;
                            selecting = true;
                        });
                    }
                    Progress(player);
                    Benfits(player.VIP_List[currentVip]);
                }
            }
        }
        protected void Progress(Player player) {
            VIP vip = player.VIP;
            int maxPoints = player.VIP_List[vip.level].nextLevelpoints;
            CurrentLvlText.text = vip.level.ToString();
            NextLvlText.text = (vip.level + 1).ToString();
            PointsBar.maxValue = maxPoints;
            PointsBar.value = vip.points;
            currentPoints.text = $"{vip.points} / {maxPoints}";
            Parchase.onClick.SetListener(() => {
                panel.SetActive(false); // TODO: go to Parchase page (when it's done)
            });
            //Test.onClick.SetListener(() => player.AddVIPPoints(new System.Random().Next(1,100))); // must be removed in Production
        }
        protected void Benfits(VIP_Info vip) {
            BenfitHandler(vip.bonusHonor, BonusHonor);// honor
            BenfitHandler(vip.questsQuota, BonusQuests); // vip quests
        }
        protected void BenfitHandler(int bonus, GameObject obj, string prefix = "+") {
            if(bonus > 0) {
                obj.SetActive(true);
                obj.transform.GetChild(0).GetComponent<Text>().text = $"{prefix}{bonus}";
            }
            else obj.SetActive(false);
        }*/
    }
}