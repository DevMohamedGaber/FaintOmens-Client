namespace Game.UI.ManagerLists
{
    [System.Serializable]
    public struct Lobby
    {
        public UILobbyWindow current;
        public UILogin login;
        public UICharacterCreation create;
        public UICharacterSelection select;
    }
}