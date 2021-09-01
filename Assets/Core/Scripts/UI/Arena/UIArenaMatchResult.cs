using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class UIArenaMatchResult : MonoBehaviour
    {
        [SerializeField] GameObject winObj;
        [SerializeField] GameObject lossObj;
        [SerializeField] TMP_Text myDmgTxt;
        [SerializeField] TMP_Text opponentDmgTxt;
        [SerializeField] TMP_Text arenaPointsTxt;
        Player player => Player.localPlayer;
        public void Show1v1(bool win, int dmg, int opponentDmg)
        {
            winObj.SetActive(win);
            lossObj.SetActive(!win);

            myDmgTxt.text = dmg.ToString();
            opponentDmgTxt.text = opponentDmg.ToString();
            arenaPointsTxt.text = $"{player.own.arena1v1Points} <color={(win ? "green" : "red")}>({(win ? "+"+Storage.data.arena.pointsOnWin : "-"+Storage.data.arena.pointsOnLoss)})</color>";
            
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void OnLeave()
        {
            if(player.own.occupation == PlayerOccupation.InMatchArena1v1)
                player.CmdLeaveArena1v1();
        }
    }
}