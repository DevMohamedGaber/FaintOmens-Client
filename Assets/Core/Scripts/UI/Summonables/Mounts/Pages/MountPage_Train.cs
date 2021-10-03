using UnityEngine;
using TMPro;
namespace Game.UI
{
    public class MountPage_Train : MountPage
    {
        [Header("Training")]
        [SerializeField] UIItemSlot itemSlot;
        [SerializeField] RTLTMPro.RTLTextMeshPro itemName;
        [SerializeField] UIToggleGroup toggleGroup;
        [Header("Attributes")]
        [SerializeField] MountPage_Train_Attribute vitality;
        [SerializeField] MountPage_Train_Attribute strength;
        [SerializeField] MountPage_Train_Attribute intelligence;
        [SerializeField] MountPage_Train_Attribute endurance;
        public override void Refresh()
        {
            base.Refresh();

            Mount info = player.own.mounts.Get(id);
            if(info.id != 0)
            {
                vitality.Set(info.training.vitality);
                strength.Set(info.training.strength);
                intelligence.Set(info.training.intelligence);
                endurance.Set(info.training.endurance);
                if(itemName != null && itemSlot != null && itemSlot.IsAssigned())
                {
                    uint count = player.InventoryCountById(Storage.data.mount.trainItemId);
                    itemName.text = $"{itemSlot.data.Name} <color={(count > 0 ? "green" : "red")}>({count})</color>";
                }
            }
            else
            {
                mounts.window.Status();
            }
        }
        public void OnTrain()
        {
            if(!IsTrainable())
                return;
            player.CmdMountTrain(id, (byte)toggleGroup.currentIndex);
        }
        public void OnTrainx10()
        {
            if(!IsTrainable())
                return;
            player.CmdMountTrainx10(id, (byte)toggleGroup.currentIndex);
        }
        bool IsTrainable()
        {
            int index = player.own.mounts.Has(id);
            if(index == -1)
            {
                Notify.list.Add("Mount isn't active", "المرافق غير مفعل");
                return false;
            }
            if(toggleGroup.currentIndex == -1 || toggleGroup.currentIndex > 3)
            {
                Notify.list.Add("Please select an attribute");
                return false;
            }
            if(!CanTrainAttribute(index))
            {
                Notify.list.Add("attribute level can't excede mount level");
                return false;
            }
            if(player.InventoryCountById(Storage.data.mount.trainItemId) < 1)
            {
                Notify.list.Add($"You don't have enough [{itemSlot.data.Name}]");
                return false;
            }
            return true;
        }
        bool CanTrainAttribute(int index)
        {
            Mount mount = player.own.mounts[index];
            if(toggleGroup.currentIndex == 0)
            {
                return mount.training.vitality.level < mount.level;
            }
            if(toggleGroup.currentIndex == 1)
            {
                return mount.training.strength.level < mount.level;
            }
            if(toggleGroup.currentIndex == 2)
            {
                return mount.training.intelligence.level < mount.level;
            }
            if(toggleGroup.currentIndex == 3)
            {
                return mount.training.endurance.level < mount.level;
            }
            return false;
        }
        void Awake()
        {
            itemSlot.Assign(Storage.data.mount.trainItemId);
        }
        [System.Serializable]
        public struct MountPage_Train_Attribute
        {
            [SerializeField] TMP_Text levelTxt;
            [SerializeField] TMP_Text pointsTxt;
            [SerializeField] TMP_Text expTxt;
            [SerializeField] UIProgressBar progressBar;
            public void Set(MountTrainingAttribute data)
            {
                if(levelTxt != null)
                {
                    levelTxt.text = data.level.ToString();
                }
                if(pointsTxt != null)
                {
                    pointsTxt.text = (data.level * Storage.data.mount.pointPerTrainingLevel).ToString();
                }
                if(expTxt != null)
                {
                    expTxt.text = $"{data.exp} / {data.expMax}";
                }
                if(progressBar != null)
                {
                    progressBar.fillAmount = (float)data.exp / (float)data.expMax;
                }
            }
        }
    }
}