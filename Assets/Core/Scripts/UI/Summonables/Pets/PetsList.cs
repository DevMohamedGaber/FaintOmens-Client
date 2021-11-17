using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class PetsList : MonoBehaviour
    {
        [SerializeField] Pets _window;
        [SerializeField] GameObject prefab;
        [SerializeField] Transform content;
        [SerializeField] GameObject summonBtn;
        [SerializeField] GameObject unsummonBtn;
        [SerializeField] Image typeImg;
        [SerializeField] Image tierImg;
        //[SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMPro.TMP_Text brTxt;
        [SerializeField] UIToggleGroup toggleGroup;
        public int selected = 0;
        public PetButton[] list;
        public Pets window => _window;
        public ushort sId => list[selected].id;
        Player player => Player.localPlayer;
        public void Select(int index)
        {
            selected = index;
            if(ScriptablePet.dict.TryGetValue(sId, out ScriptablePet pet))
            {
                PreviewManager.singleton.Instantiate(pet.prefab);
                PetInfo info = player.own.pets.Get(sId);

                summonBtn.SetActive(info.id != 0 && info.status == SummonableStatus.Saved);
                unsummonBtn.SetActive(info.id != 0 && info.status == SummonableStatus.Deployed);

                //nameTxt.text = pet.Name;
                typeImg.sprite = UIManager.data.assets.classTypeIcons[(int)pet.classType];

                if(info.id != 0)
                {
                    brTxt.text = info.battlepower.ToString();
                    //nameTxt.color = UIManager.data.assets.tierColor[(int)info.Value.tier];
                    tierImg.sprite = UIManager.data.assets.tiers[(int)info.tier];
                }
                else
                {
                    brTxt.text = pet.brMax.ToString();
                    //nameTxt.color = UIManager.data.assets.tierColor[(int)pet.maxTier];
                    tierImg.sprite = UIManager.data.assets.tiers[(int)pet.maxTier];
                }
            }
            _window.Status();
        }
        public void SelectId(ushort pId)
        {
            for(int i = 0; i < list.Length; i++)
            {
                if(list[i].id == pId)
                {
                    Select(i);
                    return;
                }
            }
        }
        public void UpdatePet(PetInfo pet)
        {
            if(list.Length < 1)
                return;
            for(int i = 0; i < list.Length; i++)
            {
                if(list[i].id == pet.id)
                {
                    list[i].SetActiveData(pet);
                    break;
                }
            }
        }
        public void OnSummon()
        {
            int pIndex = player.own.pets.Has(sId);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].status == SummonableStatus.Deployed)
            {
                Notify.list.Add("Pet is already deployer", "المرافق مستدعي بالفعل");
                return;
            }
            player.CmdPetSummon(sId);
        }
        public void OnUnsummon()
        {
            if(player.activePet == null)
            {
                Notify.list.Add("No active pet found", "لا يوجد مرافق مستدعي");
                return;
            }
            int pIndex = player.own.pets.Has(sId);
            if(pIndex == -1)
            {
                Notify.list.Add("Pet isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.pets[pIndex].status == SummonableStatus.Saved)
            {
                Notify.list.Add("Pet is already called", "المرافق غير مستدعي بالفعل");
                return;
            }
            player.CmdPetUnsummon();
        }
        void OnEnable()
        {
            if(player.own.pets.Count > 0)
            {
                for(int i = 0; i < player.own.pets.Count; i++)
                {
                    for(int j = 0; j < list.Length; j++)
                    {
                        if(list[j].id == player.own.pets[i].id)
                        {
                            list[j].SetActiveData(player.own.pets[i]);
                            break;
                        }
                    }
                }
            }
            Select(0);
        }
        void Awake()
        {
            list = new PetButton[ScriptablePet.dict.Count];
            if(ScriptablePet.dict.Count > 0)
            {
                UIUtils.BalancePrefabs(prefab, ScriptablePet.dict.Count, content);
                for (int i = 0; i < ScriptablePet.dict.Count; i++)
                {
                    PetButton btn = content.GetChild(i).GetComponent<PetButton>();
                    if(btn != null)
                    {
                        btn.Set(ScriptablePet.dict[i]);
                        btn.toggle.onSelect = () => Select(i);
                        list[i] = btn;
                    }
                }
                toggleGroup.UpdateTogglesList();
            }
        }
    }
}