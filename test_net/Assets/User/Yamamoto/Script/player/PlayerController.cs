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

    [SerializeField, Header("�I�u�W�F�N�g")]
    private GameObject boardobj;

    [SerializeField, Header("�A�C�e��������ԁi���60�łP�b�j")]
    private int collecttime;

    [SerializeField]
    private int holdtime;//�ݒ肵���A�C�e��������Ԃ�������

    [SerializeField]
    private GameObject currentObject;// ���݂̐������ꂽ�I�u�W�F�N�g

    private bool movelock = false;//�ړ��������~������

    private bool generate = false;//�I�u�W�F�N�g�����t���O

    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;

    //�e�v���C���[�̍��W
    private Vector2 p1pos;
    private Vector2 p2pos;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    private bool bjump;//�A���ŃW�����v�����Ȃ��t���O

    private Test_net test_net;//inputsystem���X�N���v�g�ŌĂяo��

    [System.NonSerialized]public bool islift = false;//�����グ�t���O

    [System.NonSerialized] public bool isliftfirst = true;//�����グ�t���O�̏�Ԃ𑗐M����Ƃ���񂵂����M���Ȃ�����

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

        holdtime = collecttime;//�ݒ肵���A�C�e��������Ԃ�������
    }
    void FixedUpdate()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            //1P�̉�ʂ�2P�̏��X�V
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                if (ManagerAccessor.Instance.dataManager.player2 != null)
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = islift;

           
            //�����グ�Ă��Ȃ��Ƃ��͕��ʂɈړ�������
            if (!islift)
            {
                Move();//�ړ�������ON

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

            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�R���g���[���[�̉��{�^�����������Ƃ����������f
                if (datamanager.isOwnerInputKey_CA && movelock)
                {
                    //Debug.Log("�����̓~");
                    //������Ĉړ����b�N������
                    if (gameObject.name == "Player1")
                    {
                        GetComponent<SpriteRenderer>().sprite = p1Image;
                        movelock = false;
                        // Debug.Log("�����̏H");
                    }
                }
                else if (datamanager.isOwnerInputKey_CB && movelock)
                {
                    if (currentObject == null && holdtime == collecttime)
                    {
                        //�������ŘA���Ő����ł��Ȃ��悤�ɂ���
                        if (holdtime == collecttime)
                        {
                            //currentObject = Instantiate(boardobj, new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                             currentObject = PhotonNetwork.Instantiate("Board", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                            // generate = true;
                            movelock = true;
                            Debug.Log("p1������");
                        }

                    }
                    else
                    {
                        holdtime--;//�������ŃA�C�e�����
                        if (holdtime <= 0)//����J�E���g��0�ɂȂ�Ɖ��
                        {
                            Destroy(currentObject);
                            currentObject = null;
                            // generate = false;

                        }
                    }

                }
                else
                {
                    holdtime = collecttime;//�{�^���𗣂��Ɖ���J�E���g���Z�b�g
                }
            }
 
        }
        else
        {
            //�����グ�Ă��鎞��2�v���C���[�������ړ���������͎��ړ�
            if ((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
               (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT))
            {
                if (islift)
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
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
            else
            {
                distanceFirst = true;
            }

            //�R���g���[���[�̉��{�^�����������Ƃ����������f�i���葤�j
            if (datamanager.isOwnerInputKey_CA &&�@movelock)
            {
                //�����ɏ�{�^���������Ă��Ȃ��Ƃ��͉摜�����ɖ߂�
                if (gameObject.name == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1Image;
                    movelock = false;

                }
            }
            //else if (datamanager.isOwnerInputKey_CB && movelock)
            //{
            //    if (gameObject.name == "Player1")
            //    {
            //        if (currentObject == null && holdtime == collecttime)
            //        {
            //            //�������ŘA���Ő����ł��Ȃ��悤�ɂ���
            //            if (holdtime == collecttime)
            //            {
            //                currentObject = Instantiate(boardobj, new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
            //                // generate = true;
            //                movelock = true;
            //                Debug.Log("p2������");
            //            }

            //        }
            //        else
            //        {
            //            holdtime--;//�������ŃA�C�e�����
            //            if (holdtime <= 0)//����J�E���g��0�ɂȂ�Ɖ��
            //            {
            //                Destroy(currentObject);
            //                currentObject = null;
            //                // generate = false;

            //            }
            //        }

            //    }
            //}
            //else
            //{
            //    holdtime = collecttime;//�{�^���𗣂��Ɖ���J�E���g���Z�b�g
            //}

        }

        //�e�v���C���[�̌��ݍ��W���擾
        p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;
        //Debug.Log("p1���ݒn=" + p1pos);
        if (ManagerAccessor.Instance.dataManager.player2 != null)
            p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;
        //Debug.Log("p2���ݒn=" + p2pos);

        // Debug.Log(Mathf.Abs(p1pos.x - p2pos.x));

        //�e�X�g�p

        //if (datamanager.isOwnerInputKey_CB)
        //{
        //    Debug.Log("������");

        //    if (gameObject.name == "Player1")
        //    {
                
        //        if(currentObject == null && holdtime==collecttime)
        //        {
        //            //�������ŘA���Ő����ł��Ȃ��悤�ɂ���
        //            if (holdtime == collecttime)
        //            {
        //                currentObject = Instantiate(boardobj, new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
        //                // generate = true;
        //                movelock = true;
        //                Debug.Log("�΂�");
        //            }
                 
        //        }
        //        else
        //        {
        //            holdtime--;//�������ŃA�C�e�����
        //            if (holdtime <= 0)//����J�E���g��0�ɂȂ�Ɖ��
        //            {
        //                Destroy(currentObject);
        //                currentObject = null;
        //               // generate = false;
                        
        //            }
        //        }
            
        //    }

        //}
        //else
        //{
        //    holdtime = collecttime;//�{�^���𗣂��Ɖ���J�E���g���Z�b�g
        //}

        //���ƌ��̓�_�ԋ���������Ĉ��̒l�Ȃ甠�I�[�v���\
        if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
        {
            Debug.Log("�����I�I�ׂ̔ӌ�сI�I");
            //��{�^���̓��������Ŕ��I�[�v��
            if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP)
            {
                Debug.Log("��L�[������");
                //�󔠂̃v���C���[�̎��A�󂢂Ă��锠�̃C���X�g�ɕύX
                if (gameObject.name == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1OpenImage;
                    movelock = true;//���̈ړ��𐧌�
                }

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
            //�ړ����b�N���������Ă��Ȃ���Έړ�
            if(!movelock)
            {
                Debug.Log("�X�e�B�b�N�������Ĉړ����Ă���");
                //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
                inputDirection = context.ReadValue<Vector2>();
            }

        }
        else
        {
            Debug.Log("���ʂł��ĂȂ�");
        }
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {
        //�A�����b�N�{�^�����N����
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock)
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
    }

    //���̊W��߂�
    public void OnBoxClose(InputAction.CallbackContext context)
    {
      
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
