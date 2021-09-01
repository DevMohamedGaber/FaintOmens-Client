using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIPlayerPreview : MonoBehaviour {
        [SerializeField] UILanguageDefiner lang;
        [SerializeField] Image flag;
        [SerializeField] Image classIcon;
        [SerializeField] Image militaryIcon;
        [SerializeField] GameObject equipSlotsObj;
        [SerializeField] GameObject equipBtn;
        [SerializeField] UIEquipmentSlot[] equipSlots;
        [SerializeField] GameObject accSlotsObj;
        [SerializeField] GameObject accBtn;
        [SerializeField] UIEquipmentSlot[] acctSlots;
        public void Show(PreviewPlayerData info) {
            gameObject.SetActive(false);// just in case it's visibule
            //on character info
            flag.sprite = ScriptableTribe.dict[info.tribeId].flag;
            classIcon.sprite = info.classInfo.data.icon;
            militaryIcon.sprite = ScriptableMilitaryRank.dict[info.militaryRank].icon;
            lang.SetSuffix(1, info.name);
            lang.SetSuffix(2, info.br.ToString());
            //info
            lang.SetSuffix(3, $":</color> {info.id}");
            lang.SetSuffix(4, $":</color> {info.vipLevel}");
            lang.SetSuffix(5, $":</color> {info.level}");
            lang.SetSuffix(6, $":</color> {info.classInfo.Name}");
            lang.SetSuffix(7, $":</color> {LanguageManger.GetWord(info.tribeId, LanguageDictionaryCategories.Tribe)}");
            lang.SetSuffix(8, $":</color> {(info.guildName == "" ? "-" : info.guildName)}");
            lang.SetSuffix(9, $":</color> {LanguageManger.GetWord(info.militaryRank, LanguageDictionaryCategories.MilitaryRank)}");
            //attributes
            lang.SetSuffix(10, $":</color> {info.health}");
            lang.SetSuffix(11, $":</color> {info.mana}");
            lang.SetSuffix(12, $":</color> {info.pAtk}");
            lang.SetSuffix(13, $":</color> {info.pDef}");
            lang.SetSuffix(14, $":</color> {info.mAtk}");
            lang.SetSuffix(15, $":</color> {info.mDef}");
            lang.SetSuffix(16, $":</color> {info.block.ToString("F0")}%");
            lang.SetSuffix(17, $":</color> {info.untiBlock.ToString("F0")}%");
            lang.SetSuffix(18, $":</color> {info.critRate.ToString("F0")}%");
            lang.SetSuffix(19, $":</color> {info.critDmg.ToString("F0")}%");
            lang.SetSuffix(20, $":</color> {info.antiCrit.ToString("F0")}%");
            lang.SetSuffix(21, $":</color> {info.speed.ToString("F0")}%");
            //equipments
            for(int i = 0; i < info.equipments.Length; i++)
                if(info.equipments[i].id != 0) equipSlots[i].Assign(info.equipments[i]);
                else equipSlots[i].Unassign();
            //acc
            for(int i = 0; i < info.accessories.Length; i++)
                if(info.accessories[i].id != 0) equipSlots[i].Assign(info.accessories[i]);
                else equipSlots[i].Unassign();

            lang.Refresh();
            UIPreviewManager.singleton.InstantiatePlayer(info.classInfo.type, info.gender, info.equipments);
            gameObject.SetActive(true);
        }
        public void ShowEquipments(bool isEquip) {
            accSlotsObj.SetActive(!isEquip);
            accBtn.SetActive(isEquip);

            equipSlotsObj.SetActive(isEquip);
            equipBtn.SetActive(!isEquip);
        }
    }
}