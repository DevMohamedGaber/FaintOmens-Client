using UnityEngine;
public class UISwitchGameObjectActivation : MonoBehaviour
{
    public void Switch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}