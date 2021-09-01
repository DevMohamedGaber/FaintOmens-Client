using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Game
{
    public static class SaveSystem
    {
        static string path = Application.persistentDataPath + "/zxcvbdfadfrc.dil";

        public static void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, UIManager.data.gameSettings);
            stream.Close();
        }
        
        public static void Load()
        {
            if(File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                GameSettings data = formatter.Deserialize(stream) as GameSettings;
                stream.Close();
                UIManager.data.gameSettings = data;//set data
                LanguageManger.Load(data.currentLang);
                UIManager.data.gameUpdater.StartPatcher();//next stage
            }
            else
            {
                UIManager.data.chooseLangPage.Show();
            }
        }
    }
}