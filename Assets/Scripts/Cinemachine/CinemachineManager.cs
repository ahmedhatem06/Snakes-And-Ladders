using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public List<CinemachineVirtualCamera> virtualCameras = new();
    public CinemachineVirtualCamera vCam;

    public void GenerateVCams(List<MainPlayer> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            CinemachineVirtualCamera tempVCam = Instantiate(vCam);
            tempVCam.transform.SetParent(transform);
            AssignLookAtAndFollowTarget(tempVCam, players[i].mainPlayer);
            virtualCameras.Add(tempVCam);
        }
        virtualCameras[0].enabled = true;
    }

    private void AssignLookAtAndFollowTarget(CinemachineVirtualCamera vcam, GameObject target)
    {
        vcam.Follow = target.transform;
        vcam.LookAt = target.transform;
    }

    public void SwitchVCamToCurrentPlayer(int currentIndex)
    {

        for (int i = 0; i < virtualCameras.Count; i++)
        {
            virtualCameras[i].enabled = false;
        }

        virtualCameras[currentIndex].enabled = true;
    }
}
