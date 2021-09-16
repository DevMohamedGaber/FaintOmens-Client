using System.Collections.Generic;
namespace Game
{
    public class SyncListAchievements : Mirror.SyncList<Achievement>
    {
        public bool Has(ushort id)
        {
            if(Count > 0)
            {
                for(int i = 0; i < Count; i++)
                {
                    if(objects[i].id == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public int IndexOf(ushort id)
        {
            if(Count > 0)
            {
                for(int i = 0; i < Count; i++)
                {
                    if(objects[i].id == id)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        public ushort GetTotalPoints()
        {
            ushort result = 0;
            if(Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    result += (ushort)objects[i].data.points;
                }
            }
            return result;
        }
        public List<ushort> NeedClaiming()
        {
            List<ushort> result = new List<ushort>();
            if(Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if(!objects[i].claimed)
                    {
                        result.Add(objects[i].id);
                    }
                }
            }
            return result;
        }
    }
}