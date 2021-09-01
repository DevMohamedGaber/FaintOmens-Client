using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIChooseLanguage : MonoBehaviour {
        [SerializeField] Transform content;
        [SerializeField] UIChooseLanguageBtn prefab;
        public void Show() {
            int length = LanguageManger.langNames.Length;
            UIUtils.BalancePrefabs(prefab.gameObject, length, content);
            for (int i = 0; i < length; i++) {
                UIChooseLanguageBtn lang = content.GetChild(i).GetComponent<UIChooseLanguageBtn>();
                lang.Name.text = LanguageManger.langNames[i];
                int iCopy = i;
                lang.button.onClick.SetListener(() => {
                    LanguageManger.Load((Languages)iCopy);
                    Invoke(nameof(Next), 1f);
                });
            }
            gameObject.SetActive(true);
        }
        void Next() {
            gameObject.SetActive(false);
            UIManager.data.gameUpdater.StartPatcher();
        }
    }
}