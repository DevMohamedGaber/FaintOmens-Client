/*using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[Serializable]
public struct Title {
    public int id;
    public bool active;
    public Title(int id, bool active = false) {
        this.id = id;
        this.active = active;
    }
    public ScriptableTitle data {
        get {
            if (!ScriptableTitle.dict.ContainsKey(id))
                throw new KeyNotFoundException("There is no ScriptableTitle with hash=" + id + ". Make sure that all ScriptableItems are in the Resources folder so they are loaded properly.");
            return ScriptableTitle.dict[id];
        }
    }
}
public class SyncListTitles : SyncList<Title> {
    
}*/