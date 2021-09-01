using UnityEngine;
using UnityEngine.UI;
public class UIHideAfter : MonoBehaviour {
    [SerializeField] float hideAfter;
    private void OnEnable() {
        Invoke(nameof(Hide), hideAfter);
    }
    void Hide() => gameObject.SetActive(false);
}