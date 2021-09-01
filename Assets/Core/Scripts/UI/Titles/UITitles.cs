using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UITitles : MonoBehaviour {
        [Header("General")]
        [SerializeField, Range(0, 1)] float updateInterval = .3f;
        [SerializeField] UILanguageDefiner lang;
        [SerializeField] GameObject categoryPrefab;
        [SerializeField] GameObject btnPrefab;
        [SerializeField] Transform content;
        [SerializeField] Color notActiveColor;
        Player player => Player.localPlayer;
        List<UICollapsableCategory> categories = new List<UICollapsableCategory>();
        int currentCategory = -1;
        int current = -1;
        [Header("Info")]
        [SerializeField] float titleMagnifiPerc = 1.3f;
        [SerializeField] Image selectedImage;
        [SerializeField] GameObject reqItemObj;
        [SerializeField] UIItemSlot reqItemSlot;
        [SerializeField] Button activatedBtn;
        void UpdateData() {
            if(currentCategory > -1) {
                for(int i = 0; i < categories[currentCategory].content.childCount; i++) {
                    UITitleButton title = categories[currentCategory].content.GetChild(i).GetComponent<UITitleButton>();
                    title.image.color = isActive(title.id) ? Color.white : notActiveColor;
                }
            }
            if(current > -1) {

            }
        }
        void OnSelectTitle(int titleId) {
            if(ScriptableTitle.dict.TryGetValue(titleId, out ScriptableTitle title)) {
                current = titleId;
                selectedImage.sprite = title.GetImage();
                Rect rect = (selectedImage.transform as RectTransform).rect;
                rect.width = title.width * titleMagnifiPerc;
                rect.height = title.height * titleMagnifiPerc;
                lang.SetSuffix(1, ":" + GetActiveBonus(title));
                lang.SetSuffix(2, ":" + GetInActiveBonus(title));
                reqItemObj.SetActive(title.bookId > 0);
                if(title.bookId > 0 && ScriptableItem.dict.TryGetValue(title.bookId, out ScriptableItem itemData)) {
                    reqItemSlot.Assign(itemData.name);
                }
            }
            else UINotifications.list.Add("Title Not found please update or contact the developer", "هذا اللقب غير موجود برجاء التحديث او التواصل مع المطور");
        }
        string GetActiveBonus(ScriptableTitle title) {
            StringBuilder result = new StringBuilder();
            if(title.hp.active > 0) result.Append(LanguageManger.GetWord(6) + " +" + title.hp.active + ", ");
            if(title.mp.active > 0) result.Append(LanguageManger.GetWord(7) + " +" + title.mp.active + ", ");
            /*if(title.atk.active > 0)
                result.Append(LanguageManger.GetWord(player.classType == DamageType.Physical ? 8 : 9) + " +" + title.atk.active + ", ");
            if(title.def.active > 0)
                result.Append(LanguageManger.GetWord(player.classType == DamageType.Physical ? 10 : 11) + " +" + title.def.active + ", ");*/
            if(title.block.active > 0)
                result.Append(LanguageManger.GetWord(15) + " +" + title.block.active.ToString("F0") + "%, ");
            if(title.antiBlock.active > 0)
                result.Append(LanguageManger.GetWord(16) + " +" + title.antiBlock.active.ToString("F0") + "%, ");
            if(title.critRate.active > 0)
                result.Append(LanguageManger.GetWord(12) + " +" + title.critRate.active.ToString("F0") + "%, ");
            if(title.critDmg.active > 0)
                result.Append(LanguageManger.GetWord(13) + " +" + title.critDmg.active.ToString("F0") + "%, ");
            if(title.antiCrit.active > 0)
                result.Append(LanguageManger.GetWord(14) + " +" + title.antiCrit.active.ToString("F0") + "%, ");
            if(title.antiStun.active > 0)
                result.Append(LanguageManger.GetWord(18) + " +" + title.antiStun.active.ToString("F0") + "%, ");
            result.Remove(result.Length - 2, 2);
            return result.ToString();
        }
        string GetInActiveBonus(ScriptableTitle title) {
            StringBuilder result = new StringBuilder();
            if(title.hp.notActive > 0) result.Append(LanguageManger.GetWord(6) + " +" + title.hp.notActive + ", ");
            if(title.mp.notActive > 0) result.Append(LanguageManger.GetWord(7) + " +" + title.mp.notActive + ", ");
            /*if(title.atk.notActive > 0)
                result.Append(LanguageManger.GetWord(player.classType == DamageType.Physical ? 8 : 9) + " +" + title.atk.notActive + ", ");
            if(title.def.notActive > 0)
                result.Append(LanguageManger.GetWord(player.classType == DamageType.Physical ? 10 : 11) + " +" + title.def.notActive + ", ");*/
            if(title.block.notActive > 0)
                result.Append(LanguageManger.GetWord(15) + " +" + title.block.notActive.ToString("F0") + "%, ");
            if(title.antiBlock.notActive > 0)
                result.Append(LanguageManger.GetWord(16) + " +" + title.antiBlock.notActive.ToString("F0") + "%, ");
            if(title.critRate.notActive > 0)
                result.Append(LanguageManger.GetWord(12) + " +" + title.critRate.notActive.ToString("F0") + "%, ");
            if(title.critDmg.notActive > 0)
                result.Append(LanguageManger.GetWord(13) + " +" + title.critDmg.notActive.ToString("F0") + "%, ");
            if(title.antiCrit.notActive > 0)
                result.Append(LanguageManger.GetWord(14) + " +" + title.antiCrit.notActive.ToString("F0") + "%, ");
            if(title.antiStun.notActive > 0)
                result.Append(LanguageManger.GetWord(18) + " +" + title.antiStun.notActive.ToString("F0") + "%, ");
            result.Remove(result.Length - 2, 2);
            return result.ToString();
        }
        public void OnSelectCategory(int index) {
            currentCategory = currentCategory != index ? index : -1;
            for(int i = 0; i < categories.Count; i++)
                categories[i].content.gameObject.SetActive(i == currentCategory);
        }
        bool isActive(int titleId) {
            if(player.own.titles.Count > 0) {
                for(int i = 0; i < player.own.titles.Count; i++) {
                    if(player.own.titles[i] == titleId)
                        return true;
                }
            }
            return false;
        }
        void Awake() {
            List<TitleCaregory> categoryList = new List<TitleCaregory>();
            foreach(TitleCaregory category in Enum.GetValues(typeof(TitleCaregory)))
                categoryList.Add(category);
            UIUtils.BalancePrefabs(categoryPrefab, categoryList.Count, content);
            for(int i = 0; i < categoryList.Count; i++) {
                UICollapsableCategory catObj = content.GetChild(i).GetComponent<UICollapsableCategory>();
                int iCopy = i;
                lang.Add(new UITextObjectIdentifier {
                    obj = catObj.Name,
                    code = (int)categoryList[iCopy]
                });
                catObj.btn.onClick.SetListener(() => OnSelectCategory(iCopy));
                categories.Add(catObj);
                foreach(ScriptableTitle title in ScriptableTitle.dict.Values) {
                    if(title.category == categoryList[i]) {
                        UITitleButton button = Instantiate(btnPrefab, catObj.content, false).GetComponent<UITitleButton>();
                        button.Set(title.name, title.GetImage(), title.width, title.height);
                        button.btn.onClick.SetListener(() => OnSelectTitle(title.name));
                        if(current == -1)
                            OnSelectTitle(title.name);
                    }
                }
            }
        }
        void OnEnable() {
            if(player != null) {
                InvokeRepeating("UpdateData", 0, updateInterval);
            }
        }
        void OnDisable() {
            CancelInvoke("UpdateData");
        }

        /*void Update() {
            if(player) {
                if(player.own.titles.Count > 0) ShowTitlesHandler();
            } 
            else gameObject.SetActive(false);
        }
        void ShowTitlesHandler() {
            // update list
            foreach(TitleCaregory cat in (TitleCaregory[]) Enum.GetValues(typeof(TitleCaregory))) {
                UITitleCategoryCollapsable catSlot = Content.GetChild((int)cat).GetComponent<UITitleCategoryCollapsable>();
                List<ScriptableTitle> catList = AllOfCategory(cat);
                if(catSlot.isOpened) {
                    for(int i = 0; i < catList.Count; i++) {
                        UITitleButton slot = catSlot.content.GetChild(i).GetComponent<UITitleButton>();
                        slot.image.sprite = catList[i].image[(int)LanguageManger.current];
                        slot.notActive.SetActive(Convert.ToInt32(catList[i].name) == player.activeTitle);
                    }
                }
            }
        }
        void OnEnable() {
            // set categories and titles
            TitleCaregory[] tgArr = (TitleCaregory[]) Enum.GetValues(typeof(TitleCaregory));
            UIUtils.BalancePrefabs(titleCategoryPrefab.gameObject, tgArr.Length, Content);
            foreach(TitleCaregory cat in tgArr) {
                UITitleCategoryCollapsable catSlot = Content.GetChild((int)cat).GetComponent<UITitleCategoryCollapsable>();
                catSlot.header.text = cat.ToString();
                if((int)cat == 0) catSlot.ToggleOpened();
                // add all titles in this Category and add them in the collapsable
                List<ScriptableTitle> catList = AllOfCategory(cat);
                UIUtils.BalancePrefabs(titleBtnPrefab.gameObject, catList.Count, catSlot.content);
                for(int i = 0; i < catList.Count; i++) {
                    UITitleButton slot = catSlot.content.GetChild(i).GetComponent<UITitleButton>();
                    slot.image.sprite = catList[i].image[(int)LanguageManger.current];
                }
            }
        }
        List<ScriptableTitle> AllOfCategory(TitleCaregory cat) {
            List<ScriptableTitle> result = new List<ScriptableTitle>();
            for(int i = 0; i < ScriptableTitle.dict.Count; i++) {
                if(ScriptableTitle.dict[i].category == cat) 
                    result.Add(ScriptableTitle.dict[i]);
            }
            return result;
        }
        void Start() {
            /*activatedBtn.onClick.SetListener(() => caregory = -1);
            for(int i = 0; i < navs.Length; i++) {
                int iCopy = i;
                navs[i].onClick.SetListener(() => caregory = iCopy);
                selectedTitle = -1;
            }
        }*/
    }
}