using UnityEngine;
using UnityEngine.UI;

public class BuffsUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image buffImage;
    [SerializeField] private Text buffAmountText;

    private int buffAmounts => UpgradesManager.Instance.GetAppliedBuffsCount();

    private void Update()
    {
      ShowBuffs();
    }

    private void ShowBuffs()
    {
        if (buffAmounts > 0) 
        {
            panel.SetActive(true);
            buffAmountText.text = $"X{buffAmounts}";

            return;
        }

        panel.SetActive(false);
    }

   

}
