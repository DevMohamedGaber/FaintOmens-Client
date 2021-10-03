namespace Game
{
    [System.Serializable]
    public struct MountTrainingAttribute
    {
        public byte _level;
        public ushort exp;

        public int level
        {
            set
            {
                _level = (byte)value;
            }
            get
            {
                return (int)_level;
            }
        }
        public ushort expMax => Storage.data.mount.trainingExpMax[level];
    }
}