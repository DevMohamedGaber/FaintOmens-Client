using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game.Network.Messages;
using Mirror;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Game.Network
{
    public class NetworkAuthenticatorMMO : NetworkAuthenticator
    {
        [Header("Components")]
        public NetworkManagerMMO manager;
        [Header("Login")]
        public string loginAccount = "";
        public string loginPassword = "";

        [Header("Security")]
        public string passwordSalt = "at_least_16_byte";
        public int accountMaxLength = 16;
        public override void OnStartClient()
        {
            // register login success message, allowed before authenticated
            NetworkClient.RegisterHandler<LoginSuccess>(OnClientLoginSuccess, false);
        }
        public override void OnClientAuthenticate()
        {
            string hash = Utils.PBKDF2Hash(loginPassword, passwordSalt + loginAccount);
            Login message = new Login{account=loginAccount, password=hash};
            NetworkClient.connection.Send(message);
            print("login message was sent");

            // set state
            manager.state = NetworkState.Handshake;
        }
        void OnClientLoginSuccess(NetworkConnection conn, LoginSuccess msg)
        {   // authenticated successfully. OnClientConnected will be called.
            OnClientAuthenticated.Invoke(conn);
            for (int i = 0; i < msg.tribes.Length; i++)
            {
                if(ScriptableTribe.dict.TryGetValue(msg.tribes[i], out ScriptableTribe tribe))
                {
                    TribeSystem.registerdTribes.Add(tribe);
                }
            }
        }
        public bool IsAllowedAccountName(string account)
        {
            return account.Length <= accountMaxLength && Regex.IsMatch(account, @"^[a-zA-Z0-9_]+$");
        }
        public override void OnServerAuthenticate(NetworkConnection conn) {}
    }
}