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
    //�v���C���[2
    [SerializeField, Header("��")]
    private Sprite p2Image;
    [SerializeField, Header("�����グ���[�V�������̌�")]
    private Sprite p2LiftImage;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //���C���X�g
        if (parentObjectName == "Player1")
        {
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
            else
            {
               // GetComponent<SpriteRenderer>().sprite = p1Image;
            }
        }
        //���C���X�g
        else if (parentObjectName == "Player2")
        {
            //�u���b�N�����グ�摜
            if (ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().change_liftimage)
            {
                GetComponent<SpriteRenderer>().sprite = p2LiftImage;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = p2Image;
            }
        }


    }
}
