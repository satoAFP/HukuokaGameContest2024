using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    
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

    private bool left1P = false;//�������Ɉړ������Ƃ��̃t���O(1P�p�j
    private bool left2P = false;//�������Ɉړ������Ƃ��̃t���O(2P�p�j

    private bool B_instantiatefirst = true;//�A���ŃA�C�e���𐶐������Ȃ�(�j

    private bool CK_instantiatefirst = true;//�A���ŃA�C�e���𐶐������Ȃ�(���j

    private bool firstboxopen = true;//���̕���t���O���L����x��������

    private bool firstLR_1P = true;//���E�ړ���x�����������s��(1P�p�j
    private bool firstLR_2P = true;//���E�ړ���x�����������s��(2P�p�j

    private bool firstchange_boximage = true;//��x���������J���t���O���L��������

    private bool firstmovelock = true;//��x�����ړ��������s��

    [System.NonSerialized] public bool boxopen = false;//���̊J��������t���O

    [System.NonSerialized] public bool cursorlock = true;//UI�J�[�\���̈ړ��𐧌�����

    [System.NonSerialized] public string choicecursor;//UI�J�[�\�������ݑI�����Ă��鐶���\�A�C�e��

    [System.NonSerialized] public bool generatestop = false;//�����𐧌䂷��

    [System.NonSerialized] public bool keymovelock = false;//�����������̈ړ��𐧌�]


    [System.NonSerialized] public bool change_boxopenimage = false;//�v���C���[�摜�𔠂��󂯂�摜�ɕύX
    [System.NonSerialized] public bool change_liftimage = false;//�v���C���[�̉摜���u���b�N�������グ���Ƃ��̉摜�ɕύX
    [System.NonSerialized] public bool change_unloadimage = false;//�u���b�N�����낵�����v���C���[�̉摜�����ɖ߂�

    [System.NonSerialized] public bool animplay = false;//�A�j���[�V�������Đ�
     private bool firstanimplay = true;

    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;

    //�e�v���C���[�̍��W
    private Vector2 p1pos;
    private Vector2 p2pos;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    [System.NonSerialized] public bool bjump;//�A���ŃW�����v�����Ȃ��t���O

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
           // GetComponent<SpriteRenderer>().sprite = p1Image;
            ManagerAccessor.Instance.dataManager.player1 = gameObject;
        }
        if (gameObject.name == "Player2")
        {
            //GetComponent<SpriteRenderer>().sprite = p2Image;
            ManagerAccessor.Instance.dataManager.player2 = gameObject;
        }
        if (gameObject.name == "CopyKey")
        {
            //GetComponent<SpriteRenderer>().sprite = p2Image;
            ManagerAccessor.Instance.dataManager.copyKey = gameObject;
        }

        test_net = new Test_net();//�X�N���v�g��ϐ��Ɋi�[
    }
    void FixedUpdate()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        if (firstLR_1P)
        {
            //�v���C���[�̍��E�̌�����ς���
            if (datamanager.isOwnerInputKey_C_L_LEFT && !movelock)
            {
                Debug.Log("����������");
                left1P = true;
                firstLR_1P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
            else if (datamanager.isOwnerInputKey_C_L_RIGHT && !movelock)
            {
                Debug.Log("�E��������");
                left1P = false;
                firstLR_1P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
 
        }

        if (firstLR_2P)
        {
            if (datamanager.isClientInputKey_C_L_LEFT)
            {
                Debug.Log("2P����������");
                left2P = true;
                firstLR_2P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
            else if (datamanager.isClientInputKey_C_L_RIGHT)
            {
                Debug.Log("2P�E��������");
                left2P = false;
                firstLR_2P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
        }

          

        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            //�����グ�Ă��Ȃ��Ƃ��͕��ʂɈړ�������
            if (!islift)
            {
                //��ԏ�Ԃɓ����Ă��Ȃ���
                if (!isFly)
                {
                    Move();//�ړ�������ON
                }

                //�󔠂��󂢂Ă��Ȃ��Ƃ��͒ʏ�̉摜�ɖ߂�
                if (!change_boxopenimage && !movelock)
                {
                   // Debug.Log("ashidaka");
                    photonView.RPC(nameof(RpcChangeUnloadImage), RpcTarget.All);
                }

                distanceFirst = true;
            }
            else
            {
                //�e�v���C���[�������グ�C���X�g�ɕύX
                photonView.RPC(nameof(RpcChangeLiftImage), RpcTarget.All);

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

            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�v���C���[1�i���j�̈ړ�����������Ă���Ƃ��i�����󂢂Ă��鎞�j
                if(movelock)
                {
                    //�����A�C�e�����}�b�v��ɂȂ��Ƃ��̂ݔ������i�ړ����������j
                    if (currentBoardObject == null &&
                         currentCopyKeyObject == null)
                    {
                        if(firstboxopen)
                        {
                            //boxopen�֐������L����
                           // Debug.Log("firstboxopen�������Ă�");
                            photonView.RPC(nameof(RpcShareBoxOpen), RpcTarget.All,true);
                            firstboxopen = false;
                        }
                       
                    }
                    else
                    {
                        if (!firstboxopen)
                        {
                            //boxopen�֐������L����
                            photonView.RPC(nameof(RpcShareBoxOpen), RpcTarget.All, false);
                            firstboxopen = true;
                        }
                           
                    }

                    //�R���g���[���[�̉��{�^�����������Ƃ��������
                    if (datamanager.isOwnerInputKey_C_D_DOWN
                         && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
                    {
                        //������Ĉړ����b�N������
                        if (gameObject.name == "Player1" && boxopen)
                        {
                            //Debug.Log("���؂�");
                            change_boxopenimage = false;//��������摜�ɂ���
                            cursorlock = true;//�J�[�\���ړ����~�߂�
                            if(!firstmovelock)
                            {
                                photonView.RPC(nameof(RpcShareMoveLock), RpcTarget.All, false);//���̈ړ��̐�������
                                firstmovelock = true;
                            }
                            //movelock = false;
                            GetComponent<PlayerGetHitObjTagManagement>().isMotion = true;//���̎���̔�����Ƃ�̂��ĊJ
                            firstboxopen = true;//boxopen�t���O���L�ĉ�
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
                               // movelock = true;
                                //Debug.Log("����");

                                //��Ɍ�����������Ă����ꍇ
                                if (currentCopyKeyObject!=null)
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
                                //movelock = true;

                                //�R�s�[���o�����t���O
                                ManagerAccessor.Instance.dataManager.isAppearCopyKey = true;
                               // Debug.Log("������");

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

            if (movelock)
            {
                //�R���g���[���[�̉��{�^�����������Ƃ����������f�i���葤�j
                if (datamanager.isOwnerInputKey_CA)
                {

                    //�����ɏ�{�^���������Ă��Ȃ��Ƃ��͉摜�����ɖ߂�
                    if (gameObject.name == "Player1" && boxopen)
                    {
                       // Debug.Log("���؂�22");
                        //GetComponent<SpriteRenderer>().sprite = p1Image;
                        //change_boxopenimage = false;//��������摜�ɂ���
                        //boxopen = false;
                    }
                }
            }
        }

       

        //�e�v���C���[�̌��ݍ��W���擾
        p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;

        if (ManagerAccessor.Instance.dataManager.player2 != null)
            p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;
      
        //���ƌ��̓�_�ԋ���������Ĉ��̒l�Ȃ甠�I�[�v���\
        if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
        {
            //��{�^���̓��������Ŕ��I�[�v��
            if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP
                && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
            {
                //Debug.Log("��L�[������");
                //�󔠂̃v���C���[�̎��A�󂢂Ă��锠�̃C���X�g�ɕύX
                if (gameObject.name == "Player1")
                {
                    if(firstchange_boximage)
                    {
                        photonView.RPC(nameof(RpcChangeBoxOpenImage), RpcTarget.All);//�����󂯂�C���X�g�ύX�t���O�𑗐M
                        firstchange_boximage = false;
                    }
                   
                    //movelock = true;//���̈ړ��𐧌�
                    if(firstmovelock)
                    {
                        photonView.RPC(nameof(RpcShareMoveLock), RpcTarget.All, true);//���̈ړ��𐧌�
                        firstmovelock = false;
                    }
                   

                    cursorlock = false;//UI�J�[�\���ړ�������
                    GetComponent<PlayerGetHitObjTagManagement>().isMotion = false;//���̎���̔�����Ƃ�̂���߂�
                }

            }
            else
            {
                firstchange_boximage = true;
            }
        }

    }




    private void Move()//�ړ������i�v�Z�����j
    {
        //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);

       // Debug.Log("�ړ���"+inputDirection.x);

        if(inputDirection.x == 0)
        {
            //���݃A�j���[�V�������Đ����Ă��鎞
            if(!firstanimplay)
            {
                photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All);
              //  Debug.Log("steam");
            }
           
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
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            //�ړ����b�N���������Ă��Ȃ���Έړ�
            if(!movelock)
            {
                if (firstanimplay)
                {
                    Debug.Log("�A�j�����M");
                    photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
                    firstanimplay = false;
                }

                //Debug.Log("�X�e�B�b�N�������Ĉړ����Ă���");
                //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
                inputDirection = context.ReadValue<Vector2>();
            }

        }
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {
        //�A�����b�N�{�^���A���P�b�g���N�����łȂ���
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock && !isFly) 
        {

            photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All);//�W�����v���Ă��鎞�͈ړ��A�j���[�V�������~�߂�

            //Input System����W�����v�̓��͂����������ɌĂ΂��
            //�A���ŃW�����v�ł��Ȃ��悤�ɂ���
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
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


    [PunRPC]
    //boxopen�ϐ������L����
    private void RpcShareBoxOpen(bool data)
    {
        boxopen = data;
    }

    [PunRPC]
    //movelock�ϐ������L����
    private void RpcShareMoveLock(bool data)
    {
        movelock = data;
    }

    [PunRPC]
    private void RpcChangeLiftImage()
    {
        //�v���C���[�������グ���̃C���X�g�ɕύX
        if (gameObject.name == "Player1")
        {
            Debug.Log("QQQP1�����グ�摜");
            change_unloadimage = false;//�ʏ�摜���玝���グ�摜��
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p1LiftImage;

        }
        else if (gameObject.name == "Player2")
        {
            Debug.Log("QQQP2�����グ�摜");
            change_unloadimage = false;//�ʏ�摜���玝���グ�摜��
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p2LiftImage;
        }
    }

    [PunRPC]
    private void RpcChangeBoxOpenImage()
    {
        change_unloadimage = false;//������false�ɂ��Ȃ��Ɣ����󂭃C���X�g�ɕς��Ȃ��̂Œ���
        change_boxopenimage = true;//���v���C���[�̉摜�ύX
    }


    [PunRPC]
    private void RpcChangeUnloadImage()
    {
        //�v���C���[���u���b�N���~�낵���Ƃ��C���X�g�ύX
        if (gameObject.name == "Player1")
        {
           // Debug.Log("P1�~�낷�摜");
            change_liftimage = false;//�����グ�摜���猳�̉摜�ɖ߂�
            change_unloadimage = true;
        }
        else if (gameObject.name == "Player2")
        {
          //  Debug.Log("P2�~�낷�摜");
            change_liftimage = false;//�����グ�摜���猳�̉摜�ɖ߂�
            change_unloadimage = true;
        }
    }

    [PunRPC]
    private void RpcMoveLeftandRight()
    {
        //�v���C���[�����E�Ɉړ����������̕����ɑΉ����ăv���C���[�̌�����ς���
        if (gameObject.name == "Player1")
        {
            if (left1P)
            {
                Debug.Log("player1�̍�");
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                firstLR_1P = true;
            }
            else
            {
                 Debug.Log("player1�̉E");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                firstLR_1P = true;
            }

        }
        else if (gameObject.name == "Player2")
        {
            if (left2P)
            {
                //Debug.Log("player2�̍�");
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                firstLR_2P = true;
            }
            else
            {
                //Debug.Log("player2�̉E");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                firstLR_2P = true;
            }
        }
    }

    [PunRPC]
    private void RpcMoveAnimPlay()
    {
       // Debug.Log("�A�j���Đ�");
        animplay = true;
    }

    [PunRPC]
    private void RpcMoveAnimStop()
    {
        //Debug.Log("�A�j��stop");
        animplay = false;
        firstanimplay = true;
    }
}
