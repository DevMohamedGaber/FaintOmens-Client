using UnityEngine;
using UnityEngine.UI;
using Game.Network.Messages;
namespace Game.UI
{
    public class PreviewManager : MonoBehaviour
    {
        public static PreviewManager singleton;
        [SerializeField] GameObject playerModel;
        [SerializeField] Transform holder;
        [SerializeField] GameObject cam;
        public GameObject go;
        Player localPlayer => Player.localPlayer;
        // General Instantiate
        public void Instantiate(GameObject perfab)
        {
            Clear();
            go = GameObject.Instantiate(perfab, holder, false);
            go.name = perfab.name;
            Animator anim = go.GetComponent<Animator>();
            if(anim != null)
            {
                anim.enabled = true;
            }
            Collider collider = go.GetComponentInChildren<Collider>();
            if(collider != null)
            {
                SetCamera(collider.bounds.center.y);
            }
        }
        // Player Instantiate
        public void InstantiatePlayer(PlayerClassData classData, PlayerModelData modelData)
        {
            Clear();
            go = GameObject.Instantiate(playerModel, holder, false);
            UpdatePlayer(classData, modelData);
        }
        public void UpdatePlayer(PlayerClassData classData, PlayerModelData modelData)
        {
            if(go != null)
            {
                PlayerPreviewModel player = go.GetComponent<PlayerPreviewModel>();
                if(player != null)
                {
                    player.classInfo = classData;
                    player.model = modelData;
                    player.RefreshAllLocation();

                    SetCamera(player.collider.bounds.center.y);
                }
            }
        }
        public void InstantiatePlayer(Player data)
        {
            Clear();
            go = GameObject.Instantiate(playerModel, holder, false);
            UpdatePlayer(data.classInfo, data.model);
        }
        public void InstantiateLocalPlayer() => InstantiatePlayer(localPlayer);
        public void UpdateLocalPlayer() => UpdatePlayer(localPlayer.classInfo, localPlayer.model);
        void SetCamera(float y)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, y, cam.transform.position.z);
            if(cam != null)
            {
                cam.SetActive(true);
            }
        }
        public void Clear()
        {
            if(cam != null)
            {
                cam.SetActive(false);
            }
            if(holder != null && go != null)
            {
                Destroy(go);
            }
        }
        public void Rotate(float rotation)
        {
            if(go != null)
            {
                float y = go.transform.eulerAngles.y;
                go.transform.rotation = Quaternion.Euler(0, y + rotation, 0);
            }
        }
        void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
            }
        }
    }
}