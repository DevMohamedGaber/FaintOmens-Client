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
        [SerializeField] RTLTextMeshPro nameTxt;
        [SerializeField] RTLTextMeshPro msg;
        [SerializeField] RTLTextMeshPro time;
        uint playerId;

        public void Set(ChatMessage data)
        {
            if(avatar != null)
            {
                avatar.sprite = UIManager.data.assets.avatars[data.sender.avatar];
            }
            if(frame != null)
            {
                frame.sprite = UIManager.data.assets.frames[data.sender.frame];
            }
            if(nameTxt != null)
            {
                nameTxt.text = $"{data.sender.name} {(data.sender.vip > 0 ? $" [VIP {data.sender.vip}]" : "")}";
            }
            if(msg != null)
            {
                msg.text = data.message;
            }
            if(time != null)
            {
                time.text = DateTime.FromOADate(data.sendTime).ToString("hh:mm");
            }
            playerId = data.sender.id;
        }
    }
}