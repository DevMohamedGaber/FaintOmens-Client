using System.Text;
using UnityEngine;
using System;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Title", order=0)]
    public class TitleItem : UsableItem
    {
        public int titleId;
        public override bool CanUse() {
            for(int i = 0; i < player.own.titles.Count; i++)
            {
                if(player.own.titles[i] == titleId)
                {
                    Notify.list.Add("This Title is already has been activated", "تم تفعيل هذا اللقب بالفعل");
                    return false;
                }
            }
            return true;
        }
    }
}