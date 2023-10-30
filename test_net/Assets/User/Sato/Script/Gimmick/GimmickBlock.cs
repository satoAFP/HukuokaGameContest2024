using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //�I�u�W�F�N�g�������オ���Ă���Ƃ�
    [System.NonSerialized] public bool liftMode = false;

    private GameObject Player = null;

    //1P�A2P�����ꂼ�ꓖ�����Ă��锻��
    private bool hitOwner = false;
    private bool hitClient = false;

    //�u���b�N�ƃv���C���[�̋���
    private Vector3 dis = Vector3.zero;

    //�A���Ŕ������Ȃ����߂̏���
    private bool first = true;

    private void FixedUpdate()
    {
        if (ManagerAccessor.Instance.dataManager.player1 != null && ManagerAccessor.Instance.dataManager.player2 != null) 
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player = ManagerAccessor.Instance.dataManager.player1;
            }
            else
            {
                Player = ManagerAccessor.Instance.dataManager.player2;
            }

            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ManagerAccessor.Instance.dataManager.chat.text = hitOwner + ":" + hitClient + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + liftMode;
                Debug.Log(hitOwner + ":" + hitClient + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + liftMode);

                if (hitClient)
                {
                    Debug.Log("�������Ă�");
                }
                else
                { 
                    Debug.Log("�������ĂȂ�");
                }
            }

            if(ManagerAccessor.Instance.dataManager.isClientInputKey_C_L_LEFT&& ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_LEFT)
            {
                Debug.Log("��");
            }

            //1P�A2P���G��Ă��邩�A�A�N�V�������Ă���Ƃ������オ��
            if (hitOwner && ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB &&
            hitClient && ManagerAccessor.Instance.dataManager.isClientInputKey_CB)
            {
                if (first)
                {
                    //�����オ�����ʒu�Ɉړ�
                    Vector3 input = gameObject.transform.position;
                    input.y += 1.0f;
                    gameObject.transform.localPosition = input;

                    dis = transform.position - Player.transform.position;

                    first = false;
                }

                //�v���C���[�ɒǏ]������
                gameObject.transform.position = dis + Player.transform.position;

                //�v���C���[�������Ă���Ƃ��A�u���b�N�T�C�h������������
                if (Player.GetComponent<AvatarTransformView>().isPlayerMove)
                    GetComponent<AvatarOnlyTransformView>().isPlayerMove = true;
                else
                    GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                liftMode = true;
                Player.GetComponent<PlayerController>().islift = true;
            }
            else
            {
                if (!first)
                {
                    Debug.Log("ccc");
                    //���̍����ɖ߂�
                    Vector3 input = gameObject.transform.position;
                    input.y -= 1.0f;
                    gameObject.transform.localPosition = input;

                    dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                    first = true;
                    hitOwner = false;
                    hitClient = false;

                    Player.GetComponent<PlayerController>().islift = false;
                }

                //��������
                GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                liftMode = false;
                
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

            //photonView.RPC(nameof(RpcShareIsOwner), RpcTarget.All, true);
            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜�\��
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

            //photonView.RPC(nameof(RpcShareIsClient), RpcTarget.All, true);
            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //�����グ�Ă��Ȃ��Ƃ�
        if (!liftMode)
        {
            if (collision.gameObject.name == "Player1")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                //photonView.RPC(nameof(RpcShareIsOwner), RpcTarget.All, false);
                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                //photonView.RPC(nameof(RpcShareIsClient), RpcTarget.All, false);
                hitClient = false;
            }
        }
    }

    [PunRPC]
    private void RpcShareIsOwner(bool data)
    {
        hitOwner = data;
    }

    [PunRPC]
    private void RpcShareIsClient(bool data)
    {
        hitClient = data;
    }

}
