using System;
using System.Linq;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
namespace Game
{
    public static class TribeSystem
    {
        public static List<ScriptableTribe> registerdTribes = new List<ScriptableTribe>();
        public static TribeRank PromoteMinRank = TribeRank.King; // includes Demote
        public static TribeRank RecallMinRank = TribeRank.Royalty; // call players to where he/she stands
        
        public static bool ValidateId(int tribeId)
        {
            for(int i = 0; i < registerdTribes.Count; i++)
            {
                if(registerdTribes[i].name == tribeId)
                    return true;
            }
            return false;
        }
    }
}
//public class SyncListTribes : SyncList<Tribe> {}