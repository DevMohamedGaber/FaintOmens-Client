using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Items/Feed", order=0)]
    public class FeedItem : ScriptableItem
    {
        [Header("Feed")]
        public ushort amount;
        
        void OnValidate()
        {
            type = ItemType.FeedItem;
        }
    }
}