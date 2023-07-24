using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int numberOfPlayers = 1;
    [SerializeField] private List<MainPlayer> Players = new();
    public GameObject[] playersPrefabs;

    private void Start()
    {
        LoadPlayersFromMenu();

        GeneratePlayers();
    }

    private void LoadPlayersFromMenu()
    {
        numberOfPlayers = PlayerPrefs.GetInt("players");
    }

    void GeneratePlayers()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject player = Instantiate(playersPrefabs[Random.Range(0, playersPrefabs.Length)]);
            player.transform.SetParent(transform, false);
            player.name = "Player " + (i + 1);

            MainPlayer mainPlayer = new()
            {
                mainPlayer = player,
                direction = 1,
                animator = player.GetComponent<Animator>()
            };

            Players.Add(mainPlayer);
        }

        GameController.instance.gamePlayManager.GetPlayers(Players);

        UIController.instance.playerLeaderBoard.GenerateLeaderboard(Players);

        UIController.instance.ChangePlayerText(Players[0].mainPlayer.name);

        GameController.instance.cinemachineManager.GenerateVCams(Players);
    }
}

[System.Serializable]
public class MainPlayer
{
    public GameObject mainPlayer;
    public int direction = 1;
    public int directionRemainder = 0;
    public bool hasSkip = true;
    public int lastRoll = 0;
    public bool hadSkippedRoll = false;
    public Animator animator;
    public int savedLastRoll = 0;
}