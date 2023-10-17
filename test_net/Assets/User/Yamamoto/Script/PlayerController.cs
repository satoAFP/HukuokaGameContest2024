using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("��")]
    private Sprite p1Image;
    [SerializeField, Header("�󂢂���")]
    private Sprite p1OpenImage;

    [SerializeField, Header("��")]
    private Sprite p2Image;

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�W�����v���x")]
    private float jumpSpeed;

    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    private bool bjump;//�A���ŃW�����v�����Ȃ��t���O

    private Test_net test_net;//inputsystem���X�N���v�g�ŌĂяo��

    [System.NonSerialized]public bool islift = false;//�����グ�t���O

    //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
    private bool distanceFirst = true;
    private Vector3 dis = Vector3.zero;

    void Start()
    {
        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        //���O��ID��ݒ�
        gameObject.name = "Player" + photonView.OwnerActorNr;

        //�v���C���[�ɂ���ăC���X�g��ς��違�f�[�^�}�l�[�W���[�ݒ�
        if (gameObject.name == "Player1")
        {
            GetComponent<SpriteRenderer>().sprite = p1Image;
            ManagerAccessor.Instance.dataManager.player1 = ManagerAccessor.Instance.dataManager.GetPlyerObj("Player1");
        }
        if (gameObject.name == "Player2")
        {
            GetComponent<SpriteRenderer>().sprite = p2Image;
            ManagerAccessor.Instance.dataManager.player2 = ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2");
        }

        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[
        //test_net.Enable();

        // �f�o�C�X�ꗗ���擾
        //foreach (var device in InputSystem.devices)
        //{
        //    // �f�o�C�X�������O�o��
        //    Debug.Log(device.name);
        //}

    }
    void Update()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            //�����グ�Ă��Ȃ��Ƃ��͕��ʂɈړ�������
            if(!islift)
            {
                Move();//�ړ�������ON
                Debug.Log("�f�t�H���g");

                distanceFirst = true;
            }
            else
            {
                //�����グ�Ă��鎞��2�v���C���[�������ړ���������͎��ړ�
                if ((datamanager.isOwnerInputKey_C_L_RIGHT&& datamanager.isClientInputKey_C_L_RIGHT)||
                   (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT))
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        Move();
                        Debug.Log("����");
                    }
                    else
                    {
                        //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
                        if (distanceFirst)
                        {
                            dis = datamanager.player1.transform.position - datamanager.player2.transform.position;
                            distanceFirst = false;
                        }

                        transform.position = datamanager.player1.transform.position + dis;
                    }
                }
            }
        }
        else
        {
            //�����グ�Ă��鎞��2�v���C���[�������ړ���������͎��ړ�
            if (((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
               (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT)) &&
               islift && PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
                if (distanceFirst)
                {
                    dis = datamanager.player1.transform.position - datamanager.player2.transform.position;
                    distanceFirst = false;
                }

                transform.position = datamanager.player1.transform.position + dis;
            }
            else
            {
                distanceFirst = true;
            }
        }

        //��{�^���̓�������
        if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP)
        {
            Debug.Log("��L�[������");
            //�󔠂̃v���C���[�̎��A�󂢂Ă��锠�̃C���X�g�ɕύX
            if (gameObject.name == "Player1")
            {
                GetComponent<SpriteRenderer>().sprite = p1OpenImage;
            }

        }
        else
        {
            //�����ɏ�{�^���������Ă��Ȃ��Ƃ��͉摜�����ɖ߂�
            if (gameObject.name == "Player1")
            {
                GetComponent<SpriteRenderer>().sprite = p1Image;
            }
        }
    }


    private void Move()//�ړ������i�v�Z�����j
    {
        //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�����܂��͒��n�o������̂ɏ���Ă��鎞�A�ăW�����v�\�ɂ���
        if (collision.gameObject.tag == "Floor")
        {
            bjump = false;

        }
    }

    //playerinput�ŋN��������֐�
    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameObject.name == "Player2")
        {
            Debug.Log("�v���C���[2�F��");
        }


        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            Debug.Log("�X�e�B�b�N�������Ĉړ����Ă���");
            //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
            inputDirection = context.ReadValue<Vector2>();
        }
        else
        {
            Debug.Log("���ʂł��ĂȂ�");
        }
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {
        //Input System����W�����v�̓��͂����������ɌĂ΂��
        if (!context.performed || bjump)
        {
            return;
        }

        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            bjump = true;//��x�W�����v�����璅�n����܂ŃW�����v�ł��Ȃ�����
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        //���삪�������Ȃ����߂̐ݒ�
        //if (photonView.IsMine)
        //{
        //    Debug.Log("�A�N�V����");
        //}
    }

    //���I�[�v��
    public void OnOpenAction(InputAction.CallbackContext context)
    {
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            Debug.Log("���J����");
        }
    }
    
}
