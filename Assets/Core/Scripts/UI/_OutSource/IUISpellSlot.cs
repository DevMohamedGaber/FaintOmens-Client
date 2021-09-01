using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public interface IUISpellSlot
    {
        Skill GetSpellInfo();
        bool Assign(Skill spellInfo);
        void Unassign();
    }
}
