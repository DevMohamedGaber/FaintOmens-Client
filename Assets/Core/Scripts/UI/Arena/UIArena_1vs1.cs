using UnityEngine;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIArena_1vs1 : UIArena_Page
    {
        [SerializeField] TMP_Text currentPoints;
        [SerializeField] TMP_Text todayWins;
        [SerializeField] TMP_Text todayLosses;
        [SerializeField] TMP_Text TotalWins;
        [SerializeField] TMP_Text TotalLosses;
        [SerializeField] GameObject registerBtn;
        [SerializeField] GameObject unregisterBtn;
        PlayerOccupation occ => player.own.occupation;
        public override void Refresh()
        {
            if(occ == PlayerOccupation.ReadyArena1v1)
            {
                UIManager.data.currenOpenWindow.Close();
                return;
            }
            registerBtn.SetActive(occ != PlayerOccupation.RegisteredArena1v1);
            unregisterBtn.SetActive(occ == PlayerOccupation.RegisteredArena1v1);
            // info
            currentPoints.text = player.own.arena1v1Points.ToString();
            todayWins.text = player.own.arena1v1WinsToday.ToString();
            todayLosses.text = player.own.arena1v1LossesToday.ToString();
            TotalWins.text = player.own.archive.arena1v1Wins.ToString();
            TotalLosses.text = player.own.archive.arena1v1Losses.ToString();
        }
        public void OnRegister()
        {
            if(player.own.occupation != PlayerOccupation.None)
            {
                UINotifications.list.Add("Already registered", "مسجل بالفعل");
                return;
            }
            player.CmdRegisterInArena1v1();
        }
        public void OnUnregister()
        {
            if(player.own.occupation != PlayerOccupation.RegisteredArena1v1)
            {
                UINotifications.list.Add("Not registered", "غير مسجل");
                return;
            }
            player.CmdUnRegisterInArena1v1();
        }
        void OnEnable()
        {
            Refresh();
        }
    }
}