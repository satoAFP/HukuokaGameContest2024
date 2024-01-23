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

    [SerializeField, Header("�{�^���̃A�C�R��")]
    private GameObject ButtonIcon;

    private int LRmove = 0;//1:�E�@2:��

    private bool movestart = false;//�ړ������𔻒f����

    private int ColorChangeframe = 0;//�F��ύX�����鎞�Ԃ��v��

    private int Change_Color = 1;//�����ɂ���ĐF��ύX������

    [SerializeField, Header("�_�ł̊Ԋu")] private int blinkingtime;

    private bool firstcolor_change = true;//�A�C�R���̐F�����ɕς���
    private bool firstdefaultcolor_change = true;//�A�C�R���̐F�����ɖ߂�

    //�J�[�\���̐F��ݒ�ł���
    [SerializeField, Header("�J�[�\���J���[1")] private Color Type1;
    [SerializeField, Header("�J�[�\���J���[2")] private Color Type2;

    // Start is called before the first frame update
    void Start()
    {
        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[

        GetComponent<Image>().color = Type1;//�����J�[�\���J���[
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
        {
            if(firstdefaultcolor_change)
            {
                //�e�v���C���[�̃A�C�R�������̐F�ɖ߂�
                photonView.RPC(nameof(RpcIconColorChangeDefault), RpcTarget.All);
                firstdefaultcolor_change = false;
                firstcolor_change = true;//�܂������F�ɏo����悤�ɂ���
            }
        }
        else
        {
            if(firstcolor_change)
            {
                //�e�v���C���[�̃A�C�R���������J���[�ɕύX
                photonView.RPC(nameof(RpcIconColorChange), RpcTarget.All);
                firstcolor_change = false;
                firstdefaultcolor_change = true;//�܂��F�����ǂ���悤�ɂ���
            }
          
        }
           

        if (ManagerAccessor.Instance.dataManager.player1 != null)
        {
            //�����J���Ă��鎞�J�[�\���ړ�������
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock)
            {
                ColorChangeframe++;

               // Debug.Log("ColorChangeframe" + ColorChangeframe);

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
            else
            {
                ColorChangeframe = 0;//�W�������ĂȂ���΃J�[�\���̐F��ς��Ȃ�
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


    //�����ŃJ�[�\���̐F��ς���
    private void CursorColorChange()
    {
        if (Change_Color == 1)
        {
            //Debug.Log("��");
            GetComponent<Image>().color = Type2;//��莞�ԂŃJ�[�\����Ԃɂ���
            ColorChangeframe = 0;//�t���[���v�Z���Z�b�g
            Change_Color = 2;
        }
        else if (Change_Color == 2)
        {
            //Debug.Log("��");
            GetComponent<Image>().color = Type1;//��莞�ԂŃJ�[�\�������ɂ���
            ColorChangeframe = 0;//�t���[���v�Z���Z�b�g
            Change_Color = 1;
        }
    }

    [PunRPC]
    private void RpcIconColorChangeDefault()//�A�C�R���̐F��ς���
    {
        BoardIcon.GetComponent<Image>().color   = new Color32(255, 255, 255, 255);
        CopyKeyIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        ButtonIcon.GetComponent<Image>().color  = new Color32(255, 255, 255, 255);
    }

    [PunRPC]
    private void RpcIconColorChange()//�A�C�R���̐F��ς���
    {
        BoardIcon.GetComponent<Image>().color = new Color32(0, 0, 0, 192);
        CopyKeyIcon.GetComponent<Image>().color = new Color32(0, 0, 0, 192);
        ButtonIcon.GetComponent<Image>().color = new Color32(0, 0, 0, 192);
    }
}
