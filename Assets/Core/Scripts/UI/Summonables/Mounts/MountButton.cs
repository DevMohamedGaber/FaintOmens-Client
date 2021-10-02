using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class MountButton : MonoBehaviour
    {
        [SerializeField] Image avatar;
        [SerializeField] RTLTextMeshPro nameTxt;
        [SerializeField] GameObject deployed;
        public UIToggle toggle;
        public ushort id
        {
            get
            {
                return (ushort)data.name;
            }
        }
        ScriptableMount data;
        public void Set(ScriptableMount data)
        {
            this.data = data;
            avatar.sprite = data.avatar;
            nameTxt.text = data.Name;
        }
        public void SetActiveData(Mount info)
        {
            deployed.SetActive(info.status == SummonableStatus.Deployed);
            nameTxt.text = $"{data.Name}\n<color=orange>{LanguageManger.Decide("Lvl.", "مستوي ")} {(int)info.level}</color>";
        }
    }
}