using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class UIMountButton : MonoBehaviour {
        public Image avatar;
        public Image tire;
        public RTLTextMeshPro nameTxt;
        public Image[] stars;
        public TMP_Text lvlTxt;
        public GameObject starsObj;
        public GameObject assigned;
        public GameObject selected;
        public BasicButton button;
        public ushort id;
        public void Set(ScriptableMount data) {
            id = (ushort)data.name;
            nameTxt.text = data.Name;
            avatar.sprite = data.avatar;
            tire.sprite = UIManager.data.assets.tiers[(int)data.maxTier];
        }
        public void SetActiveData(Mount data) {
            tire.sprite = UIManager.data.assets.tiers[(int)data.tier];
            //assigned.SetActive(data.status == SummonableStatus.Deployed);

            lvlTxt.gameObject.SetActive(true);
            lvlTxt.text = data.level.ToString();

            //starsObj.SetActive(true);
            //for(int i = 0; i < Storage.data.pet.starsCap; i++)
            //    stars[i].sprite = UIManager.data.assets.stars[i < data.stars ? 1 : 0];
        }
    }
}