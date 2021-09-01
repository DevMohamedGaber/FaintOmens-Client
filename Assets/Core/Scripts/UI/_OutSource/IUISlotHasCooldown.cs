namespace Game.UI
{
    using UnityEngine.Events;

    public interface IUISlotHasCooldown
    {
        Skill GetSpellInfo();
        UISlotCooldown cooldownComponent { get; }
        void SetCooldownComponent(UISlotCooldown cooldown);
    }
}
