using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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
                    //�o�[�`�����J������P1�ݒ�
                    GetComponent<CinemachineVirtualCamera>().Follow = ManagerAccessor.Instance.dataManager.player1.transform;
                    first = false;
                }
            }
            else
            {
                if (ManagerAccessor.Instance.dataManager.player2 != null)
                {
                    //�o�[�`�����J������P2�ݒ�
                    GetComponent<CinemachineVirtualCamera>().Follow = ManagerAccessor.Instance.dataManager.player2.transform;
                    first = false;
                }
            }
        }
    }
}
