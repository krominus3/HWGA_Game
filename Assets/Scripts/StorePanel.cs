using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private GameObject StorePanel;

    [SerializeField] private Button StoreButton;

    void Start()
    {
        if (StoreButton != null)
        {
            StoreButton.onClick.AddListener(ToggleStorePanel);
        }

        if (StorePanel != null)
        {
            StorePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStorePanel();
        }
    }

    public void ToggleStorePanel()
    {
        if (StorePanel != null)
        {
            bool isActive = StorePanel.activeSelf;
            StorePanel.SetActive(!isActive);
        }
    }

    public void CloseStorePanel()
    {
        if (StorePanel != null && StorePanel.activeSelf)
        {
            StorePanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (StoreButton != null)
        {
            StoreButton.onClick.RemoveListener(ToggleStorePanel);
        }
    }
}
