using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    private float tileAnimationSpeed;

    private void Start()
    {
        tileAnimationSpeed = GameController.instance.gridManager.width;
    }

    public void Animate(Vector3 targetPos)
    {
        StartCoroutine(AnimateTile(targetPos));
    }

    IEnumerator AnimateTile(Vector3 targetPos)
    {
        while (transform.position.x < targetPos.x || transform.position.z < targetPos.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, tileAnimationSpeed * Time.deltaTime);
            yield return null;
        }
        enabled = false;
    }
}
