using System;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/LuckCharm", order=0)]
    public class LuckCharmItem : ScriptableItem
    {
        public float amount = 10f;
    }
}