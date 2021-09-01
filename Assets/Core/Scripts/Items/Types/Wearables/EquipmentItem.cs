using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Equipment", order=0)]
    public class EquipmentItem : UsableItem
    {
        [Header("Equipment")]
        public EquipmentsCategory category;
        public int setId = -1;
        public ushort durability = 20;
        [Header("Bonus")]
        public EquipmentBonus health;
        public EquipmentBonus mana;
        public EquipmentBonus PAtk;
        public EquipmentBonus PDef;
        public EquipmentBonus MAtk;
        public EquipmentBonus MDef;
        public EquipmentFloatBonus critRate;
        public EquipmentFloatBonus critDmg;
        public EquipmentFloatBonus antiCrit;
        public EquipmentFloatBonus block;
        public EquipmentFloatBonus antiBlock;
        [Header("Models")]
        public GameObject[] modelPrefab; // [0] -> female / [1] -> male
        public GameObject QualityEffect;

        public override bool CanUse()
        {
            if(player.level < minLevel)
            {
                Notify.list.Add("Insufitint level", "مستوي غير مناسب");
                return false;
            }
            if(!player.classInfo.Validate(reqClass))
            {
                Notify.list.Add("Insufitint Class", "تخصص غير مناسب");
                return false;
            }
            return true;
        }
        public bool CanEquip(int inventoryIndex, int equipmentIndex)
        {
            //StuffCategory requiredCategory = player.equipmentInfo[equipmentIndex].requiredCategory;
            return base.CanUse()/* && requiredCategory != null && category == requiredCategory*/;
        }

        int FindEquipableSlotFor(int inventoryIndex)
        {
            for (int i = 0; i < player.equipment.Count; ++i)
            {
                if (CanEquip(inventoryIndex, i))
                {
                    return i;
                }
            }
            return -1;
        }
        void OnValidate()
        {
            type = ItemType.EquipmentItem;
        }
    }
}