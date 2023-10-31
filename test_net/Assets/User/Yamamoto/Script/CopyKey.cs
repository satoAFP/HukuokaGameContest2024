using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
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

    // Start is called before the first frame update
    void Start()
    {
        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        Move();//�ړ�������ON
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
        //1P�i�����j�ł̑��삵���󂯕t���Ȃ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("�R�s�[�L�[�ړ�");
            //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
          
        }

        inputDirection = context.ReadValue<Vector2>();
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {
        //1P�i�����j�ł̑��삵���󂯕t���Ȃ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //�A�����b�N�{�^�����N����
            if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
            {
                Debug.Log("�R�s�[�L�[�W�����v");
                //Input System����W�����v�̓��͂����������ɌĂ΂��
                if (!context.performed || bjump)
                {
                    return;
                }

      
            }
        }

        //Input System����W�����v�̓��͂����������ɌĂ΂��
        if (!context.performed || bjump)
        {
            return;
        }
        else
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            bjump = true;//��x�W�����v�����璅�n����܂ŃW�����v�ł��Ȃ�����
        }

      
    }
}


