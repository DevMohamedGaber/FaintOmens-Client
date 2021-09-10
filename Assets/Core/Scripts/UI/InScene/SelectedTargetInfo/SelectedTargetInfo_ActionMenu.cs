using UnityEngine;
namespace Game.UI
{
    public class SelectedTargetInfo_ActionMenu : ActionMenu
    {
        bool isInGuild;
        bool isInTeam;
        public void Set(Player target, bool show = true)
        {
            isInGuild = target.InGuild();
            isInTeam = target.InTeam();
            base.Set(target.id, show);
        }
        public override bool IsTargetInTeam()
        {
            return isInTeam;
        }
        protected override bool IsTargetInGuild()
        {
            return isInGuild;
        }
        public override void Hide()
        {
            base.Hide();
            isInGuild = false;
        }
    }
}