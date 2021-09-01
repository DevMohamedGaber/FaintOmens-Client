using UnityEngine;
using System;
using Mirror;
namespace Game
{
    public class ChatComponent : NetworkBehaviourNonAlloc
    {
        public Player player;
        #region Sending
        public bool OnSubmit(ChatChannels channel, string msg, uint pmId = 0)
        {
            if(!IsValid(msg))
                return false; // guard

            if(channel == ChatChannels.Local)
            {
                CmdMsgLocal(msg);
                return true;
            }
            else if(channel == ChatChannels.World)
            {
                CmdMsgWorld(msg);
                return true;
            }
            else if(channel == ChatChannels.Tribe)
            {
                CmdMsgTribe(msg);
                return true;
            }
            else if(channel == ChatChannels.Guild)
            {
                if(!player.InGuild())
                {
                    Notify.list.Add("You're not in guild", "لست بنقابة");
                    return false;
                }
                CmdMsgGuild(msg);
                return true;
            }
            else if(channel == ChatChannels.Team)
            {
                if(!player.InTeam())
                {
                    Notify.list.Add("You're not in Team", "لست مسجل بفريق");
                    return false;
                }
                CmdMsgTeam(msg);
                return true;
            }
            else if(channel == ChatChannels.Whisper)
            {
                if(pmId < 1)
                {
                    Notify.list.Add("please select a target", "برجاء اختيار هدف");
                    return false;
                }
                CmdMsgWhisper(pmId, msg);
                return true;
            }
            return false;
        }
        bool IsValid(string msg)
        {
            if(string.IsNullOrWhiteSpace(msg))
            {
                Notify.list.Add("No Message", "لا يوجد رسالة");
                return false;
            }
            if(msg.Length > Storage.data.chatMaxMsgSize)
            {
                Notify.list.Add($"Message is More than {Storage.data.chatMaxMsgSize} characters", $"الرسالة اكثر من {Storage.data.chatMaxMsgSize} حرف");
                return false;
            }
            return true;
        }
        #endregion
        [Command] void CmdMsgWorld(string message) {}
        [Command] void CmdMsgTribe(string message) {}
        [Command] void CmdMsgGuild(string message) {}
        [Command] void CmdMsgLocal(string message) {}
        [Command] void CmdMsgTeam(string message) {}
        [Command] void CmdMsgWhisper(uint playerId, string message) {}
        [TargetRpc] public void TargetMsgGeneral(ChatMessage msg)
        {
            UIManager.data.chat.AddMessage(msg);
            UIManager.data.miniChat.AddMessage(msg);
        }
        [ClientRpc] public void RpcMsgLocal(ChatMessage msg)
        {
            UIManager.data.chat.AddMessage(msg);
            UIManager.data.miniChat.AddMessage(msg);
        }
    }
}