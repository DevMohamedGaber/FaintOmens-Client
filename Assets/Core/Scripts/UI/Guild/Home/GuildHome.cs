using UnityEngine;
namespace Game.UI
{
    public class GuildHome : GuildSubWindow
    {
        [SerializeField] GuildHome_Info info;
        public override void Refresh()
        {
            if(info.id != null)
            {
                info.id.text = data.id.ToString();
            }
            if(info.nameTxt != null)
            {
                info.nameTxt.text = data.Name;
            }
            if(info.masterName != null)
            {
                info.masterName.text = data.masterName;
            }
            if(info.level != null)
            {
                info.level.text = data.level.ToString();
            }
            if(info.membersCount != null)
            {
                info.membersCount.text = $"{data.membersCount} / {data.capacity}";
            }
            if(info.notice != null)
            {
                info.notice.text = data.notice;
            }
            if(info.brTxt != null)
            {
                info.brTxt.text = data.br.ToString();
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
    }
}