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

    [SerializeField, Header("��������ԁi���60�łP�b�j")]
    private int collecttime;

    [SerializeField]
    private int holdtime;//�ݒ肵���A�C�e��������Ԃ�������

    private Test_net test_net;//inputsystem���X�N���v�g�ŌĂяo��


    //�u���b�N�����グ�Ɏg���ϐ�
    [System.NonSerialized] public bool islift = false;//�����グ�t���O
    [System.NonSerialized] public bool isliftfirst = true;//�����グ�t���O�̏�Ԃ𑗐M����Ƃ���񂵂����M���Ȃ�����                                                      
    private bool distanceFirst = true;//���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
    private Vector3 dis = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //���O��ݒ�
        gameObject.name = "CopyKey";

        //�S�̂���R�s�[���擾
        ManagerAccessor.Instance.dataManager.copyKey = gameObject;

        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        

        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[

        holdtime = collecttime;//�������J�E���g���Ԃ�������
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //�J�[�\��������I��ł���Ƃ�����\
        if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
            && ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")
        {
            //�����グ�Ă��Ȃ��Ƃ��͕��ʂɈړ�������
            if (!islift)
            {
                Move();//�ړ�������ON
                distanceFirst = true;
            }
            else
            {
                //�����グ�Ă��鎞��2�v���C���[�������ړ���������͎��ړ�
                if ((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
                   (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT))
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        Move();
                    }
                    else
                    {
                        //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
                        if (distanceFirst)
                        {
                            //1P��2P�̍��W�̍����L��
                            dis = datamanager.player1.transform.position - datamanager.player2.transform.position;
                            distanceFirst = false;
                        }

                        //2P��1P�ɒǏ]����悤�ɂ���
                        transform.position = datamanager.player1.transform.position - dis;
                    }
                }
            }

            //�Q�[���p�b�h���{�^���Œu���Ȃ���
            if (datamanager.isOwnerInputKey_CA)
            {
                holdtime--;//�������J�E���g�_�E��

                //�Q�[���p�b�h���{�^���������ŉ��
                if (holdtime <= 0)//����J�E���g��0�ɂȂ�Ɖ��
                {
                    Destroy(gameObject);

                    //�R�s�[���o�����t���O
                    ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                    ManagerAccessor.Instance.dataManager.copyKey = null;
                }
            }
            else
            {
                holdtime = collecttime;//�������J�E���g���Z�b�g
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
        //1P�i�����j�ł̑��삵���󂯕t���Ȃ�
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
        //    Debug.Log("�R�s�[�L�[�ړ�");
        //}


        //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
        inputDirection = context.ReadValue<Vector2>();
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {

        if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
            && ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")//�J�[�\��������I�����Ă��鎞
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
                    else
                    {
                        rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                        bjump = true;//��x�W�����v�����璅�n����܂ŃW�����v�ł��Ȃ�����
                    }


                }
            }

            ////Input System����W�����v�̓��͂����������ɌĂ΂��
            //if (!context.performed || bjump)
            //{
            //    return;
            //}
            //else
            //{
            //    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            //    bjump = true;//��x�W�����v�����璅�n����܂ŃW�����v�ł��Ȃ�����
            //}

        }
    }
}


