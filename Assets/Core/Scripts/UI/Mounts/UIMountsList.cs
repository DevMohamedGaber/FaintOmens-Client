using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIMountsList : MonoBehaviour {
        [SerializeField] UIMounts window;
        [SerializeField] GameObject prefab;
        [SerializeField] Transform content;
        [SerializeField] GameObject summonBtn;
        [SerializeField] GameObject unsummonBtn;
        [SerializeField] Image typeImg;
        [SerializeField] Image tierImg;
        [SerializeField] RTLTMPro.RTLTextMeshPro nameTxt;
        [SerializeField] TMPro.TMP_Text brTxt;
        public int selected = 0;
        public UIMountButton[] list;
        [HideInInspector] public UIMounts _window => window;
        public ushort sId => list[selected].id;
        Player player => Player.localPlayer;
        public void Select(int index) {
            selected = index;
            window.Status();
            if(ScriptableMount.dict.TryGetValue(sId, out ScriptableMount mount)) {
                UIPreviewManager.singleton.Instantiate(mount.prefab);
                Mount? info = player.own.mounts.Get(sId);

                summonBtn.SetActive(info != null && info.Value.status == SummonableStatus.Saved);
                unsummonBtn.SetActive(info != null && info.Value.status == SummonableStatus.Deployed);

                nameTxt.text = mount.Name;
                typeImg.sprite = Storage.data.classTypeIcons[(int)mount.classType];

                if(info != null) {
                    brTxt.text = info.Value.battlepower.ToString();
                    nameTxt.color = UIManager.data.assets.tierColor[(int)info.Value.tier];
                    tierImg.sprite = UIManager.data.assets.tiers[(int)info.Value.tier];
                }
                else {
                    brTxt.text = mount.brMax.ToString();
                    nameTxt.color = UIManager.data.assets.tierColor[(int)mount.maxTier];
                    tierImg.sprite = UIManager.data.assets.tiers[(int)mount.maxTier];
                }
            }
        }
        public void SelectId(ushort mId) {
            for(int i = 0; i < list.Length; i++) {
                if(list[i].id == mId) {
                    Select(i);
                    return;
                }
            }
        }
        public void UpdateMount(Mount mount) {
            if(list.Length < 1)
                return;
            for(int i = 0; i < list.Length; i++) {
                if(list[i].id == mount.id) {
                    list[i].SetActiveData(mount);
                    break;
                }
            }
        }
        public void OnDeploy() {
            int pIndex = player.own.mounts.Has(sId);
            if(pIndex == -1) {
                Notify.list.Add("mount isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.own.mounts[pIndex].status == SummonableStatus.Deployed) {
                Notify.list.Add("mount is already deployer", "المرافق مستدعي بالفعل");
                return;
            }
            player.CmdMountDeploy(sId);
        }
        public void OnRecall() {
            if(!player.mount.canMount) {
                Notify.list.Add("No active mount found", "لا يوجد مرافق مستدعي");
                return;
            }
            int pIndex = player.own.mounts.Has(sId);
            if(pIndex == -1) {
                Notify.list.Add("mount isn't active", "المرافق غير مفعل");
                return;
            }
            if(player.mount.id != sId) {
                Notify.list.Add("This isn't the deployed mount", "هذا ليس الراكب المفعل");
                return;
            }
            if(player.own.mounts[pIndex].status == SummonableStatus.Saved) {
                Notify.list.Add("mount is already called", "المرافق غير مستدعي بالفعل");
                return;
            }
            player.CmdMountRecall();
        }
        void OnEnable() {
            list = new UIMountButton[ScriptableMount.dict.Count];
            UIUtils.BalancePrefabs(prefab, ScriptableMount.dict.Count, content);
            int i = 0;
            foreach(ScriptableMount mount in ScriptableMount.dict.Values) {
                UIMountButton btn = content.GetChild(i).GetComponent<UIMountButton>();
                int iCopy = i;
                btn.Set(mount);
                btn.button.onClick = () => Select(iCopy);
                list[i] = btn;
            }
            if(player.own.mounts.Count > 0) {
                for(int m = 0; m < player.own.mounts.Count; m++) {
                    for(int b = 0; b < list.Length; b++) {
                        if(list[b].id == player.own.mounts[m].id) {
                            list[b].SetActiveData(player.own.mounts[m]);
                            break;
                        }
                    }
                }
            }
            Select(0);
        }
        void OnDisable() {
            list = new UIMountButton[0];
            UIUtils.DestroyChildren(content);
        }
    }
}