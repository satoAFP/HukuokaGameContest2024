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

    [SerializeField, Header("���I�u�W�F�N�g")]
    private GameObject copykeyobj;

    [SerializeField]
    private GameObject currentBoardObject;// ���݂̐������ꂽ�I�u�W�F�N�g

    [SerializeField]
    private GameObject currentCopyKeyObject;// ���݂̐������ꂽ���I�u�W�F�N�g

    private bool movelock = false;//�ړ��������~������

    private bool left = false;//�������Ɉړ������Ƃ��̃t���O

    private bool B_instantiatefirst = true;//�A���ŃA�C�e���𐶐������Ȃ�(�j

    private bool CK_instantiatefirst = true;//�A���ŃA�C�e���𐶐������Ȃ�(���j

    [System.NonSerialized] public bool boxopen = false;//���̊J��������t���O

    [System.NonSerialized] public bool cursorlock = true;//UI�J�[�\���̈ړ��𐧌�����

    [System.NonSerialized] public string choicecursor;//UI�J�[�\�������ݑI�����Ă��鐶���\�A�C�e��

    [System.NonSerialized] public bool generatestop = false;//�����𐧌䂷��

    [System.NonSerialized] public bool keymovelock = false;//�����������̈ړ��𐧌�

    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;

    //�e�v���C���[�̍��W
    private Vector2 p1pos;
    private Vector2 p2pos;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    private bool bjump;//�A���ŃW�����v�����Ȃ��t���O

    private Test_net test_net;//inputsystem���X�N���v�g�ŌĂяo��

    [System.NonSerialized] public bool islift = false;//�����グ�t���O

    [System.NonSerialized] public bool isliftfirst = true;//�����グ�t���O�̏�Ԃ𑗐M����Ƃ���񂵂����M���Ȃ�����

    [System.NonSerialized] public bool isFly = false;//���P�b�g��荞�݃t���O�t���O

    //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
    private bool distanceFirst = true;
    private Vector3 dis = Vector3.zero;

    void Start()
    {

        choicecursor = "Board";//�����\�A�C�e��������

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

      
    }
    void FixedUpdate()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //Debug.Log("isOwnerHitDown=" + ManagerAccessor.Instance.dataManager.isOwnerHitDown);
        //Debug.Log("isClientHitDown=" + ManagerAccessor.Instance.dataManager.isClientHitDown);


        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            ////1P�̉�ʂ�2P�̏��X�V
            //if (PhotonNetwork.LocalPlayer.IsMasterClient)
            //    if (ManagerAccessor.Instance.dataManager.player2 != null)
            //        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = islift;

            ////1P�̉�ʂ�2P�̏��X�V
            //if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            //    if (ManagerAccessor.Instance.dataManager.player1 != null)
            //        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = islift;

            //�����グ�Ă��Ȃ��Ƃ��͕��ʂɈړ�������
            if (!islift)
            {
                //��ԏ�Ԃɓ����Ă��Ȃ���
                if (!isFly)
                {
                    Move();//�ړ�������ON
                }
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
                //�v���C���[1�i���j�̈ړ�����������Ă���Ƃ��i�����󂢂Ă��鎞�j
                if(movelock)
                {
                    //�����A�C�e�����}�b�v��ɂȂ��Ƃ��̂ݔ������i�ړ����������j
                    if (currentBoardObject == null &&
                         currentCopyKeyObject == null)
                    {
                        boxopen = true;
                    }
                    else
                    {
                        boxopen = false;
                    }

                        //�R���g���[���[�̉��{�^�����������Ƃ��������
                        if (datamanager.isOwnerInputKey_CA)
                    {
                        //������Ĉړ����b�N������
                        if (gameObject.name == "Player1" && boxopen)
                        {
                            Debug.Log("���؂�");
                            GetComponent<SpriteRenderer>().sprite = p1Image;
                            cursorlock = true;//�J�[�\���ړ����~�߂�
                            movelock = false;
                        }
                    }
                  
                    //�Q�[���p�b�h�E�{�^���ŃA�C�e������
                    //��
                    if (datamanager.isOwnerInputKey_CB &&
                        choicecursor== "Board")
                    {
                        if (B_instantiatefirst)
                        {
                            if (currentBoardObject == null)
                            {
                                currentBoardObject = PhotonNetwork.Instantiate("Board", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                                movelock = true;

                                //��Ɍ�����������Ă����ꍇ
                                if(currentCopyKeyObject!=null)
                                {
                                    keymovelock = true;//�ɃI�u�W�F�N�g�ړ��̎哱����n��
                                }
                                else
                                {
                                    generatestop = true;//����̏ꍇ�A�̈ړ����I���܂Ō������������Ȃ�
                                }
                              
                              //  Debug.Log("p1������");
                            }
                            B_instantiatefirst = false;
                        }

                    }
                    else
                    {
                        B_instantiatefirst = true;
                    }
                    //��
                    if (datamanager.isOwnerInputKey_CB &&
                       choicecursor == "CopyKey")
                    {
                        if (CK_instantiatefirst && !generatestop)
                        {
                            if (currentCopyKeyObject == null)
                            {
                                currentCopyKeyObject = PhotonNetwork.Instantiate("CopyKey", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                                movelock = true;

                            }
                            CK_instantiatefirst = false;
                        }

                    }
                    else
                    {
                        CK_instantiatefirst = true;
                    }
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
                if (gameObject.name == "Player1"&& boxopen)
                {
                   // Debug.Log("���؂�22");
                    GetComponent<SpriteRenderer>().sprite = p1Image;
                    boxopen = false;
                }
            }
         
        }

        //�e�v���C���[�̌��ݍ��W���擾
        p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;
        //Debug.Log("p1���ݒn=" + p1pos);
        if (ManagerAccessor.Instance.dataManager.player2 != null)
            p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;
        //Debug.Log("p2���ݒn=" + p2pos);

        // Debug.Log(Mathf.Abs(p1pos.x - p2pos.x));


        //���ƌ��̓�_�ԋ���������Ĉ��̒l�Ȃ甠�I�[�v���\
        if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
        {
            //��{�^���̓��������Ŕ��I�[�v��
            if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP)
            {
                //Debug.Log("��L�[������");
                //�󔠂̃v���C���[�̎��A�󂢂Ă��锠�̃C���X�g�ɕύX
                if (gameObject.name == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1OpenImage;
                    movelock = true;//���̈ړ��𐧌�
                    cursorlock = false;//UI�J�[�\���ړ�������
                }

            }
        }

    }




    private void Move()//�ړ������i�v�Z�����j
    {
        //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        //Debug.Log(inputDirection.x);
        //�ړ������ɂ���ĉ摜�̌�����ς���
        if(inputDirection.x < 0)
        {
           // left = true;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            //left = false;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
        //�A�����b�N�{�^���A���P�b�g���N�����łȂ���
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock && !isFly) 
        {
            //Input System����W�����v�̓��͂����������ɌĂ΂��
            //�A���ŃW�����v�ł��Ȃ��悤�ɂ���
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (!context.performed || !ManagerAccessor.Instance.dataManager.isOwnerHitDown)
                {
                    return;
                }
            }
            else
            {
                if (!context.performed || !ManagerAccessor.Instance.dataManager.isClientHitDown)
                {
                    return;
                }
            }
           

            //���삪�������Ȃ����߂̐ݒ�
            if (photonView.IsMine)
            {
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                bjump = true;//��x�W�����v�����璅�n����܂ŃW�����v�ł��Ȃ�����
            }
        }
    }
}
