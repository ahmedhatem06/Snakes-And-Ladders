using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [Header("Public Values")]
    public float playersSpeed = 0.1f;

    private int playerIndex = 0;
    private bool canIncrementPlayerIndex = true;
    private int targetPos;
    private float target;
    private List<MainPlayer> Players = new();
    private List<Tile> Shortcuts = new();
    private List<Tile> Pitfalls = new();
    private int rolledDice = 0;
    private bool foundShortcutOrPitfall = false;
    public void GetPlayers(List<MainPlayer> allPlayers)
    {
        Players = allPlayers;
    }

    public void GetShortcutsAndPitfalls(List<Tile> allShortcuts, List<Tile> allPitfalls)
    {
        Shortcuts = allShortcuts;
        Pitfalls = allPitfalls;
    }

    public void DiceRolled(int diceResult)
    {
        rolledDice = diceResult;
        if (Players[playerIndex].hasSkip)
        {
            UIController.instance.skipButton.interactable = true;
            UIController.instance.moveButton.interactable = true;
            //Should Press On Move Button to move.
        }
        else
        {
            if (!Players[playerIndex].hadSkippedRoll)
            {
                rolledDice += Players[playerIndex].lastRoll;
                Players[playerIndex].hadSkippedRoll = true;
            }
            UIController.instance.skipButton.interactable = false;
            UIController.instance.moveButton.interactable = false;
            Players[playerIndex].hadSkippedRoll = true;
            StartCoroutine(MovePlayer(rolledDice));
        }
    }

    public void Move()
    {
        //Debug.Log(Players[playerIndex].savedLastRoll);
        if (rolledDice == 6 && Players[playerIndex].savedLastRoll == 6)
        {
            Players[playerIndex].savedLastRoll = 0;
            UIController.instance.LostTurn(Players[playerIndex].mainPlayer.name);
            EndTurn();
        }
        else
        {
            Players[playerIndex].savedLastRoll = rolledDice;
            UIController.instance.moveButton.interactable = false;
            UIController.instance.skipButton.interactable = false;
            StartCoroutine(MovePlayer(rolledDice));
        }
    }

    public void Skip()
    {
        if (Players[playerIndex].hasSkip)
        {
            Players[playerIndex].hasSkip = false;
            Players[playerIndex].lastRoll = rolledDice;
            EndTurn();
        }
        UIController.instance.skipButton.interactable = false;
        UIController.instance.moveButton.interactable = false;
    }

    public IEnumerator MovePlayer(int diceResult)
    {
        int rolledDiceCounter = 0;
        while (rolledDiceCounter < diceResult)
        {
            float t = ReturnTargetX();
            if (t != -1)
            {
                Players[playerIndex].animator.SetBool("Idle", false);

                Vector3 TargetPos = new(t, Players[playerIndex].mainPlayer.transform.position.y, Players[playerIndex].mainPlayer.transform.position.z);
                if (Players[playerIndex].direction == 1)
                {
                    Players[playerIndex].mainPlayer.transform.rotation = Quaternion.Euler(0, 90, 0);

                    while (Players[playerIndex].mainPlayer.transform.position.x < t)
                    {
                        Players[playerIndex].mainPlayer.transform.position = Vector3.MoveTowards(Players[playerIndex].mainPlayer.transform.position, TargetPos, playersSpeed * Time.deltaTime);
                        yield return null;
                    }
                }
                else
                {
                    Players[playerIndex].mainPlayer.transform.rotation = Quaternion.Euler(0, 270, 0);
                    while (Players[playerIndex].mainPlayer.transform.position.x > t)
                    {
                        Players[playerIndex].mainPlayer.transform.position = Vector3.MoveTowards(Players[playerIndex].mainPlayer.transform.position, TargetPos, playersSpeed * Time.deltaTime);
                        yield return null;
                    }
                }
            }
            else
            {
                t = ReturnTargetZ();

                Players[playerIndex].direction = 1;

                Players[playerIndex].animator.SetBool("Idle", false);

                Players[playerIndex].mainPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);

                Vector3 TargetPos = new(Players[playerIndex].mainPlayer.transform.position.x, Players[playerIndex].mainPlayer.transform.position.y, t);
                while (Players[playerIndex].mainPlayer.transform.position.z < t)
                {
                    Players[playerIndex].mainPlayer.transform.position = Vector3.MoveTowards(Players[playerIndex].mainPlayer.transform.position, TargetPos, playersSpeed * Time.deltaTime);
                    yield return null;
                }

                if (Players[playerIndex].directionRemainder % 2 == 0)
                {
                    Players[playerIndex].direction = -1;
                }
                else
                {
                    Players[playerIndex].direction = 1;
                }

                Players[playerIndex].directionRemainder++;
            }

            CheckWinner();
            rolledDiceCounter++;
        }

        canIncrementPlayerIndex = true;
        for (int i = 0; i < Shortcuts.Count; i++)
        {
            if (Players[playerIndex].mainPlayer.transform.position.x == Shortcuts[i].tileStartWidth && Players[playerIndex].mainPlayer.transform.position.z == Shortcuts[i].tileStartHeight)
            {
                foundShortcutOrPitfall = true;
                targetPos = Shortcuts[i].targetPos;
                int k = Shortcuts[i].tileTargetHeight - Shortcuts[i].tileStartHeight;
                for (int x = 0; x < k; x++)
                {
                    Players[playerIndex].direction *= -1;
                    Players[playerIndex].directionRemainder++;
                }

                AudioManager.instance.ShortcutSoundEffect();

                StartCoroutine(MovePlayerInAShortcut());
                canIncrementPlayerIndex = false;
                break;
            }
        }

        for (int i = 0; i < Pitfalls.Count; i++)
        {
            if (Players[playerIndex].mainPlayer.transform.position.x == Pitfalls[i].tileStartWidth && Players[playerIndex].mainPlayer.transform.position.z == Pitfalls[i].tileStartHeight)
            {
                foundShortcutOrPitfall = true;
                targetPos = Pitfalls[i].targetPos;
                int k = Pitfalls[i].tileStartHeight - Pitfalls[i].tileTargetHeight;
                for (int x = 0; x < k; x++)
                {
                    Players[playerIndex].direction *= -1;
                    Players[playerIndex].directionRemainder++;
                }

                AudioManager.instance.PitfallSoundEffect();

                StartCoroutine(MovePlayerInAPitfall());
                canIncrementPlayerIndex = false;
                break;
            }
        }
        if (!foundShortcutOrPitfall)
        {
            EndTurn();
        }
        foundShortcutOrPitfall = false;
    }

    private void CheckWinner()
    {
        if (GameController.instance.gridManager.height % 2 == 0)
        {
            if (Players[playerIndex].mainPlayer.transform.position.x == 0 && Players[playerIndex].mainPlayer.transform.position.z == GameController.instance.gridManager.height - 1)
            {
                AudioManager.instance.VictorySoundEffect();

                Players[playerIndex].animator.SetTrigger("Victory");

                UIController.instance.EndGame(Players[playerIndex].mainPlayer.name);
                StopAllCoroutines();
            }
        }
        else
        {
            if (Players[playerIndex].mainPlayer.transform.position.x == GameController.instance.gridManager.width - 1 && Players[playerIndex].mainPlayer.transform.position.z == GameController.instance.gridManager.height - 1)
            {
                AudioManager.instance.VictorySoundEffect();

                Players[playerIndex].animator.SetTrigger("Victory");

                UIController.instance.EndGame(Players[playerIndex].mainPlayer.name);
                StopAllCoroutines();
            }
        }

    }

    private void IncrementPlayerIndex()
    {
        if (canIncrementPlayerIndex)
        {
            playerIndex++;

            if (playerIndex > Players.Count - 1)
            {
                playerIndex = 0;
            }
        }
    }

    private IEnumerator MovePlayerInAShortcut()
    {
        Players[playerIndex].animator.SetBool("Idle", false);

        Players[playerIndex].mainPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 TargetPos = new(Players[playerIndex].mainPlayer.transform.position.x, Players[playerIndex].mainPlayer.transform.position.y, targetPos);
        while (Players[playerIndex].mainPlayer.transform.position.z < targetPos)
        {
            Players[playerIndex].mainPlayer.transform.position = Vector3.MoveTowards(Players[playerIndex].mainPlayer.transform.position, TargetPos, playersSpeed * Time.deltaTime);
            yield return null;
        }


        CheckWinner();

        canIncrementPlayerIndex = true;
        EndTurn();

    }

    private IEnumerator MovePlayerInAPitfall()
    {
        Players[playerIndex].animator.SetBool("Idle", false);

        Players[playerIndex].mainPlayer.transform.rotation = Quaternion.Euler(0, 180, 0);

        Vector3 TargetPos = new(Players[playerIndex].mainPlayer.transform.position.x, Players[playerIndex].mainPlayer.transform.position.y, targetPos);
        while (Players[playerIndex].mainPlayer.transform.position.z > targetPos)
        {
            Players[playerIndex].mainPlayer.transform.position = Vector3.MoveTowards(Players[playerIndex].mainPlayer.transform.position, TargetPos, playersSpeed * Time.deltaTime);
            yield return null;
        }

        CheckWinner();

        canIncrementPlayerIndex = true;
        EndTurn();

    }

    float ReturnTargetZ()
    {
        target = Players[playerIndex].mainPlayer.transform.position.z + 1f;

        if (target < GameController.instance.gridManager.height)
        {
            return target;
        }
        else
        {
            return -1;
        }
    }

    float ReturnTargetX()
    {
        if (Players[playerIndex].direction == 1)
        {
            target = Players[playerIndex].mainPlayer.transform.position.x + 1f;

            if (target < GameController.instance.gridManager.width)
            {
                return target;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            target = Players[playerIndex].mainPlayer.transform.position.x - 1f;

            if (target > -1)
            {
                return target;
            }
            else
            {
                return -1;
            }
        }
    }

    public void EndTurn()
    {
        Players[playerIndex].animator.SetBool("Idle", true);

        IncrementPlayerIndex();
        GameController.instance.PlayerMoveDone(Players[playerIndex].mainPlayer.name,playerIndex);
    }

}
