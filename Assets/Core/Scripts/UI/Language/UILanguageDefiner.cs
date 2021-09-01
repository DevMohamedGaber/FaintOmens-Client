using System;
using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
namespace Game.UI
{
    public class UILanguageDefiner : MonoBehaviour {
        [SerializeField] UITextObjectIdentifier[] items;
        public bool refreshOnEnable = true;

        public void Refresh() {
            if(!gameObject.activeSelf || items.Length < 1)
                return;

            for(int i = 0; i < items.Length; i++) {
                if(items[i].obj == null)
                    continue;
                if(items[i].code > -1) {
                    if(items[i].prefix != "")
                        items[i].obj.text += items[i].prefix + " ";
                    items[i].obj.text = LanguageManger.GetWord(items[i].code, items[i].category);
                    if(items[i].suffix != "")
                        items[i].obj.text += " " + items[i].suffix;
                }
                else items[i].obj.text = items[i].prefix + items[i].suffix;
            }
        }
        public void RefreshAt(int i) {
            if(i < 0 || i > items.Length || items[i].obj == null)
                return;
            if(items[i].code > -1) {
                if(items[i].prefix != "")
                    items[i].obj.text += items[i].prefix + " ";
                items[i].obj.text = LanguageManger.GetWord(items[i].code, items[i].category);
                if(items[i].suffix != "")
                    items[i].obj.text += " " + items[i].suffix;
            }
            else items[i].obj.text = items[i].prefix + items[i].suffix;
        }
        public void RefreshRange(int start, int end) {
            if(start < 0 || end > items.Length) return;
            for (int i = start; i <= end; i++)
                RefreshAt(i);
        }
        public int Add(UITextObjectIdentifier item) {
            Array.Resize(ref items, items.Length + 1);
            items[items.Length - 1] = item;
            return items.Length - 1;
        }
        public void SetCode(int index, int code) {
            if(index < items.Length)
                items[index].code = code;
        }
        public void SetPrefix(int index, string prefix) {
            if(index < items.Length)
                items[index].prefix = prefix;
        }
        public void SetSuffix(int index, string suffix) {
            if(index < items.Length)
                items[index].suffix = suffix;
        }
        public void SetTextColor(int index, Color color) {
            if(index < items.Length)
                items[index].obj.color = color;
        }
        public void SetCategory(int index, LanguageDictionaryCategories category) {
            if(index < items.Length)
                items[index].category = category;
        }
        public void SetActive(int index, bool value) {
            if(index < items.Length)
                items[index].obj.gameObject.SetActive(value);
        }

        void OnEnable() {
            if(refreshOnEnable)
                Refresh();
        }
    }
    [Serializable]
    public struct UITextObjectIdentifier {
        public RTLTextMeshPro obj;
        public LanguageDictionaryCategories category;
        public int code;
        public string prefix;
        public string suffix;
    }
}