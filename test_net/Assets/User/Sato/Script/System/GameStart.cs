using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameStart : MonoBehaviourPunCallbacks
{
    private const int PLAYER1 = 1;
    private const int PLAYER2 = 2;

    [SerializeField, Header("�v���C���[1�̏o�����W")] private Vector2 p1pos;
    [SerializeField, Header("�v���C���[2�̏o�����W")] private Vector2 p2pos;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            GameObject clone = PhotonNetwork.Instantiate("Avatar", p1pos, Quaternion.identity);
        }
        else
        {
            GameObject clone = PhotonNetwork.Instantiate("Avatar", p2pos, Quaternion.identity);
        }
    }

}
