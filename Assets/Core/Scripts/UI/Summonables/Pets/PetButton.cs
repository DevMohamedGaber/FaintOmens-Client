using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using TMPro;
namespace Game.UI
{
    public class PetButton : MonoBehaviour
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
        ScriptablePet data;
        public void Set(ScriptablePet data)
        {
            this.data = data;
            avatar.sprite = data.avatar;
            nameTxt.text = data.Name;
        }
        public void SetActiveData(PetInfo info)
        {
            deployed.SetActive(info.status == SummonableStatus.Deployed);
            nameTxt.text = $"{data.Name}\n<color=orange>{LanguageManger.Decide("Lvl.", "مستوي ")} {(int)info.level}</color>";
        }
    }
}