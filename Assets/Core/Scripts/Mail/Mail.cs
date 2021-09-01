using UnityEngine;
namespace Game
{
    [System.Serializable]
    public struct Mail
    {
        [HideInInspector] public uint id;
        [HideInInspector] public uint sender;
        [HideInInspector] public string senderName;
        public MailCategory category;
        public string subject;
        [TextArea(0, 30)] public string content;
        [HideInInspector] public bool opened;
        public Currencies currency;
        [HideInInspector] public double sentAt;
        public MailItemSlot[] items;

        public Mail(bool opened = false)
        {
            this.id = 0;
            this.category = MailCategory.System;
            this.sender = 0;
            this.senderName = "";
            this.subject = "";
            this.content = "";
            this.opened = false;
            this.currency = new Currencies();
            this.sentAt = 0;
            this.items = new MailItemSlot[]{};
        }
        public bool IsEmpty()
        {
            if(!currency.recieved)
                return false;
            if(items.Length > 0)
            {
                for(int i = 0; i < items.Length; i++)
                {
                    if(!items[i].recieved)
                        return false;
                }
            }
            return true;
        }
        public bool IsClaimed()
        {
            // not finished
            if(!currency.recieved)
                return false;
            if(items.Length > 0)
            {
                for(int i = 0; i < items.Length; i++)
                {
                    if(!items[i].recieved)
                        return false;
                }
            }
            return true;
        }
    }
}