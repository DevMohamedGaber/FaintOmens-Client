using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace Game.UI
{
    public class UIGameUpdater : MonoBehaviour {
        [SerializeField] string checkVerionsUrl = "http://localhost/index.php";
        [SerializeField] string patchsUrl = "http://localhost/Patches/";
        UnityWebRequest request;
        [SerializeField] float latestVersion = 0f;
        [SerializeField] float verionsToBeDownloaded;
        [SerializeField] float startingVersion;
        float currentVersion => UIManager.data.gameSettings.version;
        float nextVersion => currentVersion + 0.01f;
        bool installing = false;

        IEnumerator GetManifest() {
            request = UnityWebRequest.Get(patchsUrl + nextVersion + ".unity3d.manifest");
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) {
                // show error msg
                Debug.Log(request.error);
            }
            else {
                if(request.isDone) {
                    string[] lines = request.downloadHandler.text.Split('\n');
                    if(lines.Length > 10) {
                        uint crc = uint.Parse(lines[1].Replace("CRC: ", ""));
                        Hash128 hash = Hash128.Parse(lines[5].Split(':')[1].Trim());
                        StartCoroutine(GetNextPatch(hash, crc));
                    }
                }
            }
        }
        IEnumerator GetNextPatch(Hash128 hash, uint crc) {
            request = UnityWebRequestAssetBundle.GetAssetBundle(patchsUrl + nextVersion + ".unity3d", hash, crc);
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) {
                // show error msg
                Debug.Log(request.error);
            }
            else {
                if(request.isDone) {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                    if(bundle != null) {
                        bundle.LoadAllAssets();
                        if(nextVersion <= latestVersion) {
                            
                        }
                    }
                    /*if(bundle != null) {
                        //File.WriteAllBytes(Application.streamingAssetsPath + "/" + bundle.name, request.downloadHandler.data);
                        string[] names = bundle.GetAllAssetNames();
                        if(names.Length > 0)
                            for (int i = 0; i < names.Length; i++) {
                                //string path = PrepareFolders(names[i]);
                                //var asset = bundle.LoadAsset(names[i]);
                                Debug.Log(names[i]);
                            }
                    }*/
                    UIManager.data.gameSettings.version = nextVersion;
                    installing = false;
                    if(currentVersion < latestVersion) {
                        //GetManifest(nextVersion);
                    }
                    else gameObject.SetActive(false);
                }
            }
        }
        string PrepareFolders(string path) {
            string[] folders = path.Split(new char[] {'/'});
            for (int f = 0; f < folders.Length; f++)
                folders[f] = folders[f] != "ummorpg" ? Utils.CapitalizeString(folders[f]) : "uMMORPG";

            string assetName = folders[folders.Length - 1];
            Array.Resize(ref folders, folders.Length - 1);
            string latestPath = Application.dataPath;

            for (int f = 1; f < folders.Length; f++) {
                if(folders[f] == "") continue;
                latestPath += "/" + folders[f];
                if(!Directory.Exists(latestPath))
                    Directory.CreateDirectory(latestPath);
            }
            return latestPath + "/" + assetName;
        }
        IEnumerator CheckVersion() {
            request = UnityWebRequest.Get(checkVerionsUrl);
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) {
                // show error msg
                Debug.Log(request.error);
            }
            else if(request.isDone) {
                float latest = Convert.ToSingle(request.downloadHandler.text);
                if(currentVersion != latest) {
                    latestVersion = latest;
                    verionsToBeDownloaded = latest - currentVersion;
                    startingVersion = currentVersion;
                    //InvokeRepeating(nameof(Updater), 1f, 1f);
                    StartCoroutine(GetManifest());
                }
                else gameObject.SetActive(false);
            }
        }
        void Updater() {
            if(latestVersion != 0f) {
                /*if(currentVersion < latestVersion) {
                    // handle data
                    if(request.isDone && !installing) {
                        StartCoroutine(GetPatch(currentVersion + 0.01f));
                    }s
                    else if(!request.isDone) {
                        //Debug.Log(request.downloadProgress);
                    }
                }*/
            }
        }
        public void StartPatcher() {
            gameObject.SetActive(true);
            gameObject.SetActive(false);
            //StartCoroutine(CheckVersion());
            //AssetBundle.UnloadAllAssetBundles(true);
            //Caching.IsVersionCached(patchsUrl + nextVersion.ToString("R") + ".unity3d");
        }
        void OnDisable() {
            UIManager.data.lobby.login.Show();
            //CancelInvoke(nameof(Updater));
            //SaveSystem.Save();
        }
    }
}