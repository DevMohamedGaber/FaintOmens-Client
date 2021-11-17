using UnityEngine;
namespace Game.UI
{
    public class UIWardrobeFashionItem : MonoBehaviour
    {
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro Name;
        [SerializeField] GameObject assignBtn;
        [SerializeField] GameObject notActiveObj;
        ushort id;
        Player player => Player.localPlayer;
        bool isActive => id > 0 ? player.own.wardrobe.IndexOf(id) > -1 : false;
        public void Set(int id)
        {
            this.id = (ushort)id;
            bool active = isActive;
            slot.Assign(ScriptableWardrobe.dict[id].itemId);
            Name.text = ScriptableWardrobe.dict[id].Name;
            assignBtn.SetActive(active && player.own.clothing[(int)ScriptableWardrobe.dict[id].category].id != id);
            notActiveObj.SetActive(!active);
        }
        public void OnTry()
        {
            PlayerPreviewModel previewModel = PreviewManager.singleton.go.GetComponent<PlayerPreviewModel>();
            if(previewModel != null)
            {
                PlayerModelData modelData = previewModel.model;
                modelData.AddTo(ScriptableWardrobe.dict[id].category, id);
                previewModel.RefreshAllLocation();
            }
        }
        public void OnUse()
        {
            if(isActive) {
                player.CmdWardrobeEquip(id);
                Debug.Log("called");
            }
            else
            {
                Notifications.list.Add("Wardrop isn't active", "العتاد غير مفعل");
            }
        }
    }
}