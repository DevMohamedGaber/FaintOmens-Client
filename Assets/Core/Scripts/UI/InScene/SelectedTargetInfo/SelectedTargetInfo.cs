using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.UI
{
    public class SelectedTargetInfo : MonoBehaviour
    {
        [SerializeField] Image avatar;
        [SerializeField] UIProgressBar hpBar;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMP_Text levelTxt;
        [SerializeField] Sprite monsterAvatar;
        [SerializeField] SelectedTargetInfo_ActionMenu actionMenu;
        Player player => Player.localPlayer;
        Entity target => player.target;
        void Update()
        {
            if(player != null && target != null)
            {
                hpBar.fillAmount = target.HealthPercent();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            if(player != null && target != null)
            {
                if(target is Player tPlayer)
                {
                    avatar.sprite = UIManager.data.assets.avatars[(int)tPlayer.avatar];
                    nameTxt.text = tPlayer.name;
                    levelTxt.text = tPlayer.level.ToString();
                    hpBar.fillAmount = tPlayer.HealthPercent();
                    actionMenu.Set(tPlayer, false);
                }
                else if(target is Monster monster)
                {
                    avatar.sprite = monsterAvatar;
                    nameTxt.text = monster.Name;
                    levelTxt.text = monster.level.ToString();
                    hpBar.fillAmount = monster.HealthPercent();
                    actionMenu.Hide();
                }
            }
            else {
                Hide();
            }
        }
        public void Hide()
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                actionMenu.Hide();
            }
        }
        public void OnShowActionMenu()
        {
            if(player != null && target != null && target is Player)
            {
                actionMenu.Show();
            }
        }
    }
}