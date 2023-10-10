using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickButton : CGimmick
{
    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public bool isButton = false;

    //�ŏ������������Ȃ�����
    private bool firstPushP1 = true;
    private bool firstPushP2 = true;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    if (firstPushP1)
                    {
                        //�{�^�������Ă��锻��
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true);
                        firstPushP1 = false;
                    }
                }
                else
                {
                    if (!firstPushP1)
                    {
                        //�{�^������������
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                        firstPushP1 = true;
                    }
                }
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    if (firstPushP2)
                    {
                        //�{�^�������Ă��锻��
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true);
                        firstPushP2 = false;
                    }
                }
                else
                {
                    if (!firstPushP2)
                    {
                        //�{�^������������
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                        firstPushP2 = true;
                    }
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�{�^�����痣�ꂽ����
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                firstPushP1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�{�^�����痣�ꂽ����
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                firstPushP2 = true;
            }
        }
    }


    //�{�^�����͏��𑊎�ɑ��M
    [PunRPC]
    protected void RpcButtonCheck(bool onButton)
    {
        isButton = onButton;
    }
}
