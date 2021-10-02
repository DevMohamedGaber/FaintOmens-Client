using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class MountPage_StarUp : MountPage
    {
        [Header("Stats")]
        [SerializeField] MountStats stats;
        [SerializeField] GameObject requirementsObj;
        [SerializeField] GameObject maxedObj;
        [SerializeField] TMP_Text currentStarCount;
        [SerializeField] TMP_Text nextStarCount;
        [Header("Requirements")]
        [SerializeField] UIItemSlot slot;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        public override void Refresh()
        {
            base.Refresh();

            Mount info = player.own.mounts.Get(id);
            if(info.id != 0)
            {
                bool isMaxed = info.stars >= Storage.data.mount.starsCap;
                currentStarCount.text = info.stars.ToString();
                nextStarCount.text = !isMaxed ? info.stars.ToString() : info.stars.ToString();
                maxedObj.SetActive(isMaxed);
                requirementsObj.SetActive(!isMaxed);
                if(!isMaxed)
                {
                    stats.SetWithNextStarBonus(info);
                    if(nameTxt != null && slot.IsAssigned())
                    {
                        uint count = player.InventoryCountById(Storage.data.mount.starsUpItemId);
                        uint req = Storage.data.mount.starUpReqCount[(int)info.stars];
                        nameTxt.text = $"{slot.data.Name} <color={(count > req ? "green" : "red")}>{LanguageManger.UseSymbols($"{count} / {req}", "(", ")")}</color>";
                    }
                }
            }
            else
            {
                mounts.window.Status();
            }
        }
        public void OnStarUp()
        {
            int pIndex = player.own.mounts.Has(id);
            if(pIndex == -1)
            {
                Notify.list.Add("Mount isn't active", "المرافق غير مفعل");
                return;
            }
            byte currentStar = player.own.mounts[pIndex]._stars;
            if(currentStar >= Storage.data.mount.starsCap)
            {
                Notify.list.Add("Mount already reached max tier", "المرافق وصل لاعلي سمو");
                return;
            }
            if(player.InventoryCountById(Storage.data.mount.starsUpItemId) < Storage.data.mount.starUpReqCount[currentStar])
            {
                Notify.list.Add("Not enough starsUpItemId", "starsUpItemId غير كافي");
                return;
            }
            player.CmdMountStarUp(id);
        }
        void Awake()
        {
            slot.Assign(Storage.data.mount.starsUpItemId);
        }
    }
}