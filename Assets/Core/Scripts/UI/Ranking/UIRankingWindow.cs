using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIRankingWindow : MonoBehaviour {
        [SerializeField] Button PlayerBR;
        [SerializeField] Button PlayerLvl;
        [SerializeField] Button PlayerHnr;
        [SerializeField] Button TribeBR;
        [SerializeField] Button TribeWins;
        [SerializeField] Button GuildBR;
        [SerializeField] Button GuildLvl;
        [SerializeField] Button PetBR;
        [SerializeField] Button PetLvl;
        [SerializeField] Button MountBR;
        [SerializeField] Button MountLvl;
        [SerializeField] Transform Content;
        [SerializeField] GameObject NoDataFound;
        [SerializeField] Text[] TableHeads;
        [SerializeField] UIRankingBasicRow basicRowPrefab;
        [SerializeField] UIRankingSummonableRow SummonableRowPrefab;
        [SerializeField] Button CloseWindowButton;
        Player player => Player.localPlayer;
        [SerializeField] RankingCategory current;

        public void ShowBasicList(RankingBasicData[] list) {
            SetTableHead();
            UIUtils.BalancePrefabs(basicRowPrefab.gameObject, list.Length, Content);
            if(list.Length > 0) {
                for (int i = 0; i < list.Length; i++) {
                    UIRankingBasicRow row = Content.GetChild(i).GetComponent<UIRankingBasicRow>();
                    row.rank.text = (i+1).ToString();
                    row.value.text = list[i].value.ToString();
                    if(current == RankingCategory.TribeBR || current == RankingCategory.TribeWins)
                        row.Name.text = LanguageManger.GetWord(list[i].id, LanguageDictionaryCategories.Tribe);
                    else
                        row.Name.text = list[i].name;
                }
                NoDataFound.SetActive(false);
                Content.gameObject.SetActive(true);
            } else {
                Content.gameObject.SetActive(false);
                NoDataFound.SetActive(true);
            }
        }
        public void ShowSummonableList(SummonableRankingData[] list) {
            SetTableHead();
            UIUtils.BalancePrefabs(SummonableRowPrefab.gameObject, list.Length, Content);
            if(list.Length > 0) {
                for (int i = 0; i < list.Length; i++) {
                    bool isPet = current == RankingCategory.PetLvl || current == RankingCategory.PetBR;
                    if(isPet) Content.GetChild(i).GetComponent<UIRankingSummonableRow>().SetPet(list[i], i+1);
                    else Content.GetChild(i).GetComponent<UIRankingSummonableRow>().SetMount(list[i], i+1);
                }
                NoDataFound.SetActive(false);
                Content.gameObject.SetActive(true);
            } else {
                Content.gameObject.SetActive(false);
                NoDataFound.SetActive(true);
            }
        }
        void SetTableHead() {
            if(current == RankingCategory.PlayerBR || current == RankingCategory.GuildBR || current == RankingCategory.TribeBR) {
                TableHeads[0].text = "Name";
                TableHeads[1].text = "BR";
                TableHeads[2].gameObject.SetActive(false);
            }
            else if(current == RankingCategory.PlayerHonor) {
                TableHeads[0].text = "Name";
                TableHeads[1].text = "Honor";
                TableHeads[2].gameObject.SetActive(false);
            }
            else if(current == RankingCategory.PlayerLevel || current == RankingCategory.GuildLevel) {
                TableHeads[0].text = "Name";
                TableHeads[1].text = "Level";
                TableHeads[2].gameObject.SetActive(false);
            }
            else if(current == RankingCategory.TribeWins) {
                TableHeads[0].text = "Name";
                TableHeads[1].text = "Wins";
                TableHeads[2].gameObject.SetActive(false);
            }
            else if(current == RankingCategory.PetBR || current == RankingCategory.MountBR) {
                TableHeads[0].text = "Owner";
                TableHeads[1].text = "Type";
                TableHeads[2].text = "BR";
                TableHeads[2].gameObject.SetActive(true);
            }
            else if(current == RankingCategory.PetLvl || current == RankingCategory.MountLvl) {
                TableHeads[0].text = "Owner";
                TableHeads[1].text = "Type";
                TableHeads[2].text = "Level";
                TableHeads[2].gameObject.SetActive(true);
            }
        }
        void OnSelectCategory(RankingCategory category, bool forceSend = false) {
            if(current != category || forceSend) {
                current = category;
                player.CmdGetRankingData(category);
            }
        }
        public void Show() => gameObject.SetActive(true);
        void OnEnable() {
            OnSelectCategory((RankingCategory)0, true);// load what's first in case of change in enum order

            PlayerBR.onClick.AddListener(() => OnSelectCategory(RankingCategory.PlayerBR));
            PlayerLvl.onClick.AddListener(() => OnSelectCategory(RankingCategory.PlayerLevel));
            PlayerHnr.onClick.AddListener(() => OnSelectCategory(RankingCategory.PlayerHonor));
            TribeBR.onClick.AddListener(() => OnSelectCategory(RankingCategory.TribeBR));
            TribeWins.onClick.AddListener(() => OnSelectCategory(RankingCategory.TribeWins));
            GuildBR.onClick.AddListener(() => OnSelectCategory(RankingCategory.GuildBR));
            GuildLvl.onClick.AddListener(() => OnSelectCategory(RankingCategory.GuildLevel));
            PetBR.onClick.AddListener(() => OnSelectCategory(RankingCategory.PetBR));
            PetLvl.onClick.AddListener(() => OnSelectCategory(RankingCategory.PetLvl));
            MountBR.onClick.AddListener(() => OnSelectCategory(RankingCategory.MountBR));
            MountLvl.onClick.AddListener(() => OnSelectCategory(RankingCategory.MountLvl));
            
            CloseWindowButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
        void OnDisable() {
            PlayerBR.onClick.RemoveAllListeners();
            PlayerLvl.onClick.RemoveAllListeners();
            PlayerHnr.onClick.RemoveAllListeners();
            TribeBR.onClick.RemoveAllListeners();
            TribeWins.onClick.RemoveAllListeners();
            GuildBR.onClick.RemoveAllListeners();
            GuildLvl.onClick.RemoveAllListeners();
            PetBR.onClick.RemoveAllListeners();
            PetLvl.onClick.RemoveAllListeners();
            MountBR.onClick.RemoveAllListeners();
            MountLvl.onClick.RemoveAllListeners();
            
            CloseWindowButton.onClick.RemoveAllListeners();
        }
    }
}