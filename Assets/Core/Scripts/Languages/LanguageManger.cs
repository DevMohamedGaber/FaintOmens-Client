using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Game
{
    public class LanguageManger
    {
        public static Languages defLang = Languages.En;
        public static string[] langNames = new string[] { "العربية", "English" };
        public static bool isEn => UIManager.data.gameSettings.currentLang == Languages.En;
        public static Languages current => UIManager.data.gameSettings.currentLang;
        public static Dictionary<LanguageDictionaryCategories, Dictionary<int, string>> dictionary = new Dictionary<LanguageDictionaryCategories, Dictionary<int, string>>();

        public static void Load(Languages lang)
        {
            //UIManager.data.miniLoading.Show(2);
            UIManager.data.OnChangeLanguage(lang);
            ScriptableLanguage resource = Resources.Load<ScriptableLanguage>("Languages/" + lang.ToString());
            //Declare();
            if(resource != null)
            {
                resource.Fetch();
            }
            Refresh();
            //UIManager.data.miniLoading.Hide();
        }
        public static void LoadDefault()
        {
            Load(defLang);
        }
        static async Task Refresh()
        {
            Task.Run(() => {
                UI.UILanguageDefiner[] objs = Resources.FindObjectsOfTypeAll<UI.UILanguageDefiner>();
                if(objs.Length < 1)
                    return;
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i].Refresh();
                }
            });
        }
        public static string GetWord(int code, LanguageDictionaryCategories category = LanguageDictionaryCategories.Word)
        {
            if(dictionary.TryGetValue(category, out Dictionary<int, string> words))
            {
                if(words.TryGetValue(code, out string word))
                {
                    return word;
                }
            }
            return "";
        }
        public static string GetItem(int itemId)
        {
            return GetWord(itemId, LanguageDictionaryCategories.ItemName);
        }
        public static string GetClassName(int classId)
        {
            return GetWord(classId, LanguageDictionaryCategories.Class);
        }
        public static string Decide(string enTxt, string arTxt)
        {
            return isEn ? enTxt : arTxt;
        }
        public static string UseSymbols(string text, string leftSymbol, string rightSymbol) 
        {
            return isEn ? leftSymbol + text + rightSymbol : rightSymbol + text + leftSymbol;
        }
        /*static void Declare()
        {
            LanguageDictionaryCategories[] categoryList = (LanguageDictionaryCategories[]) Enum.GetValues(typeof(LanguageDictionaryCategories));
            if(categoryList.Length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    
                }
            }

            dictionary[LanguageDictionaryCategories.Word] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.ItemName] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.ItemDesc] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.ItemTypes] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.Monster] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.Pet] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.Mount] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.City] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.Class] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.Tribe] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.MilitaryRank] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.Wardrop] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.AchievementName] = new Dictionary<int, string>();
            dictionary[LanguageDictionaryCategories.AchievementRequirement] = new Dictionary<int, string>();
        }*/
    }
}