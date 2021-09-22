using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIClassPromotion : MonoBehaviour
    {
        [SerializeField] float updateInterval = 3f;
        [SerializeField] UILanguageDefiner lang;
        [SerializeField] Image progressBarFill;
        Player player => Player.localPlayer;
        PlayerClassData classData => player.classInfo;
        void UpdateData() {
            lang.SetSuffix(1, $": <color={(player.level >= classData.sub.reqLevel ? "white" : "red")}>{classData.sub.reqLevel}</color>");
            lang.SetSuffix(2, $": <color={(player.battlepower >= classData.sub.reqBR ? "white" : "red")}>{classData.sub.reqBR}</color>");
            lang.SetSuffix(3, $": <color={(player.own.militaryRank >= classData.sub.reqMilitaryRank ? "white" : "red")}>{classData.sub.reqMilitaryRank}</color>");

            lang.RefreshRange(1, 3);
        }
        void OnEnable() {
            if(player != null && player.level >= classData.sub.reqLevel) {
                InvokeRepeating("UpdateData", 0, updateInterval);
            } else {
                gameObject.SetActive(false);
                Notifications.list.Add("Already reached max promotion", "لقد وصلت لاعلي ترقية");
            }
        }
        void OnDisable() {
            CancelInvoke(nameof(UpdateData));
        }
        /*public GameObject panel;
        public float updateRate;
        public bool canPromote = false;
        public Button WindowBtn;
        public Text ReqLvl;
        public Text ReqBR;
        public Text ReqMR;
        public Text ReqItem;
        public Text NextUpgradeName;
        public Image progressBar;
        public Text progressText;
        public Button StartQuestBtn;
        public Button PromoteBtn;
        SubClass nextSubClass;

        public Text HPBonusText;
        public Text AtkBonusText;
        public Text DefBonusText;
        void Functionality() {
            Player player = Player.localPlayer;
            if(player) {
                if(player.subClass < Storage.data.classes[(int)player.classId].subClasses.Length - 1) {
                    nextSubClass = Storage.data.classes[(int)player.classId].subClasses[player.subClass + 1];
                    WindowBtn.gameObject.SetActive(player.level >= nextSubClass.level);
                }
                else {
                    panel.SetActive(false);
                    WindowBtn.gameObject.SetActive(false);
                }
                if(panel.activeSelf) {
                    ReqLvl.text = $"<color={(player.level >= nextSubClass.level ? "green" : "red")}>{nextSubClass.level}</color>";
                    ReqBR.text = $"<color={(player.battlepower >= nextSubClass.br ? "green" : "red")}>{nextSubClass.br}</color>";
                    ReqMR.text = $"<color={(player.own.militaryRank >= nextSubClass.militaryRank ? "green" : "red")}>{Storage.data.militaryRanks[nextSubClass.militaryRank].name}</color>";
                    int[] itemsCount = new int[]{};
                    Array.Resize(ref itemsCount, nextSubClass.UpgradeItems.Length);
                    string itemsInfo = "";
                    for(int i = 0; i < nextSubClass.UpgradeItems.Length; i++) {
                        itemsCount[i] = player.InventoryCountById(nextSubClass.UpgradeItems[i].item.name);
                        itemsInfo += $"<color={(itemsCount[i] >= nextSubClass.UpgradeItems[i].amount ? "green" : "red")}>[{nextSubClass.UpgradeItems[i].item.name.ToString()}]({itemsCount[i]}/{nextSubClass.UpgradeItems[i].amount})</color>";
                        itemsInfo += i != nextSubClass.UpgradeItems.Length - 1 ? "," : "";
                    }
                    ReqItem.text = itemsInfo;
                    NextUpgradeName.text = $"<color=green>{Storage.data.classes[(int)player.classId].subClasses[player.subClass].name}</color> => <color=red>{nextSubClass.name}</color>";
                    
                    float perc = .25f;//level
                    perc += player.battlepower >= nextSubClass.br ? .25f : ((float)player.battlepower / (float)nextSubClass.br) * .25f; // br
                    perc += player.own.militaryRank >= nextSubClass.militaryRank ? .25f : ((float)player.own.militaryRank / (float)nextSubClass.militaryRank) * .25f; // militaryRank
                    float percPerItem = .25f/itemsCount.Length;
                    for(int i = 0; i < itemsCount.Length; i++) {
                        perc += itemsCount[i] >= nextSubClass.UpgradeItems[i].amount ? percPerItem : ((float)itemsCount[i] / (float)nextSubClass.UpgradeItems[i].amount) * percPerItem;
                    }

                    progressBar.fillAmount = perc;
                    progressText.text = $"{(int)(perc * 100)}%";
                    bool hasQuest = player.HasActiveQuest(nextSubClass.startQuest.name);
                    StartQuestBtn.gameObject.SetActive(!hasQuest);
                    StartQuestBtn.onClick.SetListener(() => player.CmdStartPromotionQuest());
                    
                    PromoteBtn.gameObject.SetActive(hasQuest);
                    PromoteBtn.interactable = hasQuest && perc == 1;
                    PromoteBtn.onClick.SetListener(() => {
                        player.CmdPromoteClass();
                        panel.SetActive(false);
                        canPromote = false;
                    });

                    SetBonusText("HP", HPBonusText, nextSubClass.bonusHP);
                    SetBonusText(player.classType == PlayerType.Physical ? "PAtk" : "MAtk", AtkBonusText, nextSubClass.bonusAtk);
                    SetBonusText(player.classType == PlayerType.Physical ? "PDef" : "MDef", DefBonusText, nextSubClass.bonusDef);
                }
            }
            else panel.SetActive(false);
        }
        void SetBonusText(string prefix, Text field, int value) {
            field.gameObject.SetActive(value > 0);
            if(value > 0) {
                field.text = $"{prefix}: <color=green>+{value}</color>";
            }
        }
        IEnumerator<WaitForSeconds> Update() {
            while(true) {
                Functionality();
                yield return new WaitForSeconds(updateRate);
            }
        }
        void Start() => StartCoroutine(Update());*/
    }
}