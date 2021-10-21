using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.UI
{
    public class UILocalPlayerInfo : MonoBehaviour
    {
        public static UILocalPlayerInfo singleton;
        [SerializeField] Image avatar;
        [SerializeField] Image avatarFrame;
        [SerializeField] RTLTMPro.RTLTextMeshPro brTxt;
        [SerializeField] TMP_Text levelTxt;
        [SerializeField] TMP_Text vipLevelTxt;
        [SerializeField] UIProgressBar hpSlider;
        [SerializeField] TMP_Text hpTxt;
        [SerializeField] UIProgressBar mpSlider;
        [SerializeField] TMP_Text mpTxt;
        [SerializeField] Transform buffList;
        [SerializeField] TMP_Text buffsCount;
        [SerializeField] GameObject noBuffs;
        //[SerializeField] UIBuffSlot slotPrefab;

        Player player => Player.localPlayer;

        public void UpdateHealth()
        {
            if(hpSlider != null)
            {
                hpSlider.fillAmount = player.HealthPercent();
            }
            if(hpTxt != null)
            {
                hpTxt.text = (player.HealthPercent() * 100).ToString("F0") + "%";
            }
        }
        public void UpdateMana()
        {
            if(hpSlider != null)
            {
                mpSlider.fillAmount = player.ManaPercent();
            }
            if(hpTxt != null)
            {
                mpTxt.text = (player.ManaPercent() * 100).ToString("F0") + "%";
            }
        }
        public void UpdateFrame()
        {
            if(avatarFrame != null)
            {
                avatarFrame.sprite = UIManager.data.assets.frames[(int)player.frame];
            }
        }
        public void UpdateAvatar()
        {
            if(avatar != null)
            {
                avatar.sprite = UIManager.data.assets.avatars[(int)player.avatar];
            }
        }
        public void UpdateLevel()
        {
            if(levelTxt != null)
            {
                levelTxt.text = player.level.ToString();
            }
        }
        public void UpdateVipLevel()
        {
            if(vipLevelTxt != null)
            {
                vipLevelTxt.text = $"VIP{player.own.vip.level.ToString()}";
            }
        }
        public void UpdateBR()
        {
            if(brTxt != null)
            {
                brTxt.text = LanguageManger.Decide("BR: ", "قوة: ") + player.battlepower.ToString();
            }
        }
        public void UpdateBuffs()
        {
            if(buffList != null)
            {
                
            }
            if(noBuffs != null)
            {
                noBuffs.SetActive(player.buffs.Count == 0);
            }
            if(buffsCount != null)
            {
                buffsCount.text = LanguageManger.Decide("Buffs x", "زيادة x") + player.buffs.Count.ToString();
            }
        }
    }
}