using System;
using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class ToolTipBase : MonoBehaviour {
        [SerializeField] protected ToolTip_Info info;
        [SerializeField] protected ToolTip_Requirments reqs;
        [SerializeField] protected ToolTip_WearableItems wearable;
        [SerializeField] protected ToolTip_Buttons btns;
        protected int index = -1;
        protected ToolTipFrom from = ToolTipFrom.None;
        protected Player player => Player.localPlayer;
        public void Set(ToolTipFrom from, int index = -1) {
            this.from = from;
            this.index = index;
        }
        public void Hide() => gameObject.SetActive(false);
        protected void ClearAttributes() {
            if(wearable.attrs.Length > 0) {
                for(int i = 0; i < wearable.attrs.Length; i++) {
                    wearable.attrs[i].obj.SetActive(false);
                }
            }
        }

        [Serializable] public struct ToolTip_Info {
            public RTLTextMeshPro Name;
            public RTLTextMeshPro type;
            public RTLTextMeshPro desc;
            public Image icon;
            public Image qualityBackground;
        }
        [Serializable] public struct ToolTip_Requirments {
            public GameObject obj;
            public TMP_Text className;
            public TMP_Text level;
        }
        [Serializable] public struct ToolTip_WearableItems {
            public GameObject scroll;
            public ToolTip_GrowthEquipment growth;
            public ToolTip_Attribute[] attrs;
            public ToolTip_Sockets sockets;
            public RTLTextMeshPro desc;

            [Serializable] public struct ToolTip_GrowthEquipment {
                public GameObject obj;
                public TMP_Text maxQuality;
                public TMP_Text progress;
            }
            [Serializable] public struct ToolTip_Attribute {
                public GameObject obj;
                public TMP_Text value;
                public void Set(int baseValue, int bonus) {
                    value.text = $"{baseValue}  {(bonus > 0 ? $"<color=green>(+{bonus})</color>" : "")}";
                    obj.SetActive(true);
                }
                public void Set(float baseValue, float bonus) {
                    value.text = $"{baseValue.ToString("F0")}  {(bonus > 0 ? $"<color=green>(+{bonus.ToString("F0")}%)</color>" : "")}";
                    obj.SetActive(true);
                }
            }
            [Serializable] public struct ToolTip_Sockets {
                public GameObject obj;
                public ToolTip_Socket socket1;
                public ToolTip_Socket socket2;
                public ToolTip_Socket socket3;
                public ToolTip_Socket socket4;
                [Serializable] public struct ToolTip_Socket {
                    public Image icon;
                    public RTLTextMeshPro Name;
                    public void Set(Socket socket) {
                        if(socket.id == -1) {
                            icon.sprite = UIManager.data.assets.socketSlot[0];
                            Name.text = LanguageManger.Decide("Locked Slot", "مغلق");
                        }
                        else if(socket.id == 0) {
                            icon.sprite = UIManager.data.assets.socketSlot[1];
                            Name.text = LanguageManger.Decide("Empty Socket", "فارغ");
                        }
                        else {
                            icon.sprite = socket.data.image;
                            Name.text = $"{socket.Name} <color=green>{socket.bonusText}</color>";
                        }
                    }
                }
            }
        }
        [Serializable] public struct ToolTip_Buttons {
            public BasicButton useItem;
            public BasicButton sell;
            public BasicButton trade;

            public void ClearAll() {
                Clear(useItem);
                Clear(sell);
                Clear(trade);
            }
            void Clear(BasicButton button) {
                if(button != null) {
                    button.gameObject.SetActive(false);
                    button.onClick = null;
                }
            }

        }
    }
}