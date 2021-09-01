using UnityEngine;
namespace Game.UI
{
    public class UIChangableInterface : MonoBehaviour {
        [SerializeField] GameObject[] normal;
        [SerializeField] GameObject[] arena;
        [SerializeField] GameObject[] worldboss;
        Player player => Player.localPlayer;
        public void UpdateView(PlayerOccupation oldOcc, PlayerOccupation newOcc) {
            // hide old
            if(oldOcc == PlayerOccupation.InMatchArena1v1) HideInterface(arena);
            else HideInterface(normal);
            
            // show new
            if(newOcc == PlayerOccupation.InMatchArena1v1) ShowInterface(arena);
            else ShowInterface(normal);
        }
        void ShowInterface(GameObject[] elements) {
            for(int i = 0; i < elements.Length; i++)
                elements[i].SetActive(true);
        }
        void HideInterface(GameObject[] elements) {
            for(int i = 0; i < elements.Length; i++)
                elements[i].SetActive(false);
        }
    }
}