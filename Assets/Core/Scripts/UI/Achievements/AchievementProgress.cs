using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class AchievementProgress : MonoBehaviour
    {
        [SerializeField] TMP_Text pointsTxt;
        [SerializeField] RTLTextMeshPro descTxt;
        [SerializeField] UIProgressBar progressBar;
        [SerializeField] TMP_Text progressTxt;
        [SerializeField] Transform rewardsContent;
        [SerializeField] GameObject claimedObj;
        [SerializeField] GameObject claimBtn;
        [SerializeField] GameObject inProgressObj;
        ScriptableAchievement data;
        Player player => Player.localPlayer;
        public void Set(ScriptableAchievement data)
        {
            this.data = data;
            if(pointsTxt != null)
            {
                pointsTxt.text = data.points.ToString();
            }
            if(descTxt != null)
            {
                descTxt.text = data.GetDescription();
            }
            if(rewardsContent != null && data.rewards.Length > 0)
            {
                UIUtils.BalancePrefabs(UIManager.data.assets.itemSlots.normal, data.rewards.Length, rewardsContent);
                for (int i = 0; i < data.rewards.Length; i++)
                {
                    UIItemSlot slotSlot = rewardsContent.GetChild(i).GetComponent<UIItemSlot>();
                    if(slotSlot != null)
                    {
                        slotSlot.Assign(data.rewards[i]);
                    }
                }
            }
            RefreshProgress();
        }
        public void RefreshProgress()
        {
            if(progressBar != null)
            {
                progressBar.fillAmount = data.GetProgressPercentage();
            }
            if(progressTxt != null)
            {
                progressTxt.text = data.GetProgress();
            }
            int index = player.own.achievements.IndexOf((ushort)data.name);
            if(index > -1)
            {
                bool isClaimed = player.own.achievements[index].claimed;
                claimBtn.SetActive(!isClaimed);
                claimedObj.SetActive(isClaimed);
                inProgressObj.SetActive(false);
            }
            else
            {
                claimBtn.SetActive(false);
                claimedObj.SetActive(false);
                inProgressObj.SetActive(true);
            }
        }
        public void OnClaimReward()
        {
            if(player != null && data != null && data.name > 0)
            {
                player.CmdRecieveAchievementReward((ushort)data.name);
            }
        }
    }
}