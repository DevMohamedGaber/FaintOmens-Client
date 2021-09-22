using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIArenaMatchNotify : MonoBehaviour
    {
        [SerializeField] Image timebar;
        [SerializeField] Transform allies;
        [SerializeField] Transform opponents;
        [SerializeField] GameObject prefab;
        Player player => Player.localPlayer;
        float timeLeft;
        void Update()
        {
            //double timeLeft = startTime - Server.time.ToOADate();
            timeLeft -= Time.deltaTime;
            timebar.fillAmount = timeLeft / Storage.data.arena.cancelTime;
            if(timeLeft <= 0)
            {
                Hide();
            }
        }
        public void Show1v1()
        {
            timeLeft = Storage.data.arena.cancelTime;
            // set self data
            UIArenaMatchNotifyOpponent selfData = Instantiate(prefab, allies, false).GetComponent<UIArenaMatchNotifyOpponent>();
            selfData.Set(player);
            // set opponent data
            Instantiate(prefab, opponents, false);
            gameObject.SetActive(true);
        }
        public void OnAccept()
        {
            if(player.own.occupation == PlayerOccupation.ReadyArena1v1)
            {
                player.CmdAcceptChallengeArena1v1();
            }
            //else if(player.own.occupation = PlayerOccupation.ReadyArena3v3)
            //else if(player.own.occupation = PlayerOccupation.ReadyArena5v5)
            else
            {
                Notifications.list.Add("You're not registered in any event", "انت غير مسجل باي حدث");
            }
        }
        public void OnRefuse()
        {
            if(player.own.occupation == PlayerOccupation.ReadyArena1v1)
            {
                player.CmdRefuseChallengeArena1v1();
            }
            //else if(player.own.occupation = PlayerOccupation.ReadyArena3v3)
            //else if(player.own.occupation = PlayerOccupation.ReadyArena5v5)
            else
            {
                Notifications.list.Add("You're not registered in any event", "انت غير مسجل باي حدث");
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            UIUtils.DestroyChildren(allies);
            UIUtils.DestroyChildren(opponents);
        }
    }
}