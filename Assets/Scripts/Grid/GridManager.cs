using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Width for X-Axis, Height for Y-Axis")]
    public int width, height;

    [Header("Prefabs")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject startPrefab;

    [Header("Colors")]
    public Color pitFallColor;
    public Color targetPitFallColor;
    public Color shortcutColor;
    public Color targetShortcutColor;

    [SerializeField] private List<Tile> Shortcuts = new();
    [SerializeField] private List<Tile> Pitfalls = new();
    [SerializeField] private List<GameObject> allTiles = new();
    private int widthRemainder = 0;
    private int numberOfShortcuts = 2;
    private int numberOfPitfalls = 2;

    int randomStartWidth;
    int randomStartHeight;
    int randomTargetHeight;
    private void Start()
    {

        LoadWidthAndHeightFromMenu();

        GenerateShortcuts(numberOfShortcuts, Shortcuts);

        GeneratePitfalls(numberOfPitfalls, Pitfalls);

        GenerateGrid();

    }

    private void LoadWidthAndHeightFromMenu()
    {
        width = PlayerPrefs.GetInt("width");
        height = PlayerPrefs.GetInt("height");
    }

    private void GenerateShortcuts(int n, List<Tile> tile)
    {
        for (int i = 0; i < n; i++)
        {
            while (true)
            {
                randomStartHeight = Random.Range(0, height - 1);
                randomStartWidth = Random.Range(0, width);
                while (randomStartWidth == 0 && randomStartHeight == 0)
                {

                    randomStartWidth = Random.Range(0, width);
                    randomStartHeight = Random.Range(1, height);
                }

                randomTargetHeight = Random.Range(randomStartHeight, height);

                if (randomStartHeight != randomTargetHeight)
                {
                    bool canProceed = true;

                    if (i != 0)
                    {
                        if (Shortcuts[0].tileStartWidth == randomStartWidth && Shortcuts[0].tileStartHeight == randomStartHeight
                            || Shortcuts[0].tileStartWidth == randomStartWidth && Shortcuts[0].tileTargetHeight == randomTargetHeight
                            || Shortcuts[0].tileStartWidth == randomStartWidth && Shortcuts[0].tileStartHeight == randomTargetHeight
                            || Shortcuts[0].tileStartWidth == randomStartWidth && Shortcuts[0].tileTargetHeight == randomStartHeight)
                        {
                            canProceed = false;
                        }
                    }

                    if (canProceed)
                    {
                        break;
                    }
                }
            }

            Tile shortcutAndPitfall = new()
            {
                tileStartWidth = randomStartWidth,
                tileStartHeight = randomStartHeight,
                tileTargetHeight = randomTargetHeight
            };

            tile.Add(shortcutAndPitfall);

        }
    }

    private void GeneratePitfalls(int n, List<Tile> tile)
    {
        for (int i = 0; i < n; i++)
        {
            while (true)
            {
                randomStartHeight = Random.Range(1, height);

                randomTargetHeight = Random.Range(0, randomStartHeight);
                randomStartWidth = Random.Range(0, width);

                if (height % 2 == 0)
                {
                    while (randomStartWidth == 0 && randomStartHeight == height - 1)
                    {
                        //Debug.Log("Is Last");
                        randomStartWidth = Random.Range(0, width);
                        randomStartHeight = Random.Range(1, height);
                    }
                }
                else
                {
                    while (randomStartWidth == width && randomStartHeight == height - 1)
                    {
                        //Debug.Log("Is Last");
                        randomStartWidth = Random.Range(0, width);
                        randomStartHeight = Random.Range(1, height);
                    }
                }


                //Debug.Log(randomStartWidth + " , " + randomStartHeight);

                if (randomStartHeight != randomTargetHeight)
                {
                    bool canProceed = true;

                    if (i != 0)
                    {
                        if (Pitfalls[0].tileStartWidth == randomStartWidth && Pitfalls[0].tileStartHeight == randomStartHeight
                            || Pitfalls[0].tileStartWidth == randomStartWidth && Pitfalls[0].tileTargetHeight == randomTargetHeight
                            || Pitfalls[0].tileStartWidth == randomStartWidth && Pitfalls[0].tileStartHeight == randomTargetHeight
                            || Pitfalls[0].tileStartWidth == randomStartWidth && Pitfalls[0].tileTargetHeight == randomStartHeight)
                        {
                            canProceed = false;
                        }
                    }

                    for (int k = 0; k < Shortcuts.Count; k++)
                    {
                        if (Shortcuts[k].tileStartWidth == randomStartWidth && Shortcuts[k].tileStartHeight == randomStartHeight
                            || Shortcuts[k].tileStartWidth == randomStartWidth && Shortcuts[k].tileTargetHeight == randomStartHeight
                            || Shortcuts[k].tileStartWidth == randomStartWidth && Shortcuts[k].tileTargetHeight == randomTargetHeight
                            || Shortcuts[k].tileStartWidth == randomStartWidth && Shortcuts[k].tileStartHeight == randomTargetHeight)
                        {
                            canProceed = false;
                        }
                    }

                    if (canProceed)
                    {
                        break;
                    }
                }
            }

            Tile shortcutAndPitfall = new()
            {
                tileStartWidth = randomStartWidth,
                tileStartHeight = randomStartHeight,
                tileTargetHeight = randomTargetHeight
            };

            tile.Add(shortcutAndPitfall);

        }
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                GameObject tileGameObject = GenerateTile(x, y);
                allTiles.Add(tileGameObject);
                if (widthRemainder % 2 == 0)
                {
                    if (y < width - 1)
                    {
                        GenerateArrow(new Vector2(0.6f, 0f), new Vector3(0, 0, 0), tileGameObject, "Arrow Right");
                    }
                    else
                    {
                        GenerateArrow(new Vector2(0f, 0.6f), new Vector3(0, 0, 90f), tileGameObject, "Arrow Up");
                    }
                }
                else
                {
                    if (y != 0)
                    {
                        GenerateArrow(new Vector2(-0.6f, 0f), new Vector3(0, 0, 180f), tileGameObject, "Arrow Left");
                    }
                    else
                    {
                        GenerateArrow(new Vector2(0f, 0.6f), new Vector3(0, 0, 90f), tileGameObject, "Arrow Up");
                    }
                }

                ReColorPitfallsAndShortcutsAndSaveTargetPos(x, y, tileGameObject, Pitfalls, pitFallColor, targetPitFallColor);
                ReColorPitfallsAndShortcutsAndSaveTargetPos(x, y, tileGameObject, Shortcuts, shortcutColor, targetShortcutColor);
            }
            widthRemainder++;
        }

        GameController.instance.gamePlayManager.GetShortcutsAndPitfalls(Shortcuts, Pitfalls);

        GenerateStarAndStartAndRemoveLastArrowUp();
    }

    private void GenerateStarAndStartAndRemoveLastArrowUp()
    {
        GameObject tempStart = Instantiate(startPrefab);
        tempStart.transform.SetParent(allTiles[0].transform, false);

        GameObject tempStar = Instantiate(starPrefab);
        if (height % 2 == 0)
        {
            tempStar.transform.SetParent(allTiles[^width].transform, false);
            foreach (Transform child in allTiles[^width].transform)
            {
                if (child.name == "Arrow Up")
                {
                    child.GetComponent<Image>().enabled = false;
                }
            }
        }
        else
        {
            tempStar.transform.SetParent(allTiles[^1].transform, false);

            foreach (Transform child in allTiles[^1].transform)
            {
                if (child.name == "Arrow Up")
                {
                    child.GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    void GenerateArrow(Vector2 Pos, Vector3 quaternionEuler, GameObject tile, string name)
    {
        GameObject tempArrow = Instantiate(arrowPrefab);
        tempArrow.GetComponent<RectTransform>().SetPositionAndRotation(Pos, Quaternion.Euler(quaternionEuler));
        tempArrow.transform.SetParent(tile.transform, false);
        tempArrow.name = name;
    }

    GameObject GenerateTile(int x, int y)
    {
        GameObject tileGameObject = Instantiate(tilePrefab);
        tileGameObject.transform.SetParent(transform, false);
        if (tileGameObject.GetComponent<TileAnimation>())
        {
            tileGameObject.GetComponent<TileAnimation>().Animate(new(y, 0, x));
        }
        tileGameObject.name = "Tile " + x + " " + y;
        return tileGameObject;
    }

    private void ReColorPitfallsAndShortcutsAndSaveTargetPos(int x, int y, GameObject tileGameObject, List<Tile> tile, Color defaultColor, Color targetColor)
    {
        for (int i = 0; i < tile.Count; i++)
        {
            if (tile[i].tileStartHeight == x && tile[i].tileStartWidth == y)
            {
                tileGameObject.GetComponent<Image>().color = defaultColor;
            }

            if (tile[i].tileTargetHeight == x && tile[i].tileStartWidth == y)
            {
                tileGameObject.GetComponent<Image>().color = targetColor;
                tile[i].targetPos = x;
            }
        }
    }
}
[System.Serializable]
public class Tile
{
    public int tileStartWidth;
    public int tileStartHeight;
    public int tileTargetHeight;
    public int targetPos;
}