using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraSet : MonoBehaviourPunCallbacks
{
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (ManagerAccessor.Instance.dataManager.player1 != null)
                {
                    transform.parent = ManagerAccessor.Instance.dataManager.player1.transform;
                    transform.localPosition = new Vector3(0, 0, -10);
                    first = false;
                }
            }
            else
            {
                if (ManagerAccessor.Instance.dataManager.player2 != null)
                {
                    transform.parent = ManagerAccessor.Instance.dataManager.player2.transform;
                    transform.localPosition = new Vector3(0, 0, -10);
                    first = false;
                }
            }
        }
    }
}
