using UnityEngine;
using System.Collections.Generic;
using System;
namespace Game
{
    [Serializable]
    public struct Item
    {
        public ushort id;
        public ItemQualityData quality;
        public byte plus;
        //public ushort progress; // if 0 not growth & if >= 1 is growth
        public Socket socket1;
        public Socket socket2;
        public Socket socket3;
        public Socket socket4;
        public ushort durability;
        public bool bound;

        public string Name => LanguageManger.GetWord(id, LanguageDictionaryCategories.ItemName);
        public string desc => LanguageManger.GetWord(id, LanguageDictionaryCategories.ItemDesc);
        public string typeName => LanguageManger.GetWord(id, LanguageDictionaryCategories.ItemTypes);
        public ScriptableItem data
        {
            get
            {
                if(!ScriptableItem.dict.ContainsKey(id))
                    throw new KeyNotFoundException("There is no ScriptableItem with ID=" + id + ". Make sure that all ScriptableItems are in the Resources folder so they are loaded properly.");
                return ScriptableItem.dict[id];
            }
        }
        
        #region For Equipments
        public EquipmentItem dataEquip => (EquipmentItem)data;
        // bonus
        public int health => dataEquip.health.GetValue(plus, quality.current);
        public int healthBonus => dataEquip.health.GetBonus(plus, quality.current);
        public int mana => dataEquip.mana.GetValue(plus, quality.current);
        public int manaBonus => dataEquip.mana.GetBonus(plus, quality.current);
        public int pAtk => dataEquip.PAtk.GetValue(plus, quality.current);
        public int pAtkBonus => dataEquip.PAtk.GetBonus(plus, quality.current);
        public int mAtk => dataEquip.MAtk.GetValue(plus, quality.current);
        public int mAtkBonus => dataEquip.MAtk.GetBonus(plus, quality.current);
        public int pDef => dataEquip.PDef.GetValue(plus, quality.current);
        public int pDefBonus => dataEquip.PDef.GetBonus(plus, quality.current);
        public int mDef => dataEquip.MDef.GetValue(plus, quality.current);
        public int mDefBonus => dataEquip.MDef.GetBonus(plus, quality.current);
        public float critRate => dataEquip.critRate.GetValue(plus, quality.current);
        public float critRateBonus => dataEquip.critRate.GetBonus(plus, quality.current);
        public float critDmg => dataEquip.critDmg.GetValue(plus, quality.current);
        public float critDmgBonus => dataEquip.critDmg.GetBonus(plus, quality.current);
        public float block => dataEquip.block.GetValue(plus, quality.current);
        public float blockBonus => dataEquip.block.GetBonus(plus, quality.current);
        public ushort MaxDurability()
        {
            return data is EquipmentItem ? (ushort)
                                            ((dataEquip.durability + ( plus * 10)) * ((int)quality.current + 1)) : (ushort)0;
        }
        #endregion
        // equipment's functions
        public void SetQualityEffect(GameObject go)
        {
            if(data is EquipmentItem)
            {
                if(quality.current == Quality.Legendary && ((EquipmentItem)data).QualityEffect != null)
                {
                    GameObject qualityEffect = GameObject.Instantiate(((EquipmentItem)data).QualityEffect);
                    //qualityEffect.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect(go);
                    qualityEffect.transform.SetParent(go.transform, false);
                }
            }
        }
        public Socket GetSocket(int index)
        {
            if(index == 0)
            {
                return socket1;
            }
            if(index == 1)
            {
                return socket2;
            }
            if(index == 2)
            {
                return socket3;
            }
            if(index == 3)
            {
                return socket4;
            }
            return Socket.Empty;
        }
        public bool HasGemWithType(BonusType bonusType)
        {
            if( (socket1.id > 0 && socket1.type == bonusType) ||
                (socket2.id > 0 && socket2.type == bonusType) || 
                (socket3.id > 0 && socket3.type == bonusType) ||
                (socket4.id > 0 && socket4.type == bonusType))
            {
                return true;
            }
            return false;
        }
        public float GetSocketOfType(BonusType bonusType)
        {
            if(socket1.id > 0 && socket1.type == bonusType)
            {
                return socket1.bonus;
            }
            if(socket2.id > 0 && socket2.type == bonusType)
            {
                return socket2.bonus;
            }
            if(socket3.id > 0 && socket3.type == bonusType)
            {
                return socket3.bonus;
            }
            if(socket4.id > 0 && socket4.type == bonusType)
            {
                return socket4.bonus;
            }
            return 0f;
        }
        public void Reset(ushort newId = 0)
        {
            id = newId;
            plus = 0;
            quality.Reset();
            socket1.id = -1;
            socket2.id = -1;
            socket3.id = -1;
            socket4.id = -1;
            bound = false;
        }
    }
}