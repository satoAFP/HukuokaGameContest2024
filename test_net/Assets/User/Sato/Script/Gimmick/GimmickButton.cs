using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickButton : MonoBehaviourPunCallbacks
{
    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public bool isButton = false;
    [System.NonSerialized] public bool isOwnerOnButton = false;
    [System.NonSerialized] public bool isClientOnButton = false;
    [System.NonSerialized] public bool isOwnerHit = false;
    [System.NonSerialized] public bool isClientHit = false;

    private Rigidbody2D rb2d;


    //�ŏ������������Ȃ�����
    private bool firstPushP1 = true;
    private bool firstPushP2 = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //OnCollisionStay2D����ɓ���������
        rb2d.WakeUp();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //�����ׂ��{�^���̉摜�\��
            if (PhotonNetwork.IsMasterClient)
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB)
            {
                if (firstPushP1)
                {
                    //�{�^�������Ă��锻��
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true, true, true);
                    firstPushP1 = false;
                }
            }
            else
            {
                if (!firstPushP1)
                {
                    //�{�^������������
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, true, false);
                    firstPushP1 = true;
                }
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜�\��
            if (!PhotonNetwork.IsMasterClient)
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (ManagerAccessor.Instance.dataManager.isClientInputKey_CB)
            {
                if (firstPushP2)
                {
                    //�{�^�������Ă��锻��
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true, false, true);
                    firstPushP2 = false;
                }
            }
            else
            {
                if (!firstPushP2)
                {
                    //�{�^������������
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, false, false);
                    firstPushP2 = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, true, true);
        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, false, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, true, false);

            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(false);


            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�{�^�����痣�ꂽ����
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, true, false);
                firstPushP1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, false, false);

            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(false);


            //���g�̃I�u�W�F�N�g���������Ă��鎞�������������Ȃ�
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�{�^�����痣�ꂽ����
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, false, false);
                firstPushP2 = true;
            }
        }
    }


    //�{�^�����͏��𑊎�ɑ��M
    [PunRPC]
    protected void RpcButtonCheck(bool onButton, bool onOwner,bool data)
    {
        isButton = onButton;

        if (onOwner)
            isOwnerOnButton = data;
        else
            isClientOnButton = data;
        
    }

    //�{�^�����͏��𑊎�ɑ��M
    [PunRPC]
    protected void RpcShareIsOwnerHit(bool onOwner, bool data)
    {
        if (onOwner)
            isOwnerHit = data;
        else
            isClientHit = data;

    }
}
