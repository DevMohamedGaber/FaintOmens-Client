using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using RTLTMPro;
using Game.Achievements;
namespace Game.UI
{
    public class Achievements : WindowWithBasicCurrencies
    {
        [Header("category")]
        [SerializeField] GameObject categoryPrefab;
        [SerializeField] Transform categoryContent;
        [SerializeField] UIToggleGroup categoryToggleGroup;
        [Header("Overview")]
        [SerializeField] GameObject overviewObj;
        [SerializeField] UIProgressBar totalProgressBar;
        [SerializeField] TMP_Text totalProgressTxt;
        [SerializeField] Transform inprogressContent;
        [SerializeField] GameObject nothingInProgressObj;
        [Header("Achievements List")]
        [SerializeField] GameObject listObj;
        [SerializeField] Transform achievementsContent;
        [SerializeField] GameObject achievementPrefab;
        public override void Refresh()
        {
            if(overviewObj.activeSelf)
            {
                RefreshOverview();
            }
            else
            {
                RefreshList();
            }
        }
        void RefreshOverview()
        {
            // progress
            ushort currentTotal = player.own.achievements.GetTotalPoints();
            totalProgressTxt.text = $"{currentTotal} / {ScriptableAchievement.totalPoints}";
            totalProgressBar.fillAmount = (float)(currentTotal / ScriptableAchievement.totalPoints) * 100f;
            
            // not claimed and in progress
            InprogressAchievements inprogress = new InprogressAchievements(player);
            List<ScriptableAchievement> inprogressList = inprogress.GetAchievements();
            List<ushort> notClaimedList = player.own.achievements.NeedClaiming();

            UIUtils.BalancePrefabs(achievementPrefab, inprogressList.Count + notClaimedList.Count, inprogressContent);
            nothingInProgressObj.SetActive((inprogressList.Count + notClaimedList.Count) < 1);
            int i = 0;

            if(notClaimedList.Count > 0)
            {
                for (i = 0; i < notClaimedList.Count; i++)
                {
                    AchievementProgress achievement = inprogressContent.GetChild(i).GetComponent<AchievementProgress>();
                    if(achievement != null)
                    {
                        achievement.Set(ScriptableAchievement.dict[notClaimedList[i]]);
                    }
                }
            }

            if(inprogressList.Count > 0)
            {
                for (i = i; i < inprogressList.Count; i++)
                {
                    AchievementProgress achievement = inprogressContent.GetChild(i).GetComponent<AchievementProgress>();
                    if(achievement != null)
                    {
                        achievement.Set(inprogressList[i]);
                    }
                }
            }
        }
        void RefreshList()
        {
            if(achievementsContent.childCount > 0)
            {
                for (int i = 0; i < achievementsContent.childCount; i++)
                {
                    AchievementProgress achievement = inprogressContent.GetChild(i).GetComponent<AchievementProgress>();
                    if(achievement != null)
                    {
                        achievement.RefreshProgress();
                    }
                }
            }
        }
        public void ShowOverview()
        {
            if(!overviewObj.activeSelf)
            {
                overviewObj.SetActive(false);
                listObj.SetActive(true);
                RefreshOverview();
            }
        }
        public void OnSelectCategory(AchievementCategory category)
        {
            if(overviewObj.activeSelf)
            {
                overviewObj.SetActive(false);
                listObj.SetActive(true);
            }
            List<ScriptableAchievement> results = ScriptableAchievement.GetByCategory(category);
            UIUtils.BalancePrefabs(achievementPrefab, results.Count, achievementsContent);
            if(results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    AchievementProgress achievement = achievementsContent.GetChild(i).GetComponent<AchievementProgress>();
                    if(achievement != null)
                    {
                        achievement.Set(results[i]);
                    }
                }
            }
            else
            {
                Notify.list.Add("no Achievements in this category yet");
            }
        }
        protected virtual void OnEnable()
        {
            base.OnEnable();
            ShowOverview();
        }
        void Awake()
        {
            int catsCount = ScriptableAchievement.GetCategoryCount();
            UIUtils.BalancePrefabs(categoryPrefab, catsCount, categoryContent);
            for (int i = 0; i < catsCount; i++)
            {
                Transform cBtn = categoryContent.GetChild(i);
                cBtn.GetComponentInChildren<RTLTextMeshPro>().text = LanguageManger.GetWord(i, LanguageDictionaryCategories.AchievementCategory);
                cBtn.GetComponent<UIToggle>().onSelect = () => OnSelectCategory((AchievementCategory)i);
            }
            categoryToggleGroup.UpdateTogglesList();
        }
    }
}