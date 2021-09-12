using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RTLTMPro;
using System;
using System.Collections.Generic;
namespace Game.UI
{
    public class Titles : Window
    {
        [Header("Currency")]
        [SerializeField] TMP_Text goldTxt;
        [SerializeField] TMP_Text diamondsTxt;
        [SerializeField] TMP_Text bDiamondsTxt;
        [Header("Category")]
        [SerializeField] GameObject categoryPrefab;
        [SerializeField] Transform categoryContent;
        [Header("Titles")]
        [SerializeField] GameObject titlePrefab;
        [SerializeField] Transform titlesContent;
        [Header("Info")]
        [SerializeField] Image titleImage;
        [SerializeField] GameObject locked;
        [SerializeField] TMP_Text brTxt;
        [SerializeField] RTLTextMeshPro sourceTxt;
        [SerializeField] Stats stats;
        void OnSelectTitle(int titleId)
        {
            if(ScriptableTitle.dict.TryGetValue(titleId, out ScriptableTitle title))
            {
                bool isActive = player.own.titles.Contains((ushort)titleId);
                titleImage.sprite = title.GetImage();
                brTxt.text = title.CalculateBR(isActive).ToString();
                sourceTxt.text = title.GetSource();
                locked.SetActive(!isActive);
                stats.SetTitle(title, isActive);
            }
            else
            {
                Notify.list.Add("Title not found");
                ResetTitleInfo();
            }
        }
        void OnSelectCategory(TitleCaregory caregory)
        {
            List<ScriptableTitle> titles = ScriptableTitle.GetByCategory(caregory);
            UIUtils.BalancePrefabs(titlePrefab, titles.Count, titlesContent);
            if(titles.Count > 0)
            {
                for (int i = 0; i < titles.Count; i++)
                {
                    TitleButton title = titlesContent.GetChild(i).GetComponent<TitleButton>();
                    if(title != null)
                    {
                        title.Set(titles[i]);
                        title.button.onClick = () => OnSelectTitle(titles[i].name);
                    }
                }
                OnSelectTitle(titles[0].name);
            }
            else
            {
                Notify.list.Add("no titles in this category yet");
            }
        }
        void ResetTitleInfo()
        {
            titleImage.sprite = null;
            brTxt.text = "";
            sourceTxt.text = "";
            locked.SetActive(false);
            stats.ClearAll();
        }
        public override void UpdateCurrency()
        {
            goldTxt.text = player.own.gold.ToString();
            diamondsTxt.text = player.own.diamonds.ToString();
            bDiamondsTxt.text = player.own.b_diamonds.ToString();
        }
        protected virtual void OnEnable()
        {
            base.OnEnable();
            OnSelectCategory((TitleCaregory)0);
        }
        void Awake()
        {
            int[] tgArr = (int[]) Enum.GetValues(typeof(TitleCaregory));
            UIUtils.BalancePrefabs(categoryPrefab, tgArr.Length, categoryContent);
            for (int i = 0; i < tgArr.Length; i++)
            {
                Transform cBtn = categoryContent.GetChild(i);
                cBtn.GetComponentInChildren<RTLTextMeshPro>().text = LanguageManger.GetWord(tgArr[i], LanguageDictionaryCategories.TitleCategory);
                cBtn.GetComponent<BasicButton>().onClick = () => OnSelectCategory((TitleCaregory)tgArr[i]);
            }
        }
    }
}