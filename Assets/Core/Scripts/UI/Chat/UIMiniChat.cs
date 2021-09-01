using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIMiniChat : MonoBehaviour
    {
        Player player => Player.localPlayer;
        List<ChatMessage> list => UIManager.data.chat != null ? UIManager.data.chat.msgList : new List<ChatMessage>();
        [SerializeField] int maxMessages = 4;
        [SerializeField] ChatChannels current = ChatChannels.All;
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        [SerializeField] string[] colors;

        public void AddMessage(ChatMessage msg)
        {
            if(current == ChatChannels.All || msg.channel == ChatChannels.System || current == msg.channel)
            {
                if(content.childCount == maxMessages)
                {
                    Destroy(content.GetChild(0).gameObject);
                }
                InstantiateMsg(msg);
            }
        }
        void InstantiateMsg(ChatMessage msg)
        {
            GameObject go = Instantiate(prefab, content.transform, false);
            string cn = $"<color=#{colors[(int)msg.channel]}>{LanguageManger.GetWord(54 + (int)msg.channel)}</color>";
            string sn = msg.channel != ChatChannels.System ? $" <b>{msg.sender.name}</b>:" : "";
            go.GetComponent<RTLTMPro.RTLTextMeshPro>().text = $"[{cn}]{sn} {msg.message}";
        }
        public void OnChangeChannel(int channel)
        {
            current = (ChatChannels)channel;
            Clear();
            if(list.Count < 1)
                return;
            if(current == ChatChannels.All)
            {
                int startIndex = list.Count > maxMessages ? list.Count - maxMessages : 0;
                for (int i = startIndex; i < list.Count; i++)
                {
                    InstantiateMsg(list[i]);
                }
            }
            else
            {
                List<ChatMessage> tempList = new List<ChatMessage>();
                for(int i = list.Count; i >= 0; i--)
                {
                    if(tempList.Count == maxMessages)
                        break;
                    if(list[i].channel == current)
                    {
                        tempList.Insert(0, list[i]);
                    }
                }
                if(tempList.Count < 1)
                    return;
                for (int i = 0; i < tempList.Count; i++)
                {
                    InstantiateMsg(list[i]);
                }
            }
        }
        void Clear()
        {
            if(content.childCount > 0)
            {
                for(int i = 0; i < content.childCount; i++)
                {
                    Destroy(content.GetChild(i).gameObject);
                }
            }
        }
        void OnEnable()
        {
            if(player != null)
            {
                OnChangeChannel((int)current);
            }
        }
    }
}