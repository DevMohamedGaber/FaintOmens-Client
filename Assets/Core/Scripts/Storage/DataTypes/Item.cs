using UnityEngine;
namespace Game.StorageData
{
    [System.Serializable]
    public struct Item
    {
        public byte maxPlus;
        public uint[] plusUpCount;
        [SerializeField] ExponentialUInt plusCostGrowth;
        public uint[] plusUpCost;
        public float plusUpSuccessRateReductionPerPlus;
        public float[] plusUpSuccessRate;
        public ushort[] equipmentQualityExpMax;
        public ushort unlockSocketItemId;
        public void OnAwake() {
            int i;
            // plus
            // cost
            plusUpCost = new uint[maxPlus];
            for(i = 0; i < maxPlus; i++) {
                plusUpCost[i] = plusCostGrowth.Get(i);
            }
            // success rate
            plusUpSuccessRate = new float[maxPlus];
            for(i = 0; i < maxPlus; i++) {
                plusUpSuccessRate[i] = 100f - (plusUpSuccessRateReductionPerPlus * (float)i);
            }
        }
    }
}