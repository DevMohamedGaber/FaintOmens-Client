using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIRankingSummonableRow : MonoBehaviour {
        [SerializeField] Text rank;
        [SerializeField] Text owner;
        [SerializeField] Text Name;
        [SerializeField] Text value;
        public void SetPet(SummonableRankingData data, int rankNo) {
            if(ScriptablePet.dict.TryGetValue(data.prefab, out ScriptablePet info)) {
                rank.text = rankNo.ToString();
                owner.text = data.ownerName;
                value.text = data.value.ToString();
                Name.text = $"{info.Name} {LanguageManger.UseSymbols(data.tier.ToString(), "(", ")")}";
                Name.color = UIManager.data.assets.tierColor[(int)data.tier];
            }
        }
        public void SetMount(SummonableRankingData data, int rankNo) {
            if(ScriptableMount.dict.TryGetValue(data.prefab, out ScriptableMount info)) {
                rank.text = rankNo.ToString();
                owner.text = data.ownerName;
                value.text = data.value.ToString();
                Name.text = $"{info.Name} {LanguageManger.UseSymbols(data.tier.ToString(), "(", ")")}";
                Name.color = UIManager.data.assets.tierColor[(int)data.tier];
            }
        }
    }
}