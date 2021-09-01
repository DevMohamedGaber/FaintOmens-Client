using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UINpcDialogue : MonoBehaviour
{
    /*public static UINpcDialogue singleton;
    public GameObject panel;
    public Text welcomeText;
    public Transform questsContent;
    public Transform TeleportsContent;
    public Button teleportBtn;
    public Button buttonPrefab;
    public Button closeBtn;
    public bool hidden = true;
    public int page = -1;

    void Start() => singleton = this;

    void Update() {
        Player player = Player.localPlayer;

        // use collider point(s) to also work with big entities
        if (player != null && panel.activeSelf && player.target != null && player.target is Npc &&
            Utils.ClosestDistance(player, player.target) <= player.interactionRange) {
            hidden = false;
            Npc npc = (Npc)player.target;
            if(page == -1) {
                // welcome text
                welcomeText.text = npc.welcome;
                // teleport
                bool isTeleNPC = npc.teleports.Length > 0 && page != 0;
                teleportBtn.gameObject.SetActive(isTeleNPC);
                if(isTeleNPC) teleportBtn.onClick.SetListener(() => page = 0);

                List<ScriptableQuest> questsAvailable = npc.QuestsVisibleFor(player);
                if(questsAvailable.Count > 0) {
                    UIUtils.BalancePrefabs(buttonPrefab.gameObject, questsAvailable.Count, questsContent);
                    for(int i = 0; i < questsAvailable.Count; i++) {
                        Button btn = questsContent.GetChild(i).GetComponent<Button>();
                        btn.GetComponentInChildren<Text>().text = questsAvailable[i].name.ToString();
                        btn.onClick.SetListener(() => {
                            //Array.FindIndex(npc.quests, entry => entry.quest.name == questsAvailable[i].name);
                        });
                    }
                }
            }

            if (page == 0) {
                questsContent.gameObject.SetActive(false);
                welcomeText.text = $"Where do you want to Teleport ??";
                UIUtils.BalancePrefabs(buttonPrefab.gameObject, npc.teleports.Length, TeleportsContent);
                for(int i = 0; i < npc.teleports.Length; i++) {
                    Button btn = TeleportsContent.GetChild(i).GetComponent<Button>();
                    btn.GetComponentInChildren<Text>().text = Storage.data.cities[npc.teleports[i].city].name;
                    int iCopy = i;
                    btn.onClick.SetListener(() => {
                        if(player.own.gold >= npc.teleports[iCopy].cost)
                            player.CmdNpcTeleport(iCopy);
                    });
                }
            }

            closeBtn.onClick.SetListener(() => Hide());
        }
        else {
            if(!hidden) Hide();
        }
    }
    public void Show() => panel.SetActive(true);
    public void Hide() {
        panel.SetActive(false);
        hidden = true;
        welcomeText.text = "";
        page = -1;
        foreach(Transform child in questsContent) Destroy(child.gameObject);
        foreach(Transform child in TeleportsContent) Destroy(child.gameObject);
    }*/
}
