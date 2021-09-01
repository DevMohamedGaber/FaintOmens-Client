using UnityEngine;
using UnityEngine.UI;
using System;
namespace Game.UI
{
    public class UIMail : MonoBehaviour {
        [Header("General")]
        [SerializeField, Range(0, 1)] float updateInterval = .3f;
        [SerializeField] UILanguageDefiner lang;
        [SerializeField] Sprite mailIcon;
        Player player => Player.localPlayer;
        int current = -1;
        [Header("List")]
        [SerializeField] GameObject noMails;
        [SerializeField] Transform content;
        [SerializeField] GameObject prefab;
        [SerializeField] Button collectAllBtn;
        [SerializeField] Button deleteAllBtn;
        [Header("Info")]
        [SerializeField] GameObject currentInfoObj;
        [SerializeField] RTLTMPro.RTLTextMeshPro messageText;
        [SerializeField] GameObject[] currencyObjs;
        [SerializeField] Text[] currencyText;
        [SerializeField] Transform itemsContent;
        [SerializeField] GameObject itemSlotPrefab;
        [SerializeField] Text sendTime;
        [SerializeField] Button collectBtn;
        [SerializeField] GameObject claimed;
        void UpdateData() {
            UpdateMailList();
            currentInfoObj.SetActive(current > -1);
            if(current > -1) 
                UpdateCurrentMailInfo();
            noMails.SetActive(player.own.mailBox.Count < 1);
        }
        void UpdateMailList() {
            UIUtils.BalancePrefabs(prefab, player.own.mailBox.Count, content);
            if(player.own.mailBox.Count > 0) {
                for(int i = 0; i < player.own.mailBox.Count; i++) {
                    UIMailListItem mailBtn = content.GetChild(i).GetComponent<UIMailListItem>();
                    Mail mail = player.own.mailBox[i];
                    mailBtn.notOpened.SetActive(!mail.opened);
                    //subject
                    if(mail.category != MailCategory.Private)
                        mailBtn.lang.SetPrefix(0, mail.subject);
                    else mailBtn.lang.SetCode(0, Convert.ToInt32(mail.subject));
                    mailBtn.lang.SetPrefix(1, mail.senderName);//sender
                    mailBtn.lang.SetPrefix(2, "30d");
                    mailBtn.lang.Refresh();
                    //icon
                    if(mail.currency.gold > 0) mailBtn.image.sprite = Storage.data.currencyIcons[0];
                    else if(mail.currency.diamonds > 0) mailBtn.image.sprite = Storage.data.currencyIcons[1];
                    else if(mail.currency.b_diamonds > 0) mailBtn.image.sprite = Storage.data.currencyIcons[2];
                    else if(mail.items.Length > 0) mailBtn.image.sprite = mail.items[i].item.data.image;
                    else mailBtn.image.sprite = mailIcon;
                    //button
                    int iCopy = i;
                    mailBtn.button.onClick.SetListener(() => {
                        if(current != iCopy) {
                            current = iCopy;
                            if(!player.own.mailBox[iCopy].opened) {
                                player.CmdMarkAsSeen(iCopy);
                            }
                        }
                    });
                    
                }
            }
            collectAllBtn.interactable = player.own.mailBox.Count > 0;
            deleteAllBtn.interactable = player.own.mailBox.Count > 0;
        }
        void UpdateCurrentMailInfo() {
            Mail mail = player.own.mailBox[current];
            messageText.text = mail.content;
            lang.SetSuffix(5, ": " + mail.subject);
            lang.SetSuffix(6, ": " + LanguageManger.GetWord((int)mail.category));
            lang.RefreshRange(5, 6);
            if(mail.currency.HasCurrency()) {
                //gold
                currencyObjs[0].SetActive(mail.currency.gold > 0);
                if(mail.currency.gold > 0) 
                    currencyText[0].text = mail.currency.gold.ToString();
                //diamonds
                currencyObjs[1].SetActive(mail.currency.diamonds > 0);
                if(mail.currency.diamonds > 0) 
                    currencyText[1].text = mail.currency.diamonds.ToString();
                //b_diamonds
                currencyObjs[2].SetActive(mail.currency.b_diamonds > 0);
                if(mail.currency.b_diamonds > 0) 
                    currencyText[2].text = mail.currency.b_diamonds.ToString();
            }
            //items
            UIUtils.BalancePrefabs(itemSlotPrefab, mail.items.Length, itemsContent);
            if(mail.items.Length > 0) {
                for(int i = 0; i < mail.items.Length; i++) {
                    UIRecievableItemSlot itemSlot = itemsContent.GetChild(i).GetComponent<UIRecievableItemSlot>();
                    itemSlot.Assign(mail.items[i].item, mail.items[i].amount);
                    itemSlot.SetRecieved(mail.items[i].recieved);
                }
            }
            collectBtn.interactable = mail.IsEmpty();
            claimed.SetActive(mail.IsClaimed());
        }
        public void OnCollectAll() {
            if(player.own.mailBox.Count > 0) {
                bool notEmpty = false;
                for(int i = 0; i < player.own.mailBox.Count; i++) {
                    if(!player.own.mailBox[i].IsEmpty()) {
                        notEmpty = true;
                        break;
                    }
                }
                if(notEmpty) player.CmdCollectAllMails();
                else UINotifications.list.Add("All mails are empty.");
            }
        }
        public void OnCollect() {
            if(current > -1 && player.own.mailBox.Count > current) {
                if(!player.own.mailBox[current].IsEmpty()) {
                    player.CmdCollectMail(current);
                }
                else UINotifications.list.Add("Already Recieved.");
            }
        }
        public void OnDeleteAll() {
            if(player.own.mailBox.Count > 0) 
                player.CmdDeleteAllMails();
                current = -1;
        }
        public void OnDelete() {
            if(current > -1 && player.own.mailBox.Count > current) {
                if(player.own.mailBox[current].items.Length > 0)
                    player.CmdDeleteMail(current);
                    current = -1;
            }
        }
        public void Show() => gameObject.SetActive(true);
        void OnEnable() {
            if(player != null) {
                InvokeRepeating(nameof(UpdateData), 0, updateInterval);
            }
            else gameObject.SetActive(false);
        }
        void OnDisable() {
            CancelInvoke(nameof(UpdateData));
            current = -1;
            currentInfoObj.SetActive(false);
        }
    }
}