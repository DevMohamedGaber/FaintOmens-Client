using UnityEngine;
using UnityEngine.UI;
using Game.Network;
namespace Game.UI
{
    public class CharacterCreation : UILobbyWindow
    {
        [SerializeField] TMPro.TMP_InputField nameInput;
        [SerializeField] UICharacterCreate_TribeToggle[] tribeList;
        [SerializeField] Image charLogo;
        [SerializeField] Sprite[] charLogosList;
        [SerializeField] PlayerClass selectedClass = PlayerClass.Warrior;
        [SerializeField] Gender selectedGender = Gender.Male;
        [SerializeField] byte selectedTribe = 0;
        public void OnCreate()
        {
            if(nameInput.text.Length == 0)
            {
                Notifications.list.Add("Enter a Name", "ادخل اسم");
                return;
            }
            if(!networkManager.IsAllowedCharacterName(nameInput.text))
            {
                Notifications.list.Add("Invalid Name", "الاسم غير مسموح");
                return;
            }
            if(!ScriptableClass.dict.ContainsKey(selectedClass))
            {
                Notifications.list.Add("Select a Class", "اختر تخصص");
                return;
            }
            if(selectedGender != Gender.Male && selectedGender != Gender.Female)
            {
                Notifications.list.Add("Select a Gender", "اختر نوع");
                return;
            }
            if(!TribeSystem.ValidateId((int)selectedTribe))
            {
                Notifications.list.Add("Select a Tribe", "اختر عشيرة");
                return;
            }
            
            Mirror.NetworkClient.Send(new Game.Network.Messages.CharacterCreate
            {
                name = nameInput.text,
                classId = selectedClass,
                gender = selectedGender,
                tribeId = selectedTribe
            });
        }
        public void OnSelectClass(int classId)
        {
            selectedClass = (PlayerClass)classId;
            charLogo.sprite = charLogosList[classId - 1];
        }
        public void OnSelectGender(int gender)
        {
            selectedGender = (Gender)gender;
        }
        void OnSelectTribe(int tribeId)
        {
            selectedTribe = (byte)tribeId;
        }
        protected override void OnEnable()
        {
            if(UIManager.data.gameManager.state != NetworkState.Lobby)
            {
                Close();
                return;
            }
            base.OnEnable();
            if(TribeSystem.registerdTribes.Count > 0)
            {
                for(int i = 0; i < TribeSystem.registerdTribes.Count; i++)
                {
                    tribeList[i].Set(TribeSystem.registerdTribes[i]);
                    int tribeId = TribeSystem.registerdTribes[i].name;
                    tribeList[i].toggle.onSelect = () => OnSelectTribe(tribeId);
                }
                tribeList[0].toggle.ForceOnSelect();
            }
        }
        public override void Show()
        {
            if(CanCreate())
            {
                base.Show();
            }
        }
        bool CanCreate()
        {
            return UIManager.data.lobby.select.Count() < networkManager.characterLimit;
        }
    }
}