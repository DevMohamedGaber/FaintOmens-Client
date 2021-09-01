using UnityEngine;
using UnityEngine.UI;
public class UIMarketnHouse : MonoBehaviour {
    public GameObject itemsPage;
    public Button itemsButton;
    public GameObject sellPage;
    public Button sellButton;
    void Start() {
        itemsButton.onClick.SetListener(() => {
            itemsPage.SetActive(true);
            sellPage.SetActive(false);
        });
        sellButton.onClick.SetListener(() => {
            itemsPage.SetActive(false);
            sellPage.SetActive(true);
        });
    }

}