using System.Collections;
using UnityEngine;

public class MessagePop : MonoBehaviour
{
    private Animator lostTurnAnimator;
    public TMPro.TextMeshProUGUI lostTurnText;
    private string lostText = " has lost his turn because they rolled 6 twice in a row.";
    public float waitTime = 4;
    void Start()
    {
        lostTurnAnimator = GetComponent<Animator>();
    }

    public void PopMessage(string PlayerName)
    {
        lostTurnText.text = PlayerName + lostText;
        StopAllCoroutines();
        StartCoroutine(ShowAndHideLostTurnMessage());
    }

    IEnumerator ShowAndHideLostTurnMessage()
    {
        lostTurnAnimator.SetTrigger("Show");
        yield return new WaitForSeconds(waitTime);
        lostTurnAnimator.SetTrigger("Hide");
    }
}
