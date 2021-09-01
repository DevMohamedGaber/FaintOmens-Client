namespace Game.UI
{
    public class UISocketSlot : UIItemSlot {
        public void Lock() {
            this.Unassign();
            this.SetIcon(Storage.data.lockImage);
        }
    }
}