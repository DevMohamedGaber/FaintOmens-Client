using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Weapon", order=0)]
    public class WeaponItem : EquipmentItem
    {
        [Header("Weapon Info")]
        public WeaponType weaponType;
        [Header("Mastery")]
        public ScriptableSkill reqSkill;
        public byte reqSkillLevel;
        public override bool CanUse()
        {
            if(!base.CanUse())
                return false;
            if(reqSkill != null)
            {
                int index = player.skills.IndexOf(reqSkill.name);
                if(index > -1)
                {
                    if(player.skills[index].level < reqSkillLevel)
                    {
                        Notify.list.Add("Insufficint Skill level", "مستوي مهارة غير مناسب");
                        return false;
                    }
                    return true;
                }
                Notify.list.Add("The required skill is missing", "المهارة المطلوبة غير موجودة");
                return false;
            }
            return true;
        }
    }
}