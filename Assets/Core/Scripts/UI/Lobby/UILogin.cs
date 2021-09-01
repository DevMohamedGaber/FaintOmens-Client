using System;
using System.Linq;
//using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Game.Network;
namespace Game.UI
{
    public class UILogin : UILobbyWindow
    {
        public NetworkAuthenticatorMMO auth;
        public TMP_InputField accText;
        public TMP_InputField passwordText;
        public TMP_Text errorText;
        public GameObject loading;
        [SerializeField] bool useNetwork;
        public void OnLogin()
        {
            if(!networkManager.isNetworkActive && auth.IsAllowedAccountName(accText.text))
            {
                //await Task.Run(() => {
                    networkManager.StartClient();
                //});
                errorText.text = "";
                //loading.SetActive(true);
            }
        }
        public void OnLoginSuccess()
        {
            loading.SetActive(false);
        }
        public override void OnError(NetworkError error)
        {
            errorText.text = LanguageManger.GetWord(Convert.ToInt32((byte)error), LanguageDictionaryCategories.NetworkError);
            loading.SetActive(false);
        }
        public void OnAccChanged(string acc)
        {
            auth.loginAccount = acc;
        }
        public void OnPassworsChanged(string pass)
        {
            auth.loginPassword = pass;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            networkManager.networkAddress = networkManager.serverList[useNetwork ? 1 : 0].ip;
            OnAccChanged(accText.text);
            OnPassworsChanged(passwordText.text);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            errorText.text = "";
            //accText.text = "";
            //passwordText.text = "";
        }
    }
}