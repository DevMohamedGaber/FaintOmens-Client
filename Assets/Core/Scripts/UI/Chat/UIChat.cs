using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIChat : MonoBehaviour {
        public List<ChatMessage> msgList = new List<ChatMessage>();
        Player player => Player.localPlayer;
        [SerializeField] int maxPerChannel = 30;
        int[] msgCount = new int[7];
        [SerializeField] ChatChannels current = ChatChannels.All;
        [SerializeField] Transform content;
        [SerializeField] GameObject noMsgsObj;
        [SerializeField] GameObject cantWriteObj;
        [SerializeField] GameObject sendBtn;
        [SerializeField] TMPro.TMP_InputField input;
        [SerializeField] GameObject msgPrefab;
        [SerializeField] GameObject myMsgPrefab;
        [SerializeField] GameObject systemMsgPrefab;
        [SerializeField] Button guildBtn;
        [SerializeField] Button partyBtn;
        [SerializeField] GameObject pmInputObj;
        [SerializeField] InputField pmInput;
        [SerializeField] uint pmId;
        public void OnChangeChannel(int channel) {
            current = (ChatChannels)channel;
            pmInputObj.SetActive(current == ChatChannels.Whisper);
            bool canWrite = current != ChatChannels.All && current != ChatChannels.System;
            input.gameObject.SetActive(canWrite);
            sendBtn.SetActive(canWrite);

            noMsgsObj.SetActive(msgList.Count < 1);
            cantWriteObj.SetActive(!canWrite);

            Clear();
            if(msgList.Count < 1) return;

            if(current != ChatChannels.All) {
                List<int> channelMsgs = new List<int>();
                for(int i = 0; i < msgList.Count; i++) {
                    if(msgList[i].channel == current)
                        channelMsgs.Add(i);
                }
                if(channelMsgs.Count < 1) return;
                for(int i = 0; i < channelMsgs.Count; i++)
                    InstantiateMsg(msgList[channelMsgs[i]]);
                if(current == ChatChannels.Whisper && Server.IsPlayerIdWithInServer(pmId))
                    player.CmdNotifyIfPlayerOffline(pmId);
            }
            else {
                int displayCount = msgList.Count > 100 ? 100 : msgList.Count;
                for(int i = 0; i < displayCount; i++)
                    InstantiateMsg(msgList[i]);
            }
        }
        public void AddMessage(ChatMessage msg) {
            msgList.Add(msg);
            if(msgCount[(int)msg.channel] == maxPerChannel) 
                msgList.RemoveAt(0);
            else msgCount[(int)msg.channel]++;

            if(gameObject.activeSelf && (current == ChatChannels.All || current == msg.channel)) {
                if(msgCount[(int)msg.channel] == maxPerChannel)
                    Destroy(content.GetChild(0).gameObject);
                InstantiateMsg(msg);
            }
        }
        void InstantiateMsg(ChatMessage msg) {
            if(msg.channel != ChatChannels.System) {
                GameObject go = Instantiate(msg.sender.id != player.id ? msgPrefab : myMsgPrefab, content.transform, false);
                go.GetComponent<UIChatMessage>().Set(msg);
            } else {
                GameObject go = Instantiate(systemMsgPrefab, content.transform, false);
                go.GetComponent<RTLTMPro.RTLTextMeshPro>().text = $"[{LanguageManger.GetWord(54)}]: {msg.message}";
            }
        }
        void Clear() {
            if(content.childCount > 0)
                for(int i = 0; i < content.childCount; i++)
                    Destroy(content.GetChild(i).gameObject);
        }
        public void OnChangePMID(string v) {
            pmId = System.Convert.ToUInt32(v);
        }
        public void OnSubmit() {
            if(player.chat.OnSubmit(current, input.text, pmId))
                input.text = "";
        } 
        public void OpenPrivateChatWith(uint id) {
            if(Server.IsPlayerIdWithInServer(id)) {
                pmInput.text = id.ToString();
                pmId = id;
                current = ChatChannels.Whisper;
                gameObject.SetActive(true);
            }
        }
        void OnEnable() {
            if(player != null) {
                OnChangeChannel((int)current);
                guildBtn.interactable = player.InGuild();
                partyBtn.interactable = player.InTeam();
            }
            else gameObject.SetActive(false);
        }
    }
}