using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviourPunCallbacks
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
    }
    void Update()
    {
        Move();//�ړ�������ON

    }

    private void Move()//�ړ������i�v�Z�����j
    {

        //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
        inputDirection = context.ReadValue<Vector2>();
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        //Input System����W�����v�̓��͂����������ɌĂ΂��
        if (!context.performed)
        {
            return;
        }

        rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
    }
}
