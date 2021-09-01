namespace Game
{
    [System.Serializable]
    public class GameSettings
    {
        public float version = 0.10f;
        public float startingVersion = 0.10f;
        public Languages currentLang = LanguageManger.defLang;
        public string token = "";
        public bool remember = false;

        // test
        public string username;
        public string password;

        //in game
        public FPS_Types fps = FPS_Types.Normal;
    }
}