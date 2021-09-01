using UnityEngine;
using UnityEngine.UI;
using Game.Network.Messages;
namespace Game.UI
{
    public class UIPreviewManager : MonoBehaviour {
        public static UIPreviewManager singleton;
        [SerializeField] GameObject playerModel;
        [SerializeField] Transform holder;
        [SerializeField] GameObject cam;
        public GameObject go;
        
        public void Instantiate(GameObject perfab) {
            Clear();
            go = GameObject.Instantiate(perfab, holder, false);
            go.name = perfab.name;
            Animator anim = go.GetComponent<Animator>();
            if(anim != null)
                anim.enabled = true;
            Collider collider = go.GetComponentInChildren<Collider>();
            if(collider != null)
                SetCamera(collider.bounds.center.y);
        }
        public void InstantiatePlayerFromSelectionMsg(CharactersAvailable.CharacterPreview data) {
            Clear();
            go = GameObject.Instantiate(playerModel, holder, false);
            PlayerPreviewModel player = go.GetComponent<PlayerPreviewModel>();
            player.gender = data.gender;
            player.classInfo = data.classInfo;
            player.showWardrobe = data.showWardrobe;

            player.equipment = data.equipment;
            player.wardrobe = data.wardrobe;
            player.RefreshAllLocation();

            SetCamera(player.collider.bounds.center.y);
        }
        // for normal player Instantiation (ex. LocalPlayer)
        public void InstantiatePlayer(Player data) {
            Clear();
            go = GameObject.Instantiate(playerModel, holder, false);
            UpdatePlayer(data);
        }
        public void UpdatePlayer(Player data) {
            if(go != null) {
                PlayerPreviewModel player = go.GetComponent<PlayerPreviewModel>();
                if(player != null) {
                    player.gender = data.gender;
                    player.classInfo = data.classInfo;
                    player.showWardrobe = data.showWardrop;

                    // equipments
                    player.equipment[0].Add(data.equipment[(int)EquipmentsCategory.Armor]);
                    player.equipment[1].Add(data.equipment[(int)EquipmentsCategory.Weapon]);
                    // wardrobe
                    for(int i = 0; i < player.wardrobe.Length; i++)
                        player.wardrobe[i] = data.wardrobe[i].id;
                    
                    player.RefreshAllLocation();
                    SetCamera(player.collider.bounds.center.y);
                }
            }
        }
        public void InstantiateLocalPlayer() => InstantiatePlayer(Player.localPlayer);
        public void UpdateLocalPlayer() => UpdatePlayer(Player.localPlayer);
        public void InstantiatePlayer(PlayerClass classType, Gender gender, SyncListItemSlot equipments) {
            Clear();
            go = GameObject.Instantiate(ScriptableClass.dict[classType].prefab, holder, false);
            Player player = go.GetComponent<Player>();
            player.gender = gender;
            player.equipment = equipments;
            player.RefreshAllLocation();
            SetCamera(player.collider.bounds.center.y);
        }
        // for Preview player Instantiation
        public void InstantiatePlayer(PlayerClass classType, Gender gender, Item[] equipments) {
            Clear();
            go = GameObject.Instantiate(ScriptableClass.dict[classType].prefab, holder, false);
            Player player = go.GetComponent<Player>();
            player.gender = gender;
            player.equipment = new SyncListItemSlot();
            for(int i = 0; i < equipments.Length; i++)
                player.equipment.Add(new ItemSlot(equipments[i], 1));
            player.RefreshAllLocation();
            SetCamera(player.collider.bounds.center.y);
        }
        public void InstantiatePlayer(PlayerClass classType, Gender gender, ItemSlot[] equipments) {
            Clear();
            go = GameObject.Instantiate(ScriptableClass.dict[classType].prefab, holder, false);
            Player player = go.GetComponent<Player>();
            player.gender = gender;
            for(int i = 0; i < equipments.Length; i++)
                player.equipment.Add(equipments[i]);
            for(int i = 0; i < 4; i++) {
                player.wardrobe.Add(new WardrobeItem());
            }
            player.RefreshAllLocation();
            SetCamera(player.collider.bounds.center.y);
        }
        void SetCamera(float y) {
            cam.transform.position = new Vector3(cam.transform.position.x, y, cam.transform.position.z);
            if(cam != null)
                cam.SetActive(true);
        }
        public void Clear() {
            if(cam != null)
                cam.SetActive(false);
            if(holder != null && go != null)
                Destroy(go);
        }
        public void Rotate(float rotation) {
            if(go != null) {
                float y = go.transform.eulerAngles.y;
                go.transform.rotation = Quaternion.Euler(0, y + rotation, 0);
            }
        }
        void Awake() {
            if(singleton == null)
                singleton = this;
        }
    }
}