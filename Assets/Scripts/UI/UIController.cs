using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public DiceText diceText;
    public PlayerText playerText;
    public PlayerLeaderBoard playerLeaderBoard;
    public MessagePop messagePop;
    public TMPro.TextMeshProUGUI winnerName;
    [Header("Canvases")]
    public GameObject winnerCanvas;
    public GameObject hudCanvas;
    public GameObject rollCanvas;
    [Header("Buttons")]
    public Button rollButton;
    public Button moveButton;
    public Button skipButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeDiceText(int diceNumber)
    {
        diceText.UpdateDiceText(diceNumber);

    }

    public void ChangePlayerText(string playerName)
    {
        playerText.UpdatePlayerText(playerName);
    }

    public void EndGame(string playerName)
    {
        rollButton.interactable = false;
        moveButton.interactable = false;
        skipButton.interactable = false;
        hudCanvas.SetActive(false);
        rollCanvas.SetActive(false);
        winnerName.text = "The Winner Is: " + playerName;
        winnerCanvas.SetActive(true);
    }

    public void LostTurn(string playerName)
    {
        AudioManager.instance.PitfallSoundEffect();
        messagePop.PopMessage(playerName);
    }
}
