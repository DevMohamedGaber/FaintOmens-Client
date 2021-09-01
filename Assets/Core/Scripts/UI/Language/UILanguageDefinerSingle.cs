using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UILanguageDefinerSingle : MonoBehaviour {
        [SerializeField] RTLTMPro.RTLTextMeshPro obj;
        public LanguageDictionaryCategories category;
        public int code;
        public string prefix;
        public string suffix;
        [SerializeField] bool onEnable = true;
        public void Refresh() {
            if(obj == null)
                return;
            if(code > -1) {
                if(prefix != "")
                    obj.text += prefix + " ";
                obj.text = LanguageManger.GetWord(code, category);
                if(suffix != "")
                    obj.text += " " + suffix;
            }
            else obj.text = prefix + suffix;
        }
        public void Set(int code, string prefix = "", string suffix = "") {
            this.code = code;
            this.prefix = prefix;
            this.suffix = suffix;
            Refresh();
        }
        public void SetSuffix(string suffix) {
            this.suffix = suffix;
            Refresh();
        }
        void OnEnable() {
            if(onEnable) Refresh();
        }
        void Awake() {
            Refresh();
        }
    }
}