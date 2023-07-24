using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GridChecker : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField playersInputField;
    public TMP_InputField rowsInputField;
    public TMP_InputField columnsInputField;

    [Header("Warning Messages")]
    public GameObject playersWarningMessage;
    public GameObject rowsAndColumnsWarningMessage;
    private int canStartGame = 0;
    int parsedRow = 0;
    int parsedColumn = 0;
    int parsedPlayers = 0;
    public void ClickedOnStartGame()
    {
        canStartGame = 0;

        if (rowsInputField.text.Length > 0)
        {
            parsedRow = int.Parse(rowsInputField.text);
        }
        if (columnsInputField.text.Length > 0)
        {
            parsedColumn = int.Parse(columnsInputField.text);
        }
        if (playersInputField.text.Length > 0)
        {
            parsedPlayers = int.Parse(playersInputField.text);
        }

        if (parsedRow < 4 || parsedColumn < 4)
        {
            rowsAndColumnsWarningMessage.SetActive(true);
        }
        else
        {
            rowsAndColumnsWarningMessage.SetActive(false);
            canStartGame++;
        }

        if (parsedPlayers <= 0)
        {
            playersWarningMessage.SetActive(true);
        }
        else
        {
            playersWarningMessage.SetActive(false);
            canStartGame++;
        }

        if (canStartGame == 2)
        {
            SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
            PlayerPrefs.SetInt("width", parsedRow);
            PlayerPrefs.SetInt("height", parsedColumn);
            PlayerPrefs.SetInt("players", parsedPlayers);
        }
    }
}
