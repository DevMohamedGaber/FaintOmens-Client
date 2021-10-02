namespace Game
{
    public class SyncListMounts : Mirror.SyncList<Mount>
    {
        public int Has(ushort id)
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
        public Mount Get(ushort id)
        {
            if(Count > 0)
            {
                for(int i = 0; i < Count; i++)
                {
                    if(objects[i].id == id)
                    {
                        return objects[i];
                    }
                }
            }
            return Mount.Empty;
        }
    }
}