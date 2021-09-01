using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIQualityController : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] Image qualityBackground;
        [SerializeField] int previous = -1;
        public void SetFrame(Quality quality)
        {
            if((int)quality == previous)
                return;
            if(animator != null)
            {
                animator.SetInteger("quality", (int)quality);
                previous = (int)quality;
                qualityBackground.enabled = quality > Quality.Normal;
                if(quality > Quality.Normal)
                {
                    qualityBackground.sprite = ScriptableQuality.dict[(int)quality].frameBackground;
                }
            }  
        }
    }
}