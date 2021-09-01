namespace Game
{
    [System.Serializable]
    public struct Team
    {
        public uint id;
        public TeamMember[] members;
        public float[] bonuses;
        public uint leaderId;
        public ExperiaceShareType share;
        public bool IsFull => members != null && members.Length == Storage.data.teamMaxCapacity;
        public bool Contains(uint memberId)
        {
            if (members != null)
            {
                for(int i = 0; i < members.Length; i++)
                {
                    if(members[i].id == memberId)
                        return true;
                }
            }
            return false;
        }
        public bool HasOnline()
        {
            for(int i = 0; i < members.Length; i++)
            {
                if(members[i].online)
                    return true;
            }
            return false;
        }
    }
}