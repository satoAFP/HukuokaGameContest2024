using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
    //[SerializeField, Header("���S���̃R�s�[�L�[")]
    //private Sprite DeathImage;

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    [SerializeField, Header("�W�����v���x")]
    private float jumpSpeed;

    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    [System.NonSerialized] public bool bjump;//�A���ŃW�����v�����Ȃ��t���O

    private bool firstlanding = true;//��x�������ɒ��n�������̏�����ʂ�

    [SerializeField, Header("��������ԁi���60�łP�b�j")]
    private int collecttime;

    [SerializeField]
    public int holdtime;//�ݒ肵���A�C�e��������Ԃ�������

    private Test_net test_net;//inputsystem���X�N���v�g�ŌĂяo��

    [System.NonSerialized] public bool copykey_death = false;//�R�s�[�L�[�����S�������̃t���O
    private bool firstdeathjump = true;//���S���̃m�b�N�o�b�N�W�����v����񂾂�������
    private float knockbacktime = 1.0f;//�m�b�N�o�b�N����Ƃ��̂w���W���ړ�
    private float timer = 0f;//���Ԃ��J�E���g

    //�u���b�N�����グ�Ɏg���ϐ�
    [System.NonSerialized] public bool islift = false;//�����グ�t���O
    [System.NonSerialized] public bool isliftfirst = true;//�����グ�t���O�̏�Ԃ𑗐M����Ƃ���񂵂����M���Ȃ�����                                                      
    private bool distanceFirst = true;//���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
    private Vector3 dis = Vector3.zero;

    private bool firstDeathEreaHit = true;//��x�����Q�[���I�[�o�[�G���A�ɓ�����������������
    private bool fallingdeath = false;//�����������Ƃ��̏���
    private bool firstfallingdeath = true;//��x�����������̏������s��

    private bool firstLR = true;//���E�ړ���x�����������s��
    private bool left = false;//�R�s�[�L�[�����Ɍ����Ă���Ƃ�
    [System.NonSerialized] public bool imageleft = false;//�摜���������ɂ���t���O

    private bool firstChangeLiftImage = true;
    [System.NonSerialized] public bool changeliftimage = false;//�R�s�[�L�[�������グ�摜�ύX
    [System.NonSerialized] public bool standardCopyKeyImage = false;//�W���R�s�[�L�[�摜

    [System.NonSerialized] public bool animplay = false;//�A�j���[�V�������Đ�
    private bool firstanimplay = true;//�����A�j���N���������Ȃ��t���O

    private AudioSource audiosource = null;//�I�[�f�B�I�\�[�X
    [SerializeField, Header("�R�s�[�L�[�W��SE")] private AudioClip[] StandardSE;
    private bool oneSE = true;//�e������x�������sSE��炷
    private int walkseframe = 0;//se�Đ����ɑ���t���[��
    private bool oneDeathSE = true;//�e������x�������SSE��炷

    private DataManager datamanager;//�f�[�^�}�l�[�W���[�擾

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

        audiosource = GetComponent<AudioSource>();//AudioSource���擾
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�f�[�^�}�l�[�W���[�擾
        datamanager = ManagerAccessor.Instance.dataManager;

        Debug.Log("ani="+firstanimplay);

        if (ManagerAccessor.Instance.dataManager.isDeth)
        {
            Destroy(gameObject);//�v���C���[�����S�����Ƃ��R�s�[�L�[�폜
        }

        //�v���C���[�����ɒ����Ă��邩�𔻒f����ϐ������L
        if (PhotonNetwork.IsMasterClient)
        {
            bjump = !ManagerAccessor.Instance.dataManager.isOwnerHitDown;
        }

        //��x�������n����������ʂ�
        if (photonView.IsMine)
        {
            if (inputDirection.x != 0)//�ړ����̂ݏ�����ʂ�
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (datamanager.isOwnerHitDown)
                    {
                        if (firstlanding)
                        {
                            photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
                            firstlanding = false;
                        }
                    }
                    else
                    {
                        if (!firstlanding)
                        {
                            firstlanding = true;
                        }
                    }
                }
            }
        }

        //�v���C���[���Q�[���I�[�o�[�ɂȂ��Ă��Ȃ���΃R�s�[�L�[�̊�{���싖��
        if (!ManagerAccessor.Instance.dataManager.isDeth
            || !copykey_death
            || !ManagerAccessor.Instance.dataManager.isPause)
        {
            //�J�[�\��������I��ł���Ƃ�����\
            if ( !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
                &&ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")
            {

                if (firstLR)
                {
                    //�R�s�[�L�[�̂̍��E�̌�����ς���
                    if (datamanager.isOwnerInputKey_C_L_LEFT)
                    {
                       
                        left = true;
                        firstLR = false;
                        photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                    }
                    else if (datamanager.isOwnerInputKey_C_L_RIGHT)
                    {
                      
                        left = false;
                        firstLR = false;
                        photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                    }
                }


                //�ړ��A�j���[�V�������Đ�����Ă���Ƃ����ʉ���炷
                if (animplay
                 && !ManagerAccessor.Instance.dataManager.isPause)
                {

                    if (oneSE)
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

                    //�R�s�[�L�[��I�����Ă���Ƃ��\���L�[���̐����o���\��
                    ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                    ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossDown;

                    //�����グ�Ă��Ȃ��Ƃ��͕��ʂɈړ�������
                    if (!islift)
                    {
                        Move();//�ړ�������ON
                        distanceFirst = true;

                        photonView.RPC(nameof(RpcChangeStandardImage), RpcTarget.All, true);

                    }
                    else
                    {
                        photonView.RPC(nameof(RpcChangeLiftImege), RpcTarget.All, true);

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

          
        }
      
          
        if (copykey_death)
        {
            // �摜��؂�ւ��܂�
            //GetComponent<SpriteRenderer>().sprite = DeathImage;

            if (fallingdeath)
            {
                Debug.Log("�R�s�[�L�[����");
                //�R�s�[���o�����t���O
                ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                ManagerAccessor.Instance.dataManager.copyKey = null;

                //�R�s�[�L�[�����������Ƃ��v���C���[���ɕԂ�
                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().copykeydelete = true;

                Destroy(gameObject);//�O�̂��߂ɃR�s�[�L�[���폜
            }
            else
            {
                Debug.Log("�R�s�[�L�[�����ł͂Ȃ�");
                if (oneDeathSE)
                {
                    audiosource.PlayOneShot(StandardSE[2]);//���S����SE��炷
                    oneDeathSE = false;
                }

                timer += Time.deltaTime;//���Ԍv��

                //�m�b�N�o�b�N����
                //�����̓m�b�N�o�b�N�����Ƃ���x���˂鏈��
                if (firstdeathjump)
                {
                    //Debug.Log("copykey_deathjump");
                    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    firstdeathjump = false;
                }

                //������1�b���炢���Ɉړ����鏈��
                if (timer <= knockbacktime)
                {
                    //Debug.Log("copykey_deathmove");
                    rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
                }
                else if (timer >= 2.0f)
                {
                    //�R�s�[���o�����t���O
                    ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                    ManagerAccessor.Instance.dataManager.copyKey = null;

                    //�R�s�[�L�[�����������Ƃ��v���C���[���ɕԂ�
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().copykeydelete = true;

                    //�G�t�F�N�g����
                    GameObject clone = Instantiate(ManagerAccessor.Instance.dataManager.StarEffect);
                    clone.transform.position = transform.position;

                    Destroy(gameObject);//�O�̂��߂ɃR�s�[�L�[���폜
                }
            }

           
        }
       
    }


    private void Move()//�ړ������i�v�Z�����j
    {
        if(!copykey_death
            ||!ManagerAccessor.Instance.dataManager.isPause)
        {
            //�v���C���[�����͂��������ɉ���������ňړ����x���̗͂�������
            rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        }

        if (inputDirection.x == 0)
        {
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
        
        //�v���C���[�������������A�Q�[���I�[�o�[�̏���������
        if (collision.gameObject.tag == "DeathField")
        {
            if (firstfallingdeath)
            {
              
                photonView.RPC(nameof(RpcCopyKeyDeath), RpcTarget.All, true);//�R�s�[�L�[���S����

                fallingdeath = true;//�������p�̎��S����������
                firstfallingdeath = false;
            }
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
                //Debug.Log("�R�s�[�L�[������");
                photonView.RPC(nameof(RpcCopyKeyDeath), RpcTarget.All, true);//�R�s�[�L�[���S����
                firstDeathEreaHit = false;
            }
           
        }
    }

    //playerinput�ŋN��������֐�
    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            if (firstanimplay)
            {
                //Debug.Log("�A�j�����M");
                photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
                firstanimplay = false;
            }

            //�ړ������̓��͏��Inputdirection�̒��ɓ���悤�ɂȂ�
            inputDirection = context.ReadValue<Vector2>();
        }
  
    }

    //�W�����v
    public void Onjump(InputAction.CallbackContext context)
    {
        if (!copykey_death)
        {
            //�J�[�\��������I�����Ă��鎞
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
           && ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey"
           && !islift
           && !ManagerAccessor.Instance.dataManager.isPause)
            {
                //1P�i�����j�ł̑��삵���󂯕t���Ȃ�
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //�A�����b�N�{�^�����N����
                    if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
                    {
                        //Debug.Log("�R�s�[�L�[�W�����v");

                        photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All);//�W�����v���Ă��鎞�͈ړ��A�j���[�V�������~�߂�

                        //Input System����W�����v�̓��͂����������ɌĂ΂��
                        if (!context.performed || bjump)
                        {
                            return;
                        }
                        else
                        {
                            photonView.RPC(nameof(RpcPlayJumpSE), RpcTarget.All);//�W�����v�̌��ʉ�
                            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                            bjump = true;//��x�W�����v�����璅�n����܂ŃW�����v�ł��Ȃ�����
                        }


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

    [PunRPC]
    private void RpcChangeLiftImege(bool data)
    {
        standardCopyKeyImage = false;
        changeliftimage = data;//changeliftimage�����L����
        //firstChangeLiftImage = true;
    }

    [PunRPC]
    private void RpcChangeStandardImage(bool data)
    {
        changeliftimage = false;
        standardCopyKeyImage = data;//standardCopyKeyImage�����L����
    }

    [PunRPC]
    private void RpcMoveLeftandRight()
    {
        if (left)
        {
          
            imageleft = true;
            firstLR = true;
        }
        else
        {
           
            imageleft = false;
            firstLR = true;
        }
    }

    [PunRPC]
    private void RpcMoveAnimPlay()
    {
        animplay = true;
    }

    [PunRPC]
    private void RpcMoveAnimStop()
    {
        animplay = false;
        firstanimplay = true;
    }

    [PunRPC]
    private void RpcPlayJumpSE()
    {
        audiosource.PlayOneShot(StandardSE[1]);//�W�����v���ʉ�
    }

}


