using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIMiniMap : MonoBehaviour {
        [SerializeField] RTLTMPro.RTLTextMeshPro mapName;
        [SerializeField] TMPro.TMP_Text coordinates;
        [SerializeField] float zoomMin = 5;
        [SerializeField] float zoomMax = 50;
        [SerializeField] float zoomStepSize = 5;
        Camera minimapCamera;
        Player player => Player.localPlayer;
        public void OnMapChanged(Camera newMiniMapCamera) {
            minimapCamera = newMiniMapCamera;
            mapName.text = player.city.Name;
        }
        public void OnZoomIn() {
            if(minimapCamera != null)
                minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize - zoomStepSize, zoomMin);
        }
        public void OnZoomOut() {
            if(minimapCamera != null)
                minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomStepSize, zoomMax);
        }
        void UpdateCoords() {
            coordinates.text = $"({player.transform.position.x.ToString("F0")},{player.transform.position.y.ToString("F0")})";
        }
        void Update() {
            if(player != null && player.state == EntityState.Moving) {
                UpdateCoords();
            }
        }
        void Awake() {
            if(player != null)
                UpdateCoords();
        }
    }
}