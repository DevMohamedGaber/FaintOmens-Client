using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class MountsList : MonoBehaviour
    {
        [SerializeField] Mounts _window;
        [SerializeField] GameObject prefab;
        [SerializeField] Transform content;
        [SerializeField] GameObject summonBtn;
        [SerializeField] GameObject unsummonBtn;
        [SerializeField] Image typeImg;
        [SerializeField] Image tierImg;
        [SerializeField] TMPro.TMP_Text brTxt;
        [SerializeField] UIToggleGroup toggleGroup;
        public int selected = 0;
        public MountButton[] list;
        public Mounts window => _window;
        public ushort sId => list[selected].id;
        Player player => Player.localPlayer;
        public void Select(int index)
        {
            selected = index;
            if(ScriptableMount.dict.TryGetValue(sId, out ScriptableMount data))
            {
                PreviewManager.singleton.Instantiate(data.prefab);
                Mount info = player.own.mounts.Get(sId);

                summonBtn.SetActive(info.id != 0 && info.status == SummonableStatus.Saved);
                unsummonBtn.SetActive(info.id != 0 && info.status == SummonableStatus.Deployed);

                typeImg.sprite = UIManager.data.assets.classTypeIcons[(int)data.classType];

                if(info.id != 0)
                {
                    brTxt.text = info.battlepower.ToString();
                    tierImg.sprite = UIManager.data.assets.tiers[(int)info.tier];
                }
                else
                {
                    brTxt.text = data.brMax.ToString();
                    tierImg.sprite = UIManager.data.assets.tiers[(int)data.maxTier];
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
        public void UpdateMount(Mount mount)
        {
            if(list.Length < 1)
                return;
            for(int i = 0; i < list.Length; i++)
            {
                if(list[i].id == mount.id)
                {
                    list[i].SetActiveData(mount);
                    break;
                }
            }
        }
        public void OnSummon()
        {
            int pIndex = player.own.mounts.Has(sId);
            if(pIndex == -1)
            {
                Notify.list.Add("Mounts isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.mounts[pIndex].status == SummonableStatus.Deployed)
            {
                Notify.list.Add("Mounts is already deployer", "المرافق مستدعي بالفعل");
                return;
            }
            player.CmdMountSummon();
        }
        public void OnUnsummon()
        {
            if(!player.mount.canMount)
            {
                Notify.list.Add("No active Mount found", "لا يوجد مرافق مستدعي");
                return;
            }
            int pIndex = player.own.mounts.Has(sId);
            if(pIndex == -1)
            {
                Notify.list.Add("Mounts isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.mounts[pIndex].status == SummonableStatus.Saved)
            {
                Notify.list.Add("Mounts is already called", "المرافق غير مستدعي بالفعل");
                return;
            }
            player.CmdMountUnsummon();
        }
        void OnEnable()
        {
            if(player.own.mounts.Count > 0)
            {
                for(int i = 0; i < player.own.mounts.Count; i++)
                {
                    for(int j = 0; j < list.Length; j++)
                    {
                        if(list[j].id == player.own.mounts[i].id)
                        {
                            list[j].SetActiveData(player.own.mounts[i]);
                            break;
                        }
                    }
                }
            }
            Select(0);
        }
        void Awake()
        {
            list = new MountButton[ScriptableMount.dict.Count];
            if(ScriptableMount.dict.Count > 0)
            {
                UIUtils.BalancePrefabs(prefab, ScriptableMount.dict.Count, content);
                for (int i = 0; i < ScriptableMount.dict.Count; i++)
                {
                    MountButton btn = content.GetChild(i).GetComponent<MountButton>();
                    if(btn != null)
                    {
                        btn.Set(ScriptableMount.dict[i]);
                        btn.toggle.onSelect = () => Select(i);
                        list[i] = btn;
                    }
                }
                toggleGroup.UpdateTogglesList();
            }
        }
    }
}