using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Game.Components;
namespace Game
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))] // kinematic, only needed for OnTrigger
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerPreviewModel : MonoBehaviour
    {
        public Animator animator;
        public new Collider collider;
        public PlayerClassData classInfo;
        public Gender gender;
        public bool showWardrobe;
        public EquipmentPreview[] equipment = new EquipmentPreview[2];
        public ushort[] wardrobe = new ushort[4];
        public Transform effectMount;
        [SerializeField] Transform bodyHolder;
        [SerializeField] Transform rWeaponHolder;
        [SerializeField] Transform lWeaponHolder;
        [SerializeField] Transform wingsHolder;
        [SerializeField] Transform spiritHolder;
        Dictionary<string, Transform> skinBones = new Dictionary<string, Transform>();

        void RefreshBodyLocation()
        {
            if(bodyHolder == null)
                return;
            int index = (int)ClothingCategory.Body;
            if(wardrobe[index] > 0 && showWardrobe)
            {
                if(bodyHolder.childCount > 0)
                {
                    Destroy(bodyHolder.GetChild(0).gameObject);
                }
                GameObject go = Instantiate(ScriptableWardrobe.dict[wardrobe[index]].modelPrefab[(int)gender], bodyHolder, false);
                OnRefreshBodyLocation(go);
            }
            else
            {
                EquipmentPreview equipmentSlot = equipment[0];
                if(bodyHolder.childCount > 0)
                {
                    Destroy(bodyHolder.GetChild(0).gameObject);
                }
                if(equipment[0].id > 0)
                {
                    EquipmentItem itemData = equipment[0].data;
                    if(itemData != null && itemData.modelPrefab[(int)gender] != null)
                    {
                        GameObject go = Instantiate(itemData.modelPrefab[(int)gender], bodyHolder, false);
                        OnRefreshBodyLocation(go);
                    }
                }
                else
                {
                    GameObject go = Instantiate(Storage.data.basicBody[(int)gender], bodyHolder, false);
                    OnRefreshBodyLocation(go);
                }
            }
        }
        void OnRefreshBodyLocation(GameObject go)
        {
            ResetMeshAndAnimation(go);
            BodyPlaceholders bp = go.GetComponentInChildren<BodyPlaceholders>();
            if(bp != null)
            {
                effectMount = bp.rightHand;
                rWeaponHolder = bp.rightWeapon;
                lWeaponHolder = bp.leftWeapon;
                wingsHolder = bp.wing;
                animator.avatar = bp.avatar;
            }
            if(wardrobe[(int)ClothingCategory.Weapon] > 0)
            {
                RefreshMainWeaponLocation();// RefreshWeaponsLocation(); // after adding artifacts/second weapon
            }
            if(wardrobe[(int)ClothingCategory.Wings] > 0)
            {
                RefreshWingsLocation();
            }
        }
        void ResetMeshAndAnimation(GameObject go)
        {
            SkinnedMeshRenderer equipmentSkin = go.GetComponentInChildren<SkinnedMeshRenderer>();
            if (equipmentSkin != null && CanReplaceAllBones(equipmentSkin))
            {
                ReplaceAllBones(equipmentSkin);
            }
            //Animator anims = go.GetComponentInChildren<Animator>();
            //anims.runtimeAnimatorController = animator.runtimeAnimatorController;// assign main animation controller to it
            RebindAnimators();// restart all animators, so that skinned mesh equipment will be in sync with the main animation
        }
        void RefreshMainWeaponLocation()
        {
            Transform mainHand = classInfo.type == PlayerClass.Archer ? lWeaponHolder : rWeaponHolder;
            if(mainHand == null)
                return;
            int index = (int)ClothingCategory.Weapon;
            if(wardrobe[index] > 0 && showWardrobe && ScriptableWardrobe.dict.TryGetValue(wardrobe[index], out ScriptableWardrobe weapon)) {
                if(weapon.category != ClothingCategory.Weapon)
                    return;
                if(mainHand.childCount > 0) 
                    Destroy(mainHand.GetChild(0).gameObject);
                Instantiate(weapon.modelPrefab[(int)classInfo.type], mainHand, false);
            }
            else {// armor index => 7
                if(equipment[1].id < 1) 
                    return;
                EquipmentItem itemData = (EquipmentItem)equipment[1].data;
                if(itemData != null && itemData.category == EquipmentsCategory.Weapon && itemData.modelPrefab[0] != null) {
                    if(mainHand.childCount > 0) 
                        Destroy(mainHand.GetChild(0).gameObject);
                    Instantiate(itemData.modelPrefab[0], mainHand, false);
                }
            }
        }
        void RefreshWingsLocation()
        {
            if(wingsHolder == null)
                return;
            if(wingsHolder.childCount > 0)
                Destroy(wingsHolder.GetChild(0).gameObject);
            int index = (int)ClothingCategory.Wings;
            if(wardrobe[index] > 0 && showWardrobe)
                Instantiate(ScriptableWardrobe.dict[wardrobe[index]].modelPrefab[0], wingsHolder, false);
        }
        void RefreshSoulLocation() {
            if(spiritHolder == null)
                return;
            if(spiritHolder.childCount > 0)
                Destroy(spiritHolder.GetChild(0).gameObject);
            int index = (int)ClothingCategory.Spirit;
            if(wardrobe[index] > 0 && showWardrobe)
                Instantiate(ScriptableWardrobe.dict[wardrobe[index]].modelPrefab[0], spiritHolder, false);
        }
        public void RefreshAllLocation() {
            RefreshBodyLocation();
            RefreshMainWeaponLocation();// RefreshWeaponsLocation(); // after adding artifacts/second weapon
            RefreshWingsLocation();
            RefreshSoulLocation();
        }
        // helpers
        bool CanReplaceAllBones(SkinnedMeshRenderer equipmentSkin) {
            // are all equipment SkinnedMeshRenderer bones in the player bones?
            foreach(Transform bone in equipmentSkin.bones)
                if(!skinBones.ContainsKey(bone.name))
                    return false;
            return true;
        }
        void ReplaceAllBones(SkinnedMeshRenderer equipmentSkin) {
            // get equipment bones
            Transform[] bones = equipmentSkin.bones;
            // replace each one
            for (int i = 0; i < bones.Length; ++i) {
                string boneName = bones[i].name;
                if(!skinBones.TryGetValue(boneName, out bones[i]))
                    Debug.LogWarning(equipmentSkin.name + " bone " + boneName + " not found in original player bones. Make sure to check CanReplaceAllBones before.");
            }
            // reassign bones
            equipmentSkin.bones = bones;
        }
        void RebindAnimators() {
            foreach(Animator anim in GetComponentsInChildren<Animator>())
                anim.Rebind();
        }
        void Awake() {
            foreach (SkinnedMeshRenderer skin in GetComponentsInChildren<SkinnedMeshRenderer>())
                foreach (Transform bone in skin.bones)
                    skinBones[bone.name] = bone;
        }
    }
}