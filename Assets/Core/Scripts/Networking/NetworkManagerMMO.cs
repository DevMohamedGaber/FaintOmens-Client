using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game.Network.Messages;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Game.Network
{
    public class NetworkManagerMMO : NetworkManager
    {
        [Header("Components")]
        public NetworkState state = NetworkState.Offline;
        public ServerInfo[] serverList = new ServerInfo[]
        {
            new ServerInfo{name="Local", ip="localhost", port=7777}
        };
        [Header("Data")]
        public int characterLimit = 4;
        public int characterNameMaxLength = 16;
        public float combatLogoutDelay = 5;
        public bool IsAllowedCharacterName(string characterName)
        {
            return characterName.Length <= characterNameMaxLength && Regex.IsMatch(characterName, @"^[a-zA-Z0-9_]+$");
        }
        public override void OnStartClient()
        {
            // setup handlers
            NetworkClient.RegisterHandler<Error>(OnClientError, false); // allowed before auth!
            NetworkClient.RegisterHandler<CharactersAvailable>(OnClientCharactersAvailable);
        }
        void Update()
        {   // any valid local player? then set state to world
            if (NetworkClient.localPlayer != null)
                state = NetworkState.World;
        }
        void OnClientError(NetworkConnection conn, Error message)
        {
            print("OnClientError: " + message.error);

            if(state == NetworkState.Offline)
            {
                if(UIManager.data.lobby.current != null)
                {
                    UIManager.data.lobby.current.OnError(message.error);
                }
            }
            if(message.action == NetworkErrorAction.Disconnect)
            {
                conn.Disconnect();
            }
        }
        void OnClientCharactersAvailable(NetworkConnection conn, CharactersAvailable message)
        {
            state = NetworkState.Lobby;
            UIManager.data.lobby.select.Set(message.characters != null ? message.characters : new CharactersAvailable.CharacterPreview[]{});
        }
        public void ReturnToLobby()
        {
            if(state == NetworkState.World)
            {
                NetworkClient.connection.Send(new ReturnToLobby());
            }
            else
            {
                Notify.list.Add("You are not inside the game", "انت لست بداخل اللعبة");
            }
        }
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            print("OnClientDisconnect");
            base.OnClientDisconnect(conn);
            // set state
            state = NetworkState.Offline;
            // TODO: re-connect with Master Server Lobby
            UIManager.data.lobby.login.Show();
            Storage.data.mainCam.transform.position = Vector3.zero;
        }
        public bool IsConnecting() => NetworkClient.active && !ClientScene.ready;
        public static void Quit()
        {
    #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
        }
    }
}