using UnityEngine;
namespace Game
{
    [CreateAssetMenu(menuName="Custom/Quests/Location", order=0)]
    public class LocationQuest : ScriptableQuest
    {
        [Header("Fulfillment")]
        public string locationName;
        public override string GetProgress(Quest quest)
        {
            if(quest.progress == 1)
            {
                return LanguageManger.Decide("Quest Completed", "تمت المهمة");
            }
            return LanguageManger.Decide("Location Not reached yet", "لم تصل للمكان المحدد");
        }
        public override bool IsFulfilled(Quest quest)
        {
            return quest.progress == 1;
        }
    }
}