using UnityEngine;
namespace Game.UI
{
    public class UIWardrobeFashionItem : MonoBehaviour {
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro Name;
        [SerializeField] GameObject assignBtn;
        [SerializeField] GameObject notActiveObj;
        ushort id;
        Player player => Player.localPlayer;
        bool isActive => id > 0 ? player.own.wardrobe.IndexOf(id) > -1 : false;
        public void Set(int id) {
            this.id = (ushort)id;
            bool active = isActive;
            slot.Assign(ScriptableWardrobe.dict[id].itemId);
            Name.text = ScriptableWardrobe.dict[id].Name;
            assignBtn.SetActive(active && player.wardrobe[(int)ScriptableWardrobe.dict[id].category].id != id);
            notActiveObj.SetActive(!active);
        }
        public void OnTry() {
            Player preview = UIPreviewManager.singleton.go.GetComponent<Player>();
            SyncListWardrop items = preview.wardrobe;
            items[(int)ScriptableWardrobe.dict[id].category] = new WardrobeItem(id);
            preview.wardrobe = items;
            preview.RefreshAllLocation();
        }
        public void OnUse() {
            if(isActive) {
                player.CmdWardrobeEquip(id);
                Debug.Log("called");
            }
            else Notifications.list.Add("Wardrop isn't active", "العتاد غير مفعل");
        }
    }
}