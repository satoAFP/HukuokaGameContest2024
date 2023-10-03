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

        //if(gameObject.name== "Player2")
        //{
        //    GetComponent<PlayerInput>().enabled = false;
        //}
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
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
            inputDirection = context.ReadValue<Vector2>();
        }
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            //Input System����W�����v�̓��͂����������ɌĂ΂��
            if (!context.performed)
            {
                return;
            }

            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
