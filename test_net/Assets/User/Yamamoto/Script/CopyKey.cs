using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("���S���̃R�s�[�L�[")]
    private Sprite DeathImage;

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

    private bool copykey_death = false;//�R�s�[�L�[�����S�������̃t���O
    private bool firstdeathjump = true;//���S���̃m�b�N�o�b�N�W�����v����񂾂�������
    private float knockbacktime = 1.0f;//�m�b�N�o�b�N����Ƃ��̂w���W���ړ�
    private float timer = 0f;//���Ԃ��J�E���g

    //�u���b�N�����グ�Ɏg���ϐ�
    [System.NonSerialized] public bool islift = false;//�����グ�t���O
    [System.NonSerialized] public bool isliftfirst = true;//�����グ�t���O�̏�Ԃ𑗐M����Ƃ���񂵂����M���Ȃ�����                                                      
    private bool distanceFirst = true;//���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
    private Vector3 dis = Vector3.zero;

    private bool firstDeathEreaHit = true;//��x�����Q�[���I�[�o�[�G���A�ɓ�����������������

    // Start is called before the first frame update
    void Start()
    {
        //���O��ݒ�
        gameObject.name = "CopyKey";

        //�S�̂���R�s�[���擾
        ManagerAccessor.Instance.dataManager.copyKey = gameObject;

        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        ManagerAccessor.Instance.dataManager.isAppearCopyKey = true;

        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[

        holdtime = collecttime;//�������J�E���g���Ԃ�������
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //�v���C���[���Q�[���I�[�o�[�ɂȂ��Ă��Ȃ���΃R�s�[�L�[�̊�{���싖��
        if (!ManagerAccessor.Instance.dataManager.isDeth || !copykey_death)
        {
            //�J�[�\��������I��ł���Ƃ�����\
            if ( !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
                &&ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")
            {
                //���삪�������Ȃ����߂̐ݒ�
                if (photonView.IsMine)
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
                                    if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                                        dis = datamanager.player1.transform.position - gameObject.transform.position;
                                    else
                                        dis = datamanager.copyKey.transform.position - gameObject.transform.position;
                                    distanceFirst = false;
                                }

                                //2P��1P�ɒǏ]����悤�ɂ���
                                if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                                    transform.position = datamanager.player1.transform.position - dis;
                                else
                                    transform.position = datamanager.copyKey.transform.position - dis;
                            }
                        }
                    }
                }

            }

            //�Q�[���p�b�h���{�^���Œu���Ȃ���
            if (datamanager.isOwnerInputKey_C_D_DOWN)
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
      
          
        if (copykey_death)
        {


            // �摜��؂�ւ��܂�
            GetComponent<SpriteRenderer>().sprite = DeathImage;

            timer += Time.deltaTime;//���Ԍv��

            //�m�b�N�o�b�N����
            //�����̓m�b�N�o�b�N�����Ƃ���x���˂鏈��
            if (firstdeathjump)
            {
                Debug.Log("copykey_deathjump");
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                firstdeathjump = false;
            }

            //������1�b���炢���Ɉړ����鏈��
            if (timer <= knockbacktime)
            {
                Debug.Log("copykey_deathmove");
                rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
            }
            else if (timer >= 2.5f)
            {
                Destroy(gameObject);//�O�̂��߂ɃR�s�[�L�[���폜

                //�R�s�[���o�����t���O
                ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                ManagerAccessor.Instance.dataManager.copyKey = null;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //���΃G���A�ɓ���ƃR�s�[�L�[���S�̏���
        if (collision.gameObject.tag == "DeathErea")
        {
            if(firstDeathEreaHit)
            {
                Debug.Log("�R�s�[�L�[������");
                photonView.RPC(nameof(RpcCopyKeyDeath), RpcTarget.All, true);
                firstDeathEreaHit = false;
            }
           
        }
    }

    //playerinput�ŋN��������֐�
    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
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


    [PunRPC]
    private void RpcCopyKeyDeath(bool data)
    {
        copykey_death = data;//copykey_death�ϐ������L����
    }
}


