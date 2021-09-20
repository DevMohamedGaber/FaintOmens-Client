using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
[InitializeOnLoad]
public class AutoSave : EditorWindow
{
    [SerializeField] int saveIntervalInMins = 3;
    EditorWaitForSeconds waitForSeconds;
    void Save()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetSceneByName("World"));
        AssetDatabase.SaveAssets();
        Debug.Log("Scene Saved at " + System.DateTime.Now);
    }
    IEnumerator SaveScene()
    {
        while(true)
        {
            yield return waitForSeconds;
            Save();
        }
    }
    [InitializeOnLoadMethod]
    void OnEnable()
    {
        Debug.Log("Auto Save Is On");
        waitForSeconds = new EditorWaitForSeconds(saveIntervalInMins * 60);
        EditorCoroutineUtility.StartCoroutineOwnerless(SaveScene());
    }
}
