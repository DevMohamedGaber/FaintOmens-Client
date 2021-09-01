using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public partial class UIWorldBoss : MonoBehaviour {
    /*public UIEventsInvite invite;
    public Sprite EventImage;
    public bool showedMsg = false;
    public GameObject RankList;
    public UIWorldBossRank rankPrefab;
    public Transform RankContent;
    public GameObject noPlayerInRank;
    public Button BackBtn;*/

    /*void Update() {
        Player player = Player.localPlayer;
        if(player) {
            ScheduledEventsHandler events = ScheduledEventsHandler.singleton;
            if((events.ReadyForWorldBoss || events.WorldBossEnabled) && !showedMsg) ShowInviteMsg(player);
            if(player.InWorldBossArea) {
                RanksHandler(events);

                BackBtn.gameObject.SetActive(true);
                BackBtn.onClick.SetListener(() => {
                    player.CmdTeleportToLastLocation();
                    player.InWorldBossArea = false;
                });
            }
            else {
                RankList.SetActive(false);
                BackBtn.gameObject.SetActive(false);
            }
        }
    }*/
    /*protected void RanksHandler(ScheduledEventsHandler events) {
        RankList.SetActive(true);
        if(events.WorldBossRankList.Count > 0) {
            noPlayerInRank.SetActive(false);
            List<WorldBossRank> list = events.WorldBossRankList.OrderBy(p => p.damage).ToList();
            UIUtils.BalancePrefabs(rankPrefab.gameObject, list.Count, RankContent);
            for(int i = 0; i < list.Count; i++) {
                Color rankColor = RankColor(i);
                UIWorldBossRank row = RankContent.GetChild(i).GetComponent<UIWorldBossRank>();
                WorldBossRank rank = list[i];
                row.SetColor(RankColor(i));
                row.name.text = rank.Name;
                row.rank.text = $"#{i + 1}";
                row.damage.text = Utils.MinifyLong(rank.damage);
            }
        } else {
            noPlayerInRank.SetActive(true);
        }
    }*/
    /*protected Color RankColor(int rank) {
        if(rank == 0) return Color.red;
        if(rank == 1) return Color.green;
        if(rank == 2) return Color.blue;
        return Color.white;
    }
    protected void ShowInviteMsg(Player player) {
        showedMsg = true;
        invite.image.sprite = EventImage;
        invite.Show("World Boss", "Join World Boss Now", 
        () => {
            player.CmdWorldBossTeleport();
            invite.close();
        });
    }*/
}