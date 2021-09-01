namespace Game
{
    [System.Serializable]
    public struct ChatMessage
    {
        public PlayerChatInfo sender;
        public ChatChannels channel;
        public string message;
        public double sendTime;
        public ChatMessage(PlayerChatInfo sender, ChatChannels channel, string message)
        {
            this.sender = sender;
            this.channel = channel;
            this.message = message;
            this.sendTime = System.DateTime.Now.ToOADate();
        }
    }
}