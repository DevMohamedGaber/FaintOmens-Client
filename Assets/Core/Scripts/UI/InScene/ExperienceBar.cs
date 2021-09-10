using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Game.UI
{
    public class ExperienceBar : MonoBehaviour
    {
        [SerializeField] UIProgressBar bar;
        //[SerializeField] TMP_Text text;
        Player player => Player.localPlayer;
        public void Refresh()
        {
            if(player != null)
            {
                bar.fillAmount = player.own.ExperiencePercent();
                //text.text = $"{player.own.experience} / {player.own.experienceMax}  ({player.own.ExperiencePercent().ToString("F0")}%)";
            }
        }
    }
}