using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    [SerializeField] private List<ItemData> appliedBuffs = new();

    public float TotalSpeedMultiplier => CalculateTotalMultiplier();

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);

        else Instance = this;
    }

    public void ApplyUpgrade(ItemData buff)
    {
        appliedBuffs.Add(buff);
    }

    private float CalculateTotalMultiplier()
    {
        float total = 1f;

        if(appliedBuffs.Count > 0)
        {
            foreach (var buff in appliedBuffs)
            {
                total += buff.speedMultiplier;
            }
        }
        
        return total;
    }
    
    public int GetAppliedBuffsCount() => appliedBuffs.Count;

    public List<ItemData> GetAppliedBuffs() => appliedBuffs;

    public List<ItemData> SetAppliedBuffs(List<ItemData> data) => appliedBuffs = data;
}
