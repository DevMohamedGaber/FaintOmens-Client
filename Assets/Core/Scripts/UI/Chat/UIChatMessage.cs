using UnityEngine;
using UnityEngine.UI;
using System;
using RTLTMPro;
namespace Game.UI
{
    public class UIChatMessage : MonoBehaviour
    {
        [SerializeField] Image avatar;
        [SerializeField] Image frame;
        [SerializeField] RTLTextMeshPro Name;
        [SerializeField] RTLTextMeshPro msg;
        [SerializeField] RTLTextMeshPro time;
        uint playerId;

        public void Set(ChatMessage data)
        {
            avatar.sprite = Storage.data.avatars[data.sender.avatar];
            frame.sprite = Storage.data.avatarFrames[data.sender.frame];
            Name.text = data.sender.name;
            Name.text += data.sender.vip > 0 ? $" [VIP {data.sender.vip}]" : "";
            msg.text = data.message;
            time.text = DateTime.FromOADate(data.sendTime).ToString("hh:mm");
            playerId = data.sender.id;
        }
    }
}