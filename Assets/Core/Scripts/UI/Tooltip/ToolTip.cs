using UnityEngine;
namespace Game.UI
{
    public class ToolTip : ToolTipBase
    {
        // item
        public void Show(Item item, ToolTipFrom from = ToolTipFrom.None, int index = -1)
        {
            Set(from, index);
            ShowItem(item);
            gameObject.SetActive(true);
        }
        void ShowItem(Item item)
        {
            // set info
            info.icon.sprite = item.data.GetImage();
            info.Name.text = item.Name;
            info.Name.color = item.quality.data.color;
            info.type.text = item.typeName;
            // quality background
            info.qualityBackground.gameObject.SetActive(item.quality.current != Quality.Normal);
            if(item.quality.current != Quality.Normal)
            {
                info.qualityBackground.color = item.quality.data.color;
            }
            // set by type
            bool isEquipment = item.data is EquipmentItem;
            reqs.obj.SetActive(isEquipment);
            wearable.scroll.SetActive(isEquipment);
            wearable.sockets.obj.SetActive(isEquipment);
            // for equipments
            if(isEquipment)
            {
                SetEquipmentItem(item);
            }
            OperateOnButtons(item);
        }
        void SetEquipmentItem(Item item)
        {
            EquipmentItem itemData = (EquipmentItem)item.data;
            // set requirments
            reqs.level.text = itemData.minLevel.ToString();
            reqs.level.color = player.level >= itemData.minLevel ? Color.white : Color.red;
            reqs.className.text = itemData.reqClass.Name;
            reqs.className.color = player.classInfo.Validate(itemData.reqClass) ? Color.white : Color.red;

            // attributes
            if(itemData.health.Base > 0) // health
                wearable.attrs[0].Set(itemData.health.Base, item.healthBonus);
            if(itemData.mana.Base > 0) // mana
                wearable.attrs[1].Set(itemData.mana.Base, item.manaBonus);
            if(itemData.PAtk.Base > 0) // PAtk
                wearable.attrs[2].Set(itemData.PAtk.Base, item.pAtkBonus);
            if(itemData.MAtk.Base > 0) // MAtk
                wearable.attrs[3].Set(itemData.MAtk.Base, item.mAtkBonus);
            if(itemData.PDef.Base > 0) // PDef
                wearable.attrs[4].Set(itemData.PDef.Base, item.pDefBonus);
            if(itemData.MDef.Base > 0) // MDef
                wearable.attrs[5].Set(itemData.MDef.Base, item.mDefBonus);
            if(itemData.critRate.Base > 0) // critRate
                wearable.attrs[6].Set(itemData.critRate.Base, item.critRateBonus);
            if(itemData.critDmg.Base > 0) // critDmg
                wearable.attrs[7].Set(itemData.critDmg.Base, item.critDmgBonus);
            if(itemData.block.Base > 0) // block
                wearable.attrs[8].Set(itemData.block.Base, item.blockBonus);
            // quality growth
            wearable.growth.obj.SetActive(item.quality.isGrowth);
            if(item.quality.isGrowth)
            {
                wearable.growth.maxQuality.text = item.quality.max.ToString();
                wearable.growth.maxQuality.color = ScriptableQuality.dict[(int)item.quality.max].color;
                wearable.growth.progress.text = $"{item.quality.progress} / {item.quality.expMax}  ({((item.quality.progress / item.quality.expMax) * 100).ToString("F0")}%)";
            }
            // sockets
            wearable.sockets.socket1.Set(item.socket1);
            wearable.sockets.socket2.Set(item.socket2);
            wearable.sockets.socket3.Set(item.socket3);
            wearable.sockets.socket4.Set(item.socket4);
        }
        void OperateOnButtons(Item item)
        {
            bool canUse = from == ToolTipFrom.Inventory && (item.data is UsableItem);
            btns.useItem.gameObject.SetActive(canUse);
            if(canUse)
            {
                btns.useItem.onClick = () => OnUseItem(item);
            }
        }
        public void OnUseItem(Item item)
        {
            if(from == ToolTipFrom.Inventory)
            {
                if(item.data is UsableItem usableItem && ((UsableItem)item.data).CanUse())
                {
                    player.CmdUseInventoryItem(index);
                    Hide();
                }
            }
        }
        // skill
        public void Show(Skill skill)
        {

        }
        void OnDisable()
        {
            ClearAttributes();
            index = -1;
            from = ToolTipFrom.None;
            btns.ClearAll();
        }
    }
}