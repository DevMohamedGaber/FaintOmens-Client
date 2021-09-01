using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.UI
{
    public class UIExperienceBar : MonoBehaviour {
        [SerializeField] Slider slider;
        [SerializeField] TMP_Text text;
        Player player => Player.localPlayer;
        public void UpdateData() {
            if(player) {
                slider.maxValue = player.own.experienceMax;
                slider.value = player.own.experience;
                text.text = $"{player.own.experience} / {player.own.experienceMax}  ({player.own.ExperiencePercent().ToString("F0")}%)";
            }
        }
    }
}