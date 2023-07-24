using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScaler : MonoBehaviour
{
    void Start()
    {
        ScalePlaneOnGrid();
    }

    private void ScalePlaneOnGrid()
    {
        transform.localScale = new Vector3(0.05f * GameController.instance.gridManager.width, 
            transform.localScale.y, 0.05f * GameController.instance.gridManager.height);
    }
}
