using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public class UIFriends : MonoBehaviour {
        [SerializeField, Range(0, 1)] float updateInterval = .3f;
        [SerializeField] UILanguageDefiner lang;
        [Header("List")]
        [SerializeField] GameObject noFriendsObj;
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        [SerializeField] GameObject infoPanelObj;
        [SerializeField] GameObject addPanelObj;
        [SerializeField] InputField addInput;

        Player player => Player.localPlayer;
        uint current = 0;
        void UpdateData() {
            noFriendsObj.SetActive(player.own.friends.Count < 1);
            UIUtils.BalancePrefabs(prefab, player.own.friends.Count, content);
            int onlineCount = 0;
            if(player.own.friends.Count > 0) {
                int slotTurn = 0;
                for(int i = 0; i < player.own.friends.Count; i++) {
                    if(!player.own.friends[i].IsOnline()) continue;
                    UIFriendSlot slot = content.GetChild(slotTurn).GetComponent<UIFriendSlot>();
                    slot.Set(player.own.friends[i]);
                    int iCopy = i;
                    slot.button.onClick.SetListener(() => UpdateInfoPanel(player.own.friends[iCopy]));
                    slotTurn++;
                    onlineCount++;
                }
                for(int i = 0; i < player.own.friends.Count; i++) {
                    if(player.own.friends[i].IsOnline()) continue;
                    UIFriendSlot slot = content.GetChild(slotTurn).GetComponent<UIFriendSlot>();
                    slot.Set(player.own.friends[i]);
                    int iCopy = i;
                    slot.button.onClick.SetListener(() => UpdateInfoPanel(player.own.friends[iCopy]));
                    slotTurn++;
                }
            }
            lang.SetSuffix(3, $": {player.own.friends.Count} / {Storage.data.friend.maxCount}");
            lang.SetSuffix(4, $": {onlineCount} / {player.own.friends.Count}");
            lang.RefreshRange(3, 4);
        }
        void UpdateInfoPanel(Friend info) {
            lang.SetSuffix(5, $":</color> <b>{info.id}</b>");//id
            lang.SetSuffix(6, $":</color> <b>{info.classInfo.Name}</b>");//class
            lang.SetSuffix(7, $":</color> <b>{LanguageManger.GetWord(info.tribe, LanguageDictionaryCategories.Tribe)}</b>");//tribe
            lang.SetSuffix(8, $":</color> <b>{info.br}</b>");//br
            lang.RefreshRange(5, 8);
            current = info.id;
            infoPanelObj.SetActive(true);
        }
        public void OnCloseInfoPanel() => current = 0;
        public void OnCloseAddPanel() => addInput.text = "";
        public void OnRemove() {
            if(current != 0) {
                player.CmdRemoveFriend(current);
                current = 0;
            }
        }
        public void OnPrivate() {
            if(current != 0) {
                for(int i = 0; i < player.own.friends.Count; i++) {
                    if(player.own.friends[i].id == current && player.own.friends[i].IsOnline()) {
                        gameObject.SetActive(false);
                        UIManager.data.chat.OpenPrivateChatWith(current);
                        return;
                    }
                }
                Notifications.list.Add("Player is Offline.", "اللاعب غير متصل الان.");
            }
        }
        public void OnInspect() {
            if(current != 0) {
                player.CmdPreviewPlayerInfo(current);
            }
        }
        public void OnAdd() {
            if(addInput.text != "") {
                uint id = Convert.ToUInt32(addInput.text);
                if(Server.IsPlayerIdWithInServer(id)) {
                    player.CmdSendFriendRequest(id);
                    addPanelObj.SetActive(false);
                    addInput.text = "";
                }
                else Notifications.list.Add("Please enter a valid ID.", "برجاء ادخال رقم تعريفي صحيح.");
            }
            else Notifications.list.Add("Please enter an ID.", "برجاء ادخال رقم تعريفي.");
        }
        public void Show() => gameObject.SetActive(true);
        void OnEnable() {
            if(player != null) {
                player.CmdRefreshOnlineFriends();
                InvokeRepeating(nameof(UpdateData), 0, updateInterval);
            }
        }
        void OnDisable() {
            CancelInvoke(nameof(UpdateData));
            current = 0;
            infoPanelObj.SetActive(false);
        }
    }
}