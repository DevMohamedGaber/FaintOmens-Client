// Attach to the prefab for easier component access by the UI Scripts.
// Otherwise we would need slot.GetChild(0).GetComponentInChildren<Text> etc.
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIWorldBossRank : MonoBehaviour {
        public Text rank;
        public Text Name;
        public Text damage;

        public void SetColor(Color color) {
            rank.color = color;
            Name.color = color;
            damage.color = color;
        }
    }
}