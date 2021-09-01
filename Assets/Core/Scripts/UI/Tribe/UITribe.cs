using System;
using UnityEngine;
using UnityEngine.UI;
public class UITribe : MonoBehaviour {
    /*public GameObject panel;
    public Text Name;
    public Text Troops;
    public Text BattlePower;
    public Text Wealth;
    public Button Donation;
    public Button TribesWar;
    public GameObject DonationPage;
    long GoldDonation = 0;
    int DiamondsDonation = 0;
    public InputField GoldDonateInput;
    public InputField DiamondsDonateInput;
    public Button DonateBtn;
    public GameObject TribesWarPage;

    void Update() {
        Player player = Player.localPlayer;
        if(player) {
            if(panel.activeSelf) {
                Name.text = $"{LanguageManger.GetWord(player.tribe.id, LanguageDictionaryCategories.Tribe)} Tribe Hall";
                Troops.text = player.tribe.Troops.ToString();
                BattlePower.text = player.tribe.TotalBR.ToString();
                Wealth.text = player.tribe.Wealth.ToString();
                if(DonationPage.activeSelf) DonationHandler(player);
            }
        }
        else panel.SetActive(false);
    }
    void DonationHandler(Player player) {
        GoldDonateInput.text = GoldDonation > 0 ? GoldDonation.ToString() : "";
        GoldDonateInput.onValueChanged.SetListener((value) => {
            long v = Convert.ToInt64(value);
            if(v < 0) GoldDonation = 0;
            else if(v >= player.own.gold) GoldDonation = player.own.gold;
            else GoldDonation = v;
        });

        DiamondsDonateInput.text = DiamondsDonation > 0 ? DiamondsDonation.ToString() : "";
        DiamondsDonateInput.onValueChanged.SetListener((value) => {
            int v = Convert.ToInt32(value);
            if(v < 0) DiamondsDonation = 0;
            else if(v >= player.own.diamonds) DiamondsDonation = player.own.diamonds;
            else DiamondsDonation = v;
        });
        DonateBtn.interactable = DiamondsDonation > 0 || GoldDonation > 0;
        DonateBtn.onClick.SetListener(() => {
            player.CmdDonateToTribe(GoldDonation, DiamondsDonation);
            GoldDonation = 0;
            DiamondsDonation = 0;
        });
    }
    void Awake() {
        Donation.onClick.SetListener(() => {
            DonationPage.SetActive(!DonationPage.activeSelf);
        });
    }*/
}