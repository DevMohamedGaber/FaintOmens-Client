using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [Serializable]
    public struct City
    {
        public string name;
        public int minLvl;
        public CityStatus status;
        public List<Npc> teleportNPCs;
        public CityArea prefab;
    }
}