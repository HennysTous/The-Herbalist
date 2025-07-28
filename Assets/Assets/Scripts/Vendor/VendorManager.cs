using UnityEngine;

public class VendorManager : MonoBehaviour
{
    public static VendorManager Instance { get; private set; }

    public bool isVendorActive;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void OpenVendor()
    {
        isVendorActive = true;
        VendorUI.Instance.Open();
        InventoryUIManager.Instance.OpenInventory(); // Vendor mode
    }

    public void CloseVendor()
    {
        isVendorActive = false;
        VendorUI.Instance.Close();
        InventoryUIManager.Instance.CloseInventory();
    }
}
