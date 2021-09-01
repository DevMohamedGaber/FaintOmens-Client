namespace Game
{
    [System.Serializable]
    public enum PlayerOccupation : byte
    {
        None,
        // arena 1v1
        RegisteredArena1v1,
        ReadyArena1v1,
        InMatchArena1v1,
    }
}