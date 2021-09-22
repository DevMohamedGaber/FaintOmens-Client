using UnityEngine;
using UnityEngine.UI;
using Game.Network;
namespace Game.UI
{
    public partial class UICharacterCreation : UILobbyWindow
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
            //ClassToggle.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
            /*if(TribeSystem.registerdTribes.Count > 0)
            {
                UIUtils.BalancePrefabs(TribeTogglePrefab, TribeSystem.registerdTribes.Count, TribeToggle.transform);
                for(int i = 0; i < TribeSystem.registerdTribes.Count; i++)
                {
                    int tribeId = TribeSystem.registerdTribes[i].name;
                    UITribeToggle toggle = TribeToggle.transform.GetChild(i).GetComponent<UITribeToggle>();
                    toggle.Name.text = LanguageManger.GetWord(tribeId, LanguageDictionaryCategories.Tribe);
                    toggle.icon.sprite = TribeSystem.registerdTribes[i].flag;
                    Toggle tog = toggle.GetComponent<Toggle>();
                    tog.group = TribeToggle;
                    tog.onValueChanged.SetListener((changed) =>
                    {
                        if(changed) OnSelectTribe((byte)tribeId);
                    });
                    TribeToggle.RegisterToggle(tog);
                }
                TribeToggle.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
                selectedTribe = (byte)TribeSystem.registerdTribes[0].name;
            }*/
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