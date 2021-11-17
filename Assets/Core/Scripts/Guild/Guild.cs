using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [System.Serializable]
    public struct Guild
    {
        public uint id;
        public string Name;
        public byte tribeId;
        public byte level;
        public uint exp;
        public bool AutoAccept;
        public byte JoinLevel;
        public uint br;
        public byte membersCount;
        public string notice;
        public string masterName;
        public PlayerModelDataWithClass masterModel;
        public GuildAssets assets;
        public GuildBuildings buildings;
        public uint expMax => Storage.data.guild.expMax[level - 1];
        public int capacity => Storage.data.guild.capacity + (Storage.data.guild.capacityIncresePerLevel * buildings.hall);
        public uint expPerc => (exp / expMax) * 100;
    }
}