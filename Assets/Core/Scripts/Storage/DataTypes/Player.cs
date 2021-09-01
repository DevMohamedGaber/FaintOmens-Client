using UnityEngine;
namespace Game.StorageData
{
    [System.Serializable]
    public struct Player
    {
        public byte maxLevel;
        public ExponentialUInt expMax;
        public GameObject indicator;
        public GameObject arrowPrefab;
        public int respawnTime;
        public byte maxInventorySize;
        public int equipmentCount;
        public int accessoriesCount;
    }
}