using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIPetsList : MonoBehaviour {
        [SerializeField] UIPets window;
        [SerializeField] GameObject prefab;
        [SerializeField] Transform content;
        [SerializeField] GameObject summonBtn;
        [SerializeField] GameObject unsummonBtn;
        [SerializeField] Image typeImg;
        [SerializeField] Image tierImg;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMPro.TMP_Text brTxt;
        public int selected = 0;
        public UIPetButton[] list;
        [HideInInspector] public UIPets _window => window;
        public ushort sId => list[selected].id;
        Player player => Player.localPlayer;
        public void Select(int index) {
            selected = index;
            window.Status();
            if(ScriptablePet.dict.TryGetValue(sId, out ScriptablePet pet)) {
                UIPreviewManager.singleton.Instantiate(pet.prefab);
                PetInfo? info = player.own.pets.Get(sId);

                summonBtn.SetActive(info != null && info.Value.status == SummonableStatus.Saved);
                unsummonBtn.SetActive(info != null && info.Value.status == SummonableStatus.Deployed);

                nameTxt.text = pet.Name;
                typeImg.sprite = Storage.data.classTypeIcons[(int)pet.classType];

                if(info != null) {
                    brTxt.text = info.Value.battlepower.ToString();
                    nameTxt.color = UIManager.data.assets.tierColor[(int)info.Value.tier];
                    tierImg.sprite = UIManager.data.assets.tiers[(int)info.Value.tier];
                }
                else {
                    brTxt.text = pet.brMax.ToString();
                    nameTxt.color = UIManager.data.assets.tierColor[(int)pet.maxTier];
                    tierImg.sprite = UIManager.data.assets.tiers[(int)pet.maxTier];
                }
            }
        }
        public void SelectId(ushort pId) {
            for(int i = 0; i < list.Length; i++) {
                if(list[i].id == pId) {
                    Select(i);
                    return;
                }
            }
        }
        public void UpdatePet(PetInfo pet) {
            if(list.Length < 1)
                return;
            for(int i = 0; i < list.Length; i++) {
                if(list[i].id == pet.id) {
                    list[i].SetActiveData(pet);
                    break;
                }
            }
        }
        public void OnSummon() {
            int pIndex = player.own.pets.Has(sId);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].status == SummonableStatus.Deployed) {
                Notify.list.Add("Pet is already deployer", "المرافق مستدعي بالفعل");
                return;
            }
            player.CmdPetSummon(sId);
        }
        public void OnUnsummon() {
            if(player.activePet == null) {
                Notify.list.Add("No active pet found", "لا يوجد مرافق مستدعي");
                return;
            }
            int pIndex = player.own.pets.Has(sId);
            if(pIndex == -1) {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].status == SummonableStatus.Saved) {
                Notify.list.Add("Pet is already called", "المرافق غير مستدعي بالفعل");
                return;
            }
            player.CmdPetUnsummon();
        }
        void OnEnable() {
            list = new UIPetButton[ScriptablePet.dict.Count];
            UIUtils.BalancePrefabs(prefab, ScriptablePet.dict.Count, content);
            int i = 0;
            foreach(ScriptablePet pet in ScriptablePet.dict.Values) {
                UIPetButton btn = content.GetChild(i).GetComponent<UIPetButton>();
                int iCopy = i;
                btn.Set(pet);
                btn.button.onClick = () => Select(iCopy);
                list[i] = btn;
            }
            if(player.own.pets.Count > 0) {
                for(int p = 0; p < player.own.pets.Count; p++) {
                    for(int b = 0; b < list.Length; b++) {
                        if(list[b].id == player.own.pets[p].id) {
                            list[b].SetActiveData(player.own.pets[p]);
                            break;
                        }
                    }
                }
            }
            Select(0);
        }
        void OnDisable() {
            list = new UIPetButton[0];
            UIUtils.DestroyChildren(content);
        }
    }
}