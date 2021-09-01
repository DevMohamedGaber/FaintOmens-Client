namespace Game
{
    public class SyncListPets : Mirror.SyncList<PetInfo>
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
        public PetInfo? Get(ushort id)
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
            return null;
        }
    }
}