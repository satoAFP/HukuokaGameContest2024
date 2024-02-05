using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerImage : MonoBehaviourPunCallbacks
{
    private string parentObjectName;//�e�I�u�W�F�N�g�̖��O���擾����
    //�v���C���[�摜
    //�v���C���[1
    [SerializeField, Header("��")]
    private Sprite p1Image;
    [SerializeField, Header("�󂢂���")]
    private Sprite p1OpenImage;
    [SerializeField, Header("�����グ���[�V�������̕�")]
    private Sprite p1LiftImage;
    [SerializeField, Header("���S���̕�")]
    private Sprite p1DeathImage;

    //�v���C���[2
    [SerializeField, Header("��")]
    private Sprite p2Image;
    [SerializeField, Header("�����グ���[�V�������̌�")]
    private Sprite p2LiftImage;
    [SerializeField, Header("���S���̌�")]
    private Sprite p2DeathImage;

    private Animator anim;//�A�j���[�^�[

    // Start is called before the first frame update
    void Start()
    {
        //�e�I�u�W�F�N�g�̖��O���擾
        parentObjectName = transform.parent.name;

        //�v���C���[�ɂ���ăC���X�g��ς���
        if (parentObjectName == "Player1")
        {
            GetComponent<SpriteRenderer>().sprite = p1Image;
        }
        if (parentObjectName == "Player2")
        {
            GetComponent<SpriteRenderer>().sprite = p2Image;
        }
        if (parentObjectName == "CopyKey")
        {
            GetComponent<SpriteRenderer>().sprite = p2Image;
        }

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //���C���X�g
        if (parentObjectName == "Player1")
        {
            //���S���̉摜
            if(ManagerAccessor.Instance.dataManager.isDeth)
            {
                //��ɓ��������ق��̃v���C���[�����S���̉摜�ɕύX
                if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1DeathImage;
                    anim.SetBool("isMove", false);//�A�j���[�V�������~�߂�
                }
            }
            else
            {
                //�v���C���[�̈ړ����������ɉ����ăv���C���[�̌�����ς���
                if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().imageleft)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                //�󔠃I�[�v���摜
                if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_boxopenimage)
                {
                    GetComponent<SpriteRenderer>().sprite = p1OpenImage;
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = p1Image;
                }

                //�u���b�N�����グ�摜
                if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_liftimage)
                {
                    GetComponent<SpriteRenderer>().sprite = p1LiftImage;
                }

                //�u���b�N���~�낵�����i���̉摜�ɖ߂��j
                if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_unloadimage)
                {
                    GetComponent<SpriteRenderer>().sprite = p1Image;
                }


                //�A�j���[�V�������Đ�
                if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().animplay
                && !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_boxopenimage
                && !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().change_liftimage)
                {
                    anim.SetBool("isMove", true);
                }
                else
                {
                    anim.SetBool("isMove", false);
                }

                //�W�����v���̓A�j���[�V�������f
                if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().bjump)
                {
                    anim.SetBool("isMove", false);
                }
            }
        }
        //���C���X�g
        else if (parentObjectName == "Player2")
        {
            //���S���̉摜
            if (ManagerAccessor.Instance.dataManager.isDeth)
            {
                //��ɓ��������ق��̃v���C���[�����S���̉摜�ɕύX
                if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player2")
                {
                    GetComponent<SpriteRenderer>().sprite = p2DeathImage;
                    anim.SetBool("isMove", false);//�A�j���[�V�������~�߂�
                }
                  
            }
            else
            {
                //�v���C���[�̈ړ����������ɉ����ăv���C���[�̌�����ς���
                if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().imageleft)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                //�u���b�N�����グ�摜
                if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_liftimage)
                {
                    GetComponent<SpriteRenderer>().sprite = p2LiftImage;
                }

                //�u���b�N���~�낵�����i���̉摜�ɖ߂��j
                if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_unloadimage)
                {
                    GetComponent<SpriteRenderer>().sprite = p2Image;
                }

                //�A�j���[�V�������Đ�
                if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().animplay
                && !ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_liftimage)
                {
                    anim.SetBool("isMove", true);
                }
                else
                {
                    anim.SetBool("isMove", false);
                }

                //�W�����v���̓A�j���[�V�������f
                if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().bjump)
                {
                    anim.SetBool("isMove", false);
                }
            }
        }
    }
}
