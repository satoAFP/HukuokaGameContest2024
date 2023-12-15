using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class UICursor : MonoBehaviourPunCallbacks
{
    private Test_net test_net;//inputsystem���X�N���v�g�ŌĂяo��

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�̃A�C�R��")]
    private GameObject BoardIcon;

    [SerializeField, Header("�R�s�[�L�[�̃A�C�R��")]
    private GameObject CopyKeyIcon;

    private int LRmove = 0;//1:�E�@2:��

    private bool movestart = false;//�ړ������𔻒f����

    //�J�[�\���̐F�����߂�
    [SerializeField] private Color color_black;
    [SerializeField] private Color color_red;

    private int ColorChangeframe = 0;//�F��ύX�����鎞�Ԃ��v��

    private int Change_Color = 1;

    // Start is called before the first frame update
    void Start()
    {
        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        ColorChangeframe++;

        if(ColorChangeframe <= 60 && Change_Color==1)
        {
            GetComponent<SpriteRenderer>().color = color_red;//��莞�ԂŃJ�[�\����Ԃɂ���
            ColorChangeframe = 0;
            Change_Color = 2;
        }
        else if (ColorChangeframe <= 60 && Change_Color == 2)
        {
            GetComponent<SpriteRenderer>().color = color_black;//��莞�ԂŃJ�[�\�������ɂ���
            ColorChangeframe = 0;
            Change_Color = 1;
        }

        if (ManagerAccessor.Instance.dataManager.player1 != null)
        {
            //�����J���Ă��鎞�J�[�\���ړ�������
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
            {
                //�����ꂽ�{�^���̍��E�ŃJ�[�\���̈ړ��ʒu�����߂�
                if (datamanager.isOwnerInputKey_C_D_RIGHT && !movestart)
                {
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "None";
                    movestart = true;
                    LRmove = 1;//�E�ɃJ�[�\���ړ�
                }

                if (datamanager.isOwnerInputKey_C_D_LEFT && !movestart)
                {
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "None";
                    movestart = true;
                    LRmove = 2;//���ɃJ�[�\���ړ�
                }
            }
        }

        if(movestart)
        {
            if(LRmove==1)
            {
                transform.position = Vector2.MoveTowards(transform.position, CopyKeyIcon.transform.position, moveSpeed * Time.deltaTime);
                if (transform.position == CopyKeyIcon.transform.position)
                {
                    movestart = false;
                    LRmove = 0;//�ړ��I��
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "CopyKey";//���݃J�[�\�����I�����Ă���A�C�e����
                }
            }
            else if(LRmove==2)
            {
                transform.position = Vector2.MoveTowards(transform.position, BoardIcon.transform.position, moveSpeed * Time.deltaTime);
                if (transform.position == BoardIcon.transform.position)
                {
                    movestart = false;
                    LRmove = 0;//�ړ��I��
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor = "Board";//���݃J�[�\�����I�����Ă���A�C�e����
                }
            }
            
        }
    }
}
