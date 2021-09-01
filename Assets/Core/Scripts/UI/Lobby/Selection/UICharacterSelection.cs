using System;
using UnityEngine;
using UnityEngine.UI;
using RTLTMPro;
using Mirror;
using Game.Network;
using Game.Network.Messages;
namespace Game.UI
{
    public class UICharacterSelection : UILobbyWindow
    {
        [SerializeField] Transform content;
        [SerializeField] UIToggleGroup charsToggleGroup;
        [SerializeField] GameObject selectPrefab;
        [SerializeField] GameObject createPrefab;
        [SerializeField] Image tribeFlag;
        [SerializeField] GameObject confirmDelete;
        [SerializeField] GameObject deleteBtn;
        [SerializeField] GameObject enterBtn;
        CharactersAvailable.CharacterPreview[] characters;
        int selected = -1;
        public void Set(CharactersAvailable.CharacterPreview[] charsData)
        {
            Reset();
            bool hasChars = charsData != null && charsData.Length > 0;
            deleteBtn.SetActive(hasChars);
            enterBtn.SetActive(hasChars);
            tribeFlag.gameObject.SetActive(hasChars);
            if(hasChars)
            {
                characters = charsData;
                int i;
                for(i = 0; i < charsData.Length; i++)
                {
                    GameObject go = Instantiate(selectPrefab, content, false);
                    go.GetComponent<UICharacterSelection_SelectButton>().Set(characters[i], i);
                }
                charsToggleGroup.UpdateTogglesList();
                if(charsData.Length < networkManager.characterLimit)
                {
                    FillWithCreateButtons();
                }
            }
            else
            {
                FillWithCreateButtons();
                UIManager.data.lobby.create.Show();
                return;
            }
            if(charsData != null && characters.Length > 0)
            {
                OnSelect(0);
            }
            gameObject.SetActive(true);
        }
        void FillWithCreateButtons()
        {
            if(characters.Length < networkManager.characterLimit)
            {
                for(int i = characters.Length; i < networkManager.characterLimit; i++)
                {
                    Instantiate(createPrefab, content, false)
                    .transform.GetComponent<UIBasicButton>().onClick = () =>
                    {
                        UIManager.data.lobby.create.Show();
                    };
                }
            }
        }
        public void OnSelect(int index)
        {
            //if(index == selected)
            //    return;
            if(characters != null && index >= 0 && index < characters.Length)
            {
                selected = index;
                UIPreviewManager.singleton.InstantiatePlayerFromSelectionMsg(characters[index]);
                //tribeFlag.gameObject.SetActive(true);
                tribeFlag.sprite = ScriptableTribe.dict[characters[index].tribeId].flag;
            }
            else
            {
                UIManager.data.lobby.create.Show();
            }
        }
        public void OnStart()
        {
            if(characters == null || selected < 0 || selected > characters.Length)
            {
                UINotifications.list.Add("Select Character", "اختر شخصية");
                return;
            }
            if(!NetworkClient.ready)
                NetworkClient.Ready();
            NetworkClient.connection.Send(new CharacterSelect
            {
                id=characters[selected].id
            });
            Reset();
            gameObject.SetActive(false);
        }
        public void OnDelete()
        {
            if(characters == null || selected < 0 || selected > characters.Length)
            {
                UINotifications.list.Add("Select Character", "اختر شخصية");
                return;
            }
            confirmDelete.SetActive(true);
        }
        public void OnConfirmDelete() {
            if(characters == null || selected < 0 || selected > characters.Length)
            {
                UINotifications.list.Add("Select Character", "اختر شخصية");
                return;
            }
            confirmDelete.SetActive(false);
            NetworkClient.Send(new CharacterDelete
            {
                id=characters[selected].id
            });
        }
        public int Count()
        {
            return characters != null ? characters.Length : 0;
        }
        void Reset()
        {
            UIPreviewManager.singleton.Clear();
            selected = -1;
            characters = new CharactersAvailable.CharacterPreview[] {};
            if(content.childCount > 0)
            {
                UIUtils.DestroyChildren(content);
                charsToggleGroup.Clear();
            }
            
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Reset();
        }
    }
}