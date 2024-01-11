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

    private DataManager datamanager;

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

    [System.NonSerialized] public bool imageleft = false;//�摜���������ɂ���t���O
    
    [System.NonSerialized] public bool animplay = false;//�A�j���[�V�������Đ�

    private bool firstanimplay = true;//�����A�j���N���������Ȃ��t���O

    private bool firststickprocess = true;//�X�e�B�b�N�𓮂������Ƃ��̈ړ���������񂸂s��

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


    private bool firstdeathjump = true;//���S���̃m�b�N�o�b�N�W�����v����񂾂�������
    private float knockbacktime = 1.0f;//�m�b�N�o�b�N����Ƃ��̂w���W���ړ�
    private float timer = 0f;//���Ԃ��J�E���g
    private bool firstdeathknockback = true;
    private bool knockbackmove = true;

    [System.NonSerialized] public bool knockback_finish = false;//�m�b�N�o�b�N�I��

    [System.NonSerialized] public bool copykeydelete = false;//�R�s�[�L�[����ɓ������ď������Ƃ�


    private AudioSource audiosource = null;//�I�[�f�B�I�\�[�X
    [SerializeField, Header("�v���C���[�W��SE")]   private AudioClip[] StandardSE;
    [SerializeField, Header("���v���C���[��pSE")] private AudioClip[] BoxplayerSE;
    private bool oneSE = true;
    private int walkseframe = 0;//se�Đ����ɑ���t���[��
    private bool oneboxopenSE = true;//��x��������������SE
    
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

        audiosource = GetComponent<AudioSource>();//AudioSource���擾

    }
    void FixedUpdate()
    {
        //�f�[�^�}�l�[�W���[�擾
        datamanager = ManagerAccessor.Instance.dataManager;


        //���S���ƃ|�[�Y���ɑS�Ă̏������~�߂�
        if (datamanager.isDeth || ManagerAccessor.Instance.dataManager.isPause)
        {
            //���S��
            if(datamanager.isDeth)
            {
                timer += Time.deltaTime;

                //1�b���炢�o�߂�����ēxRpcDeathKnockBack���Ăяo��
                if (knockbacktime <= timer)
                {
                    knockbackmove = false;
                    firstdeathknockback = true;
                }

                if (firstdeathknockback)
                {
                    Debug.Log("Rpc�Ԃ�");

                    photonView.RPC(nameof(RpcDeathKnockBack), RpcTarget.All);//���S���̃m�b�N�o�b�N

                    firstdeathknockback = false;
                }

                if (!knockbackmove)
                {
                    rigid.constraints = RigidbodyConstraints2D.FreezePositionX|
                                        RigidbodyConstraints2D.FreezeRotation;//FreezePositionX���I���ɂ���
                    knockback_finish = true;
                }
            }

            //�|�[�Y��
            if(ManagerAccessor.Instance.dataManager.isPause)
            {
                inputDirection.x = 0;
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX|
                                    RigidbodyConstraints2D.FreezeRotation;//�ړ��ʂ�0�ɂ���
            }
           

        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;//��]���~�߂�

            if (firstLR_1P)
            {
                //�v���C���[�̍��E�̌�����ς���
                if (datamanager.isOwnerInputKey_C_L_LEFT && !movelock)
                {
                    //Debug.Log("����������");
                    left1P = true;
                    firstLR_1P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }
                else if (datamanager.isOwnerInputKey_C_L_RIGHT && !movelock)
                {
                    //Debug.Log("�E��������");
                    left1P = false;
                    firstLR_1P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }

            }

            if (firstLR_2P)
            {
                if (datamanager.isClientInputKey_C_L_LEFT)
                {
                    // Debug.Log("2P����������");
                    left2P = true;
                    firstLR_2P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }
                else if (datamanager.isClientInputKey_C_L_RIGHT)
                {
                    // Debug.Log("2P�E��������");
                    left2P = false;
                    firstLR_2P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }
            }

            //�ړ��A�j���[�V�������Đ�����Ă���Ƃ����ʉ���炷
            if(animplay)
            {

                if(oneSE)
                {
                    audiosource.PlayOneShot(StandardSE[0]);//�������ʉ�
                    oneSE = false;
                }
                else
                {
                    walkseframe++;//��̂̌��ʉ��̍Đ����Ԃ��v������

                    //���ʉ������񂾃^�C�~���O�ōēx���ʉ���炷
                    if (walkseframe >= 30)
                    {
                        oneSE = true;
                        walkseframe = 0;//�����Ńt���[���v�Z�����Z�b�g
                    }
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

                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //�v���C���[1�i���j�̈ړ�����������Ă���Ƃ��i�����󂢂Ă��鎞�j
                    if (movelock)
                    {
                        //�����A�C�e�����}�b�v��ɂȂ��Ƃ��̂ݔ������i�ړ����������j
                        if (currentBoardObject == null &&
                             currentCopyKeyObject == null)
                        {
                            if (firstboxopen)
                            {
                                //boxopen�֐������L����
                                // Debug.Log("firstboxopen�������Ă�");
                                photonView.RPC(nameof(RpcShareBoxOpen), RpcTarget.All, true);
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

                        //�R���g���[���[�̉��{�^�����������Ƃ��������E�܂��̓R�s�[�L�[�����񂾂Ƃ����o���Ă��Ȃ���Ε���
                        if (datamanager.isOwnerInputKey_C_D_DOWN
                             && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart
                             || copykeydelete)
                        {
                            //������Ĉړ����b�N������
                            if (gameObject.name == "Player1" && boxopen)
                            {
                                change_boxopenimage = false;//��������摜�ɂ���
                                cursorlock = true;//�J�[�\���ړ����~�߂�

                                //�����蔻���߂�
                                GetComponent<BoxCollider2D>().isTrigger = false;
                                GetComponent<Rigidbody2D>().simulated = true;

                                if (!firstmovelock)
                                {
                                    photonView.RPC(nameof(RpcShareMoveLock), RpcTarget.All, false);//���̈ړ��̐�������
                                    firstmovelock = true;
                                }
                                //movelock = false;
                                GetComponent<PlayerGetHitObjTagManagement>().isMotion = true;//���̎���̔�����Ƃ�̂��ĊJ
                                copykeydelete = false;//�R�s�[�L�[�폜�ς݃t���O���Z�b�g
                                firstboxopen = true;//boxopen�t���O���L�ĉ�
                            }
                        }

                        //�Q�[���p�b�h�E�{�^���ŃA�C�e������
                        //��
                        if (datamanager.isOwnerInputKey_CB &&
                            choicecursor == "Board")
                        {
                            if (B_instantiatefirst)
                            {
                                if (currentBoardObject == null)
                                {
                                    currentBoardObject = PhotonNetwork.Instantiate("Board", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                                    // movelock = true;
                                    //Debug.Log("����");

                                    //��Ɍ�����������Ă����ꍇ
                                    if (currentCopyKeyObject != null)
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

                                    copykeydelete = false;//���폜�t���O��false

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
            }



            //�e�v���C���[�̌��ݍ��W���擾
            if (ManagerAccessor.Instance.dataManager.player1 != null)
                p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;

            if (ManagerAccessor.Instance.dataManager.player2 != null)
                p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;

            //���ƌ��̓�_�ԋ���������Ĉ��̒l�Ȃ甠�I�[�v���\
            if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
            {
                //��{�^���̓��������Ŕ��I�[�v��
                if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP
                    && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !ManagerAccessor.Instance.dataManager.isNotOpenBox) 
                {
                    //Debug.Log("��L�[������");
                    //�󔠂̃v���C���[�̎��A�󂢂Ă��锠�̃C���X�g�ɕύX
                    if (gameObject.name == "Player1")
                    {
                        if (firstchange_boximage)
                        {
                            Debug.Log("�\���W���[1st");
                            audiosource.PlayOneShot(BoxplayerSE[0]);//�����J������ʉ�
                            photonView.RPC(nameof(RpcChangeBoxOpenImage), RpcTarget.All);//�����󂯂�C���X�g�ύX�t���O�𑗐M
                            firstchange_boximage = false;
                        }

                        //movelock = true;//���̈ړ��𐧌�
                        if (firstmovelock)
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


        
    }




    private void Move()//�ړ������i�v�Z�����j
    {

        //�Q�[���I�[�o�[�܂��̓N���A������Ԃ��܂ňړ��̌v�Z������
        if(!ManagerAccessor.Instance.dataManager.isDeth 
            || !ManagerAccessor.Instance.dataManager.isClear
            || !ManagerAccessor.Instance.dataManager.isPause)
        {
            rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        }


        ////���̈ړ��ʂ������Ɛi�܂Ȃ��悤�ɂ���
        //if (Mathf.Abs(inputDirection.x) > 0.08f
        //    && firststickprocess)
        //{
        //    Debug.Log("���̋u");
        //    //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
            
        //    firststickprocess = false;//�X�e�B�b�N���ē��͂���Ȃ���Ώ������s��Ȃ�
        //}


        if (inputDirection.x == 0)
        {
           // firststickprocess = true;
            //���݃A�j���[�V�������Đ����Ă��鎞
            if (!firstanimplay)
            {
                photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All); 
              //  Debug.Log("steam");
            }
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //�v���C���[�����܂��͒��n�o������̂ɏ���Ă��鎞�A�ăW�����v�\�ɂ���
        if (collision.gameObject.tag == "Floor")
        {
            bjump = false;
        }

        //�v���C���[�������������A�Q�[���I�[�o�[�̏���������
        if (collision.gameObject.tag == "DeathField")
        {
            //Debug.Log("si");
            datamanager.isDeth = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //���΃G���A�ɓ���ƃQ�[���I�[�o�[�̃V�[��
        if (collision.gameObject.tag == "DeathErea")
        {
             

            datamanager.DeathPlayerName = this.gameObject.name;

            Debug.Log("���񂾓z�̖��O"+ datamanager.DeathPlayerName);

            datamanager.isDeth = true;
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
                   // Debug.Log("�A�j�����M");
                    photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
                    firstanimplay = false;
                }

                //Debug.Log("�X�e�B�b�N�������Ĉړ����Ă���");
                //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
                if (islift &&
                    !((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
                    (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT)))
                {

                    inputDirection.x = 0;
                }
                else
                    inputDirection = context.ReadValue<Vector2>();
            }

        }
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //�A�����b�N�{�^���A���P�b�g���N�����łȂ��� ���S���ĂȂ���
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock && !isFly
          &&!islift  && !ManagerAccessor.Instance.dataManager.isDeth
          && !ManagerAccessor.Instance.dataManager.isClear
          && !ManagerAccessor.Instance.dataManager.isPause) 
        {
            Debug.Log("�W�����v�ł���");

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
                photonView.RPC(nameof(RpcPlayJumpSE), RpcTarget.All);//�W�����v�̌��ʉ�
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
            //Debug.Log("QQQP1�����グ�摜");
            change_unloadimage = false;//�ʏ�摜���玝���グ�摜��
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p1LiftImage;

        }
        else if (gameObject.name == "Player2")
        {
           // Debug.Log("QQQP2�����グ�摜");
            change_unloadimage = false;//�ʏ�摜���玝���グ�摜��
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p2LiftImage;
        }
    }

    [PunRPC]
    private void RpcChangeBoxOpenImage()
    {

        Debug.Log("ake");
        //�����蔻���؂�
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().simulated = false;

       

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
               // Debug.Log("player1�̍�");
                imageleft = true;
                firstLR_1P = true;
            }
            else
            {
                 //Debug.Log("player1�̉E");
                imageleft = false;
                firstLR_1P = true;
            }

        }
        else if (gameObject.name == "Player2")
        {
            if (left2P)
            {
                //Debug.Log("player2�̍�");
                imageleft = true;
                firstLR_2P = true;
            }
            else
            {
                //Debug.Log("player2�̉E");
                imageleft = false;
                firstLR_2P = true;
            }
        }
    }

    [PunRPC]
    private void RpcDeathKnockBack()
    {
        Debug.Log("�m�b�N�o�b�N�����͂���");

        if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player1")
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�m�b�N�o�b�N����
                //�����̓m�b�N�o�b�N�����Ƃ���x���˂鏈��
                if (firstdeathjump)
                {
                    //Debug.Log("Takeru");
                    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    firstdeathjump = false;
                }

                //������1�b���炢���Ɉړ����鏈��
                if (knockbackmove)
                {
                    rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
                }
                //else
                //{
                //    rigid.constraints = RigidbodyConstraints2D.FreezePositionX;//FreezePositionX���I���ɂ���
                //    knockback_finish = true;
                //}
            }
        }
        else if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player2")
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //�m�b�N�o�b�N����
                //�����̓m�b�N�o�b�N�����Ƃ���x���˂鏈��
                if (firstdeathjump)
                {
                    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    firstdeathjump = false;
                }

                //������1�b���炢���Ɉړ����鏈��
                if (knockbackmove)
                {
                    rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
                }
                //else
                //{
                //    rigid.constraints = RigidbodyConstraints2D.FreezePositionX;//FreezePositionX���I���ɂ���
                //    knockback_finish = true;
                //}
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

    [PunRPC]
    private void RpcPlayJumpSE()
    {
        audiosource.PlayOneShot(StandardSE[1]);//�W�����v���ʉ�
    }


}
