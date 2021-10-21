using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RTLTMPro;
using System;
using System.Collections.Generic;
namespace Game.UI
{
    public class Titles : WindowWithBasicCurrencies
    {
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
        [SerializeField] TitleStats stats;
        void OnSelectTitle(int titleId)
        {
            if(ScriptableTitle.dict.TryGetValue(titleId, out ScriptableTitle title))
            {
                bool isActive = player.own.titles.Contains((ushort)titleId);
                titleImage.sprite = title.GetImage();
                brTxt.text = title.CalculateBR(isActive).ToString();
                sourceTxt.text = title.GetSource();
                locked.SetActive(!isActive);
                stats.Set(title, isActive);
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
        protected virtual void OnEnable()
        {
            base.OnEnable();
            OnSelectCategory((TitleCaregory)0);
        }
        void Awake()
        {
            TitleCaregory[] tgArr = (TitleCaregory[]) Enum.GetValues(typeof(TitleCaregory));
            UIUtils.BalancePrefabs(categoryPrefab, tgArr.Length, categoryContent);
            for (int i = 0; i < tgArr.Length; i++)
            {
                Transform cBtn = categoryContent.GetChild(i);
                cBtn.GetComponentInChildren<RTLTextMeshPro>().text = LanguageManger.GetWord((int)tgArr[i], LanguageDictionaryCategories.TitleCategory);
                cBtn.GetComponent<BasicButton>().onClick = () => OnSelectCategory(tgArr[i]);
            }
        }
    }
}