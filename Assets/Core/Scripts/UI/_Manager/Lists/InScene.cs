namespace Game.UI.ManagerLists
{
    [System.Serializable]
    public struct InScene
    {
        public ExperienceBar expBar;
        public UIEventCounter counter;
        public UILevelUpNotice levelUpNotice;
        public UIArenaMatchNotify arenaNotify;
        public UIArenaMatchResult arenaMatchResult;
        public UIChangableInterface changableInterface;
        public UIRespawn respawn;
        public ToolTip tooltip;
        public UISideBox sideBox;
        public SelectedTargetInfo selectedTargetInfo;
        public MiniMap miniMap;
    }
}