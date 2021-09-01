using UnityEngine;
namespace Game.Components
{
    public class BowInfo : MonoBehaviour
    {
        public Transform center;
        public AE_BowString bowString;
        public void Set(Player holder)
        {
            if(holder != null && holder.bodyHolder != null)
            {
                Transform shp = holder.bodyHolder.Find("StringHandPosition");
                if(shp != null)
                {
                    bowString = GetComponent<AE_BowString>();
                    if(bowString != null)
                    {
                        bowString.HandBone = shp;
                    }
                }
            }
        }
    }
}