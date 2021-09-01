using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIGuildJoinRequests : MonoBehaviour {
        Player player => Player.localPlayer;
        void OnEnable() {
            player.CmdGuildJoinRequests();
        }
    }
}