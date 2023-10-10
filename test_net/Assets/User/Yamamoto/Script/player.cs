using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�W�����v���x")]
    private float jumpSpeed;

    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    void Start()
    {
        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        //���O��ID��ݒ�
        gameObject.name = "Player" + photonView.OwnerActorNr;

        // �f�o�C�X�ꗗ���擾
        foreach (var device in InputSystem.devices)
        {
            // �f�o�C�X�������O�o��
            Debug.Log(device.name);
        }

    }
    void Update()
    {
        Move();//�ړ�������ON
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
          
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
        if (!context.performed)
        {
            return;
        }

        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
