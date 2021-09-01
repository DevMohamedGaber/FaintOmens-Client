using UnityEngine;
using Mirror;
namespace Game
{
    public class Meteor : Entity
    {
        [Header("Meteor Static")]
        [SerializeField] RTLTMPro.RTLTextMeshPro3D nameText;
        [Header("Meteor Synced")]
        [SyncVar, SerializeField] byte level;
        [SyncVar, SerializeField] Tier tier = Tier.F;
        void Start()
        {
            nameText.text = LanguageManger.UseSymbols(tier.ToString(), "[", "]") + " " + LanguageManger.Decide("Meteor Lvl." + level, "نيزك مستوي " + level);
        }
        [Client] protected override void UpdateClient() {}
        public override void ResetMovement() {}
    }
}