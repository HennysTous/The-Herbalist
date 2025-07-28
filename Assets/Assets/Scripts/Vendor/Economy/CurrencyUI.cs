using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private Text coinText;

    private void Start()
    {
        CurrencySystem.Instance.OnCurrencyChanged += UpdateCoins;
        UpdateCoins(CurrencySystem.Instance.Coins); // Initial sync
    }

    private void OnDestroy()
    {
        CurrencySystem.Instance.OnCurrencyChanged -= UpdateCoins;
    }

    private void UpdateCoins(int amount)
    {
        coinText.text = $"{amount}";
    }
}