using UnityEngine;
using UnityEngine.UI;

public class VendorUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button buyUpgradeButton;
    [SerializeField] private Button closeButton;

    public int currentUpgradeLevel; 
    private int[] upgradeCosts = { 200, 400, 800 };

    public static VendorUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        buyUpgradeButton.onClick.AddListener(BuyInventoryUpgrade);
        closeButton.onClick.AddListener(() => VendorManager.Instance.CloseVendor());
        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
        UpdateBuyButton();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    private void UpdateBuyButton()
    {
        if (currentUpgradeLevel >= upgradeCosts.Length)
        {
            buyUpgradeButton.interactable = false;
            buyUpgradeButton.GetComponentInChildren<Text>().text = "Max Upgrade Reached";
        }
        else
        {
            int cost = upgradeCosts[currentUpgradeLevel];
            buyUpgradeButton.interactable = CurrencySystem.Instance.Coins >= cost;
            buyUpgradeButton.GetComponentInChildren<Text>().text = $"Buy Upgrade ({cost})";
        }
    }

    private void BuyInventoryUpgrade()
    {
        if (currentUpgradeLevel >= upgradeCosts.Length) return;

        int cost = upgradeCosts[currentUpgradeLevel];

        if (CurrencySystem.Instance.Spend(cost))
        {
            bool success = InventorySystem.Instance.TryExpandInventory();
            if (success) currentUpgradeLevel++;
            UpdateBuyButton();
        }
    }
}
