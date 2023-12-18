using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private int ColorChangeframe = 0;//�F��ύX�����鎞�Ԃ��v��

    private int Change_Color = 1;//�����ɂ���ĐF��ύX������

    [SerializeField, Header("�_�ł̊Ԋu")] private int blinkingtime;

    // Start is called before the first frame update
    void Start()
    {
        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[

        GetComponent<Image>().color = Color.black;//�����J�[�\���J���[
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;


        if (ManagerAccessor.Instance.dataManager.player1 != null)
        {
            //�����J���Ă��鎞�J�[�\���ړ�������
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
            {
                ColorChangeframe++;

                Debug.Log("ColorChangeframe" + ColorChangeframe);

                //���b���x�ŃJ�[�\���̐F��ς���
                if(ColorChangeframe >= blinkingtime)
                {
                    CursorColorChange();
                }

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



    private void CursorColorChange()
    {
        if (Change_Color == 1)
        {
            Debug.Log("��");
            GetComponent<Image>().color = Color.red;//��莞�ԂŃJ�[�\����Ԃɂ���
            ColorChangeframe = 0;
            Change_Color = 2;
        }
        else if (Change_Color == 2)
        {
            Debug.Log("��");
            GetComponent<Image>().color = Color.black;//��莞�ԂŃJ�[�\�������ɂ���
            ColorChangeframe = 0;
            Change_Color = 1;
        }
    }
}
