using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //�I�u�W�F�N�g�������オ���Ă���Ƃ�
    [System.NonSerialized] public bool liftMode = false;

    //�v���C���[�擾�p
    private GameObject Player;

    //1P�A2P�����ꂼ�ꓖ�����Ă��锻��
    private bool hitOwner = false;
    private bool hitClient = false;

    //�u���b�N�ƃv���C���[�̋���
    private Vector3 dis = Vector3.zero;

    //�A���Ŕ������Ȃ����߂̏���
    private bool first = true;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            Player = GetPlyerObj("Player1");
        else
            Player = GetPlyerObj("Player2");

        //1P�A2P���G��Ă��邩�A�A�N�V�������Ă���Ƃ������オ��
        if (hitOwner && (ManagerAccessor.Instance.dataManager.isOwnerInputKey_LM || ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB) &&
            hitClient && (ManagerAccessor.Instance.dataManager.isClientInputKey_LM || ManagerAccessor.Instance.dataManager.isClientInputKey_CB))
        {
            if(first)
            {
                //�����オ�����ʒu�Ɉړ�
                Vector3 input = gameObject.transform.position;
                input.y += 0.5f;
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

                //���̍����ɖ߂�
                Vector3 input = gameObject.transform.position;
                input.y -= 0.5f;
                gameObject.transform.localPosition = input;

                dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                first = true;

                
            }

            //��������
            GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

            liftMode = false;
            Player.GetComponent<PlayerController>().islift = false;
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
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
                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                hitClient = false;
            }
        }
    }


}
