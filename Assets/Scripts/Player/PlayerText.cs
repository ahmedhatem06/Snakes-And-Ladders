using TMPro;
using UnityEngine;

public class PlayerText : MonoBehaviour
{
    public TextMeshProUGUI playerText;

    public void UpdatePlayerText(string playerName)
    {
        playerText.text = playerName + "'s" + " Turn";
    }
}
