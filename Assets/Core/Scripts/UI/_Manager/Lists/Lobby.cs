namespace Game.UI.ManagerLists
{
    [System.Serializable]
    public struct Lobby
    {
        public UILobbyWindow current;
        public UILogin login;
        public CharacterCreation create;
        public UICharacterSelection select;
    }
}