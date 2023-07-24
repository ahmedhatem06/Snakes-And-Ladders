using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaderBoard : MonoBehaviour
{
    public GameObject content;
    public GameObject imageSeparator;
    public GameObject playerInfo;

    public void GenerateLeaderboard(List<MainPlayer> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            GameObject info = Instantiate(playerInfo);
            info.transform.SetParent(content.transform);
            info.GetComponent<PlayerInfo>().playerName.text = players[i].mainPlayer.name;
            info.GetComponent<PlayerInfo>().playerImage.sprite = players[i].mainPlayer.GetComponent<MainPlayerInfo>().playerSprite;

            if (i + 1 < players.Count)
            {
                GameObject separator = Instantiate(imageSeparator);
                separator.transform.SetParent(content.transform);
            }
        }
    }
}
