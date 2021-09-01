using UnityEngine;
namespace Game.UI
{
    public class UILeaveEventButton : MonoBehaviour {
        Player player => Player.localPlayer;
        public void OnClick() {
            if(player.own.occupation != PlayerOccupation.None) {
                if(player.own.occupation == PlayerOccupation.InMatchArena1v1)
                    player.CmdLeaveArena1v1();
            }
            else UINotifications.list.Add("You're not in an Event", "انت لست في حدث");
        }
    }
}