using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem Instance { get; private set; }

    [SerializeField] private int coins = 0;
    public int Coins => coins;

    public delegate void CurrencyChanged(int newAmount);
    public event CurrencyChanged OnCurrencyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool Spend(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            OnCurrencyChanged?.Invoke(coins);
            return true;
        }
        return false;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCurrencyChanged?.Invoke(coins);
    }

    public void SetCoins(int amount)
    {
        coins = amount;
        OnCurrencyChanged?.Invoke(coins);
    }
}
