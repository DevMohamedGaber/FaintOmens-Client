using UnityEngine;
namespace Game.UI
{
    public class GuildSubWindow : SubWindow
    {
        protected Guild window => UIManager.data.pages.guild;
        protected Game.Guild data => window.data;
        protected Game.GuildMember myData => window.myData;
    }
}