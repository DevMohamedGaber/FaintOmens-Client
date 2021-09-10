using UnityEngine;
namespace Game.UI
{
    public class UISideBox_ActionMenu : ActionMenu
    {
        bool isOnline;
        byte level;
        public void Set(TeamMember member)
        {
            isOnline = member.online;
            base.Set(member.id);
        }
        protected override bool IsTargetOnline()
        {
            return isOnline;
        }
    }
}