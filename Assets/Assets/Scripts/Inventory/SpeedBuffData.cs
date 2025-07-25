using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "Buffs/BuffData")]
public class SpeedBuffData : ScriptableObject
{
    public float multiplier = 1.5f;  // Example: 1.5 = +50% 
    public float duration = 10f;     // Duration in Seconds
    [TextArea]
    public string description;
}
