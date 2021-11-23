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
        public PlayerModelData model;
        [SerializeField] Transform effectMount;
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
            if(model.body.type == PlayerModelPartType.Clothing && model.body.id > 0)
            {
                if(bodyHolder.childCount > 0)
                {
                    Destroy(bodyHolder.GetChild(0).gameObject);
                }
                GameObject go = Instantiate(ScriptableWardrobe.dict[model.body.id].modelPrefab[(int)model.gender], bodyHolder, false);
                OnRefreshBodyLocation(go);
            }
            else
            {
                if(bodyHolder.childCount > 0)
                {
                    Destroy(bodyHolder.GetChild(0).gameObject);
                }
                if(model.body.id > 0 && ScriptableItem.dict.TryGetValue(model.body.id, out ScriptableItem sItemData))
                {
                    EquipmentItem itemData = (EquipmentItem)sItemData;
                    if(itemData != null && itemData.modelPrefab[(int)model.gender] != null)
                    {
                        GameObject go = Instantiate(itemData.modelPrefab[(int)model.gender], bodyHolder, false);
                        OnRefreshBodyLocation(go);
                    }
                }
                else
                {
                    GameObject go = Instantiate(Storage.data.player.basicBody[(int)model.gender], bodyHolder, false);
                    OnRefreshBodyLocation(go);
                }
            }
        }
        void OnRefreshBodyLocation(GameObject go)
        {
            BodyPlaceholders bp = go.GetComponentInChildren<BodyPlaceholders>();
            if(bp != null)
            {
                effectMount = bp.rightHand;
                rWeaponHolder = bp.rightWeapon;
                lWeaponHolder = bp.leftWeapon;
                wingsHolder = bp.wing;
                animator.avatar = bp.avatar;
            }
            RefreshMainWeaponLocation();// RefreshWeaponsLocation(); // after adding artifacts/second weapon
            if(model.wing > 0)
            {
                RefreshWingsLocation();
            }
            ResetMeshAndAnimation(go);
        }
        void RefreshMainWeaponLocation()
        {
            Transform mainHand = classInfo.type == PlayerClass.Archer ? lWeaponHolder : rWeaponHolder;
            if(mainHand == null)
                return;
            if(model.weapon.type == PlayerModelPartType.Clothing && model.weapon.id > 0
                && ScriptableWardrobe.dict.TryGetValue(model.weapon.id, out ScriptableWardrobe weapon))
            {
                if(weapon.category != ClothingCategory.Weapon)
                return;

                if(mainHand.childCount > 0)
                {
                    Destroy(mainHand.GetChild(0).gameObject);
                }
                Instantiate(weapon.modelPrefab[(int)classInfo.type], mainHand, false);
            }
            else
            {
                if(model.weapon.id > 0 && ScriptableItem.dict.TryGetValue(model.weapon.id, out ScriptableItem sItemData)) 
                {
                    EquipmentItem itemData = (EquipmentItem)sItemData;
                    if(itemData != null && itemData.category == EquipmentsCategory.Weapon && itemData.modelPrefab[0] != null)
                    {
                        if(mainHand.childCount > 0)
                        {
                            Destroy(mainHand.GetChild(0).gameObject);
                        }
                        Instantiate(itemData.modelPrefab[0], mainHand, false);
                    }
                }
            }
        }
        void RefreshWingsLocation()
        {
            if(wingsHolder == null)
                return;
            if(wingsHolder.childCount > 0)
            {
                Destroy(wingsHolder.GetChild(0).gameObject);
            }
            if(model.wing > 0)
            {
                Instantiate(ScriptableWardrobe.dict[model.wing].modelPrefab[0], wingsHolder, false);
            }
        }
        void RefreshSoulLocation()
        {
            if(spiritHolder == null)
                return;
            if(spiritHolder.childCount > 0)
            {
                Destroy(spiritHolder.GetChild(0).gameObject);
            }
            if(model.soul > 0)
            {
                Instantiate(ScriptableWardrobe.dict[model.soul].modelPrefab[0], spiritHolder, false);
            }
        }
        public void RefreshAllLocation()
        {
            RefreshBodyLocation();
            RefreshMainWeaponLocation();// RefreshWeaponsLocation(); // after adding artifacts/second weapon
            RefreshWingsLocation();
            RefreshSoulLocation();
            //ResetMeshAndAnimation(bodyHolder.GetChild(0).gameObject);
        }
        // helpers
        void ResetMeshAndAnimation(GameObject go)
        {
            SkinnedMeshRenderer equipmentSkin = go.GetComponentInChildren<SkinnedMeshRenderer>();
            if (equipmentSkin != null && CanReplaceAllBones(equipmentSkin))
            {
                ReplaceAllBones(equipmentSkin);
            }
            RebindAnimators();// restart all animators, so that skinned mesh equipment will be in sync with the main animation
        }
        bool CanReplaceAllBones(SkinnedMeshRenderer equipmentSkin)
        {
            // are all equipment SkinnedMeshRenderer bones in the player bones?
            foreach(Transform bone in equipmentSkin.bones)
            {
                if(!skinBones.ContainsKey(bone.name))
                    return false;
            }
            return true;
        }
        void ReplaceAllBones(SkinnedMeshRenderer equipmentSkin)
        {
            // get equipment bones
            Transform[] bones = equipmentSkin.bones;
            // replace each one
            for (int i = 0; i < bones.Length; ++i)
            {
                string boneName = bones[i].name;
                if(!skinBones.TryGetValue(boneName, out bones[i]))
                {
                    Debug.LogWarning(equipmentSkin.name + " bone " + boneName + " not found in original player bones. Make sure to check CanReplaceAllBones before.");
                }
            }
            // reassign bones
            equipmentSkin.bones = bones;
        }
        void RebindAnimators()
        {
            foreach(Animator anim in GetComponentsInChildren<Animator>())
            {
                anim.Rebind();
            }
        }
        void Awake()
        {
            foreach (SkinnedMeshRenderer skin in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                foreach (Transform bone in skin.bones)
                {
                    skinBones[bone.name] = bone;
                }
            }
        }
    }
}