using UnityEngine;
using System.Collections.Generic;
namespace Game.UI
{
    public class UIWardobeFashions : MonoBehaviour {
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        Player player => Player.localPlayer;
        public void ChangeChategory(int selectedCat) {
            List<ScriptableWardrobe> list = ScriptableWardrobe.Get((ClothingCategory)selectedCat);
            UIUtils.BalancePrefabs(prefab, list.Count, content);
            if(list.Count > 0) {
                for(int i = 0; i < list.Count; i++)
                    content.GetChild(i).GetComponent<UIWardrobeFashionItem>().Set((ushort)list[i].name);
            }
        }
        void OnEnable() {
            ChangeChategory(0);
        }
        void OnDisable() {
            if(content.childCount > 0) {
                for(int i = 0; i < content.childCount; i++)
                    Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}