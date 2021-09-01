using UnityEngine;
namespace Game.UI
{
    public class UIRecievableItemSlot : UIItemSlot {
        [SerializeField] protected GameObject Recieved;
        public void Assign(ItemSlot item, bool isRecieved = false) {
            this.Assign(item);
            Recieved.SetActive(isRecieved);
        }
        public void SetRecieved(bool isRecieved) => Recieved.SetActive(isRecieved);
    }
}