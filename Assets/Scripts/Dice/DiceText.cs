using TMPro;
using UnityEngine;

public class DiceText : MonoBehaviour
{
    TextMeshProUGUI diceText;

    private void Start()
    {
        diceText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDiceText(int diceNumber)
    {
        diceText.text = diceNumber.ToString();
    }
}
