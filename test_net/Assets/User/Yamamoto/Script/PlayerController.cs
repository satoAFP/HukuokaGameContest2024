using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("��")]
    private Sprite p1Image;

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

    void Start()
    {
        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        //���O��ID��ݒ�
        gameObject.name = "Player" + photonView.OwnerActorNr;

        if (gameObject.name == "Player1")
            GetComponent<SpriteRenderer>().sprite = p1Image;
        if (gameObject.name == "Player2")
            GetComponent<SpriteRenderer>().sprite = p2Image;

        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[
        //test_net.Enable();

        // �f�o�C�X�ꗗ���擾
        foreach (var device in InputSystem.devices)
        {
            // �f�o�C�X�������O�o��
            Debug.Log(device.name);
        }

    }
    void Update()
    {
        
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {

            Move();//�ړ�������ON
        }
    }

    private void Move()//�ړ������i�v�Z�����j
    {
        //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
    }


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
        if (photonView.IsMine)
        {
            Debug.Log("�A�N�V����");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�����܂��͒��n�o������̂ɏ���Ă��鎞�A�ăW�����v�\�ɂ���
        if (collision.gameObject.tag == "Floor")
        {
            bjump = false;

        }
    }
}
