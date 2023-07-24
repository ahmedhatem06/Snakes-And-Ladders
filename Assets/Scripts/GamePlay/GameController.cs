using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GridManager gridManager;
    public GamePlayManager gamePlayManager;
    public DiceZone diceZone;
    public CinemachineManager cinemachineManager;

    [HideInInspector]
    public bool canRoll = true;
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

    public void Rolled()
    {
        canRoll = false;
        UIController.instance.rollButton.interactable = false;
    }

    public void DoneRolling(int diceNumber)
    {
        gamePlayManager.DiceRolled(diceNumber);
        UIController.instance.ChangeDiceText(diceNumber);
    }

    public void PlayerMoveDone(string playerName, int playerIndex)
    {
        UIController.instance.ChangeDiceText(0);
        diceZone.boxCollider.enabled = true;
        canRoll = true;
        UIController.instance.rollButton.interactable = true;
        UIController.instance.ChangePlayerText(playerName);
        cinemachineManager.SwitchVCamToCurrentPlayer(playerIndex);
    }
}
