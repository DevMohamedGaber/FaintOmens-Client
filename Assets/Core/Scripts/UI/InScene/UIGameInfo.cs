using UnityEngine;
using TMPro;
using Mirror;
namespace Game.UI
{
    public class UIGameInfo : MonoBehaviour
    {
        [SerializeField] TMP_Text latencyTxt;
        [SerializeField] TMP_Text timeTxt;
        [SerializeField] float goodThreshold = 0.3f;
        [SerializeField] float okayThreshold = 2;
        [SerializeField] Color goodColor = Color.green;
        [SerializeField] Color okayColor = Color.yellow;
        [SerializeField] Color badColor = Color.red;
        void UpdateData()
        {
            if (NetworkTime.rtt <= goodThreshold)
            {
                latencyTxt.color = goodColor;
            }
            else if (NetworkTime.rtt <= okayThreshold)
            {
                latencyTxt.color = okayColor;
            }
            else
            {
                latencyTxt.color = badColor;
            }
            latencyTxt.text = Mathf.Round((float)NetworkTime.rtt * 1000) + "ms";
            //timeText.text = Server.time.ToString("MM/dd HH:mm");
        }
        void OnEnable()
        {
            if(Player.localPlayer != null)
            {
                InvokeRepeating(nameof(UpdateData), 0, 1f);
            }
        }
        void OnDisable()
        {
            CancelInvoke(nameof(UpdateData));
        }
    }
}