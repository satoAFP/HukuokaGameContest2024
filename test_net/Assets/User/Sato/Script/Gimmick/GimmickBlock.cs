using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //�I�u�W�F�N�g�������オ���Ă���Ƃ�
    [System.NonSerialized] public bool liftMode = false;

    //���҂̎����グ���������t���O
    private bool isStart = false;

    //�f�[�^�}�l�[�W���[�擾
    DataManager dataManager = null;

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
        dataManager = ManagerAccessor.Instance.dataManager;
        
        if (dataManager.player1 != null && dataManager.player2 != null) 
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (!dataManager.isAppearCopyKey)
                {
                    Player = dataManager.player1;
                }
                else
                {
                    Player = dataManager.copyKey;
                }
            }
            else
            {
                Player = dataManager.player2;
            }
            
            //1P�A2P���G��Ă��邩�A�A�N�V�������Ă���Ƃ������オ��
            if (dataManager.isOwnerInputKey_CB && dataManager.isClientInputKey_CB) 
            {
                //�����グ��������
                if ((hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitLeft) ||
                    (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitRight))
                {
                    isStart = true;
                }

                //�����グ�J�n
                if (isStart)
                {
                    if (first)
                    {
                        //�����オ�����ʒu�Ɉړ�
                        Vector3 input = gameObject.transform.position;
                        input.y += 1.2f;
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

                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!dataManager.isAppearCopyKey)
                        {
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = true;
                        }
                        else
                        {
                            ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().islift = true;
                        }
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = true;
                    }
                }
            }
            else
            {
                if (!first)
                {
                    //���̍����ɖ߂�
                    Vector3 input = gameObject.transform.position;
                    input.y -= 1.2f;
                    gameObject.transform.localPosition = input;

                    dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                    first = true;
                    hitOwner = false;
                    hitClient = false;
                    isStart = false;

                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!dataManager.isAppearCopyKey)
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = false;
                        else
                            ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().islift = false;
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = false;
                    }
                }

                //��������
                GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                liftMode = false;
                
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //�����ׂ��{�^���̉摜�\��
            if (PhotonNetwork.IsMasterClient && (dataManager.isOwnerHitRight || dataManager.isOwnerHitLeft)) 
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜�\��
            if (!PhotonNetwork.IsMasterClient && (dataManager.isClientHitRight || dataManager.isClientHitLeft))
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //�����グ�Ă��Ȃ��Ƃ�
        if (!liftMode)
        {
            if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(false);

                hitClient = false;
            }
        }
    }

}
