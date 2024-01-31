using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Board : MonoBehaviourPunCallbacks
{

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    // 2�����͂��󂯎��Action
    [SerializeField] private InputActionProperty _moveAction;

    private Collider2D collider;//�̃R���C�_�[

    private Rigidbody2D rigid;//���W�b�h�{�f�B

    public int pushnum = 0;//�{�^������������

    //inputsystem���X�N���v�g�ŌĂяo��
    private BoardInput boardinput;

    //�ړ����~�߂�
    private bool movelock = false;
    //�{�^���̕�������͂�h��
    private bool pushbutton = false;

    [SerializeField, Header("�A�C�e��������ԁi���60�łP�b�j")]
    private int collecttime;

    [SerializeField]
    public int holdtime;//�ݒ肵���A�C�e��������Ԃ�������

    private bool firstcorsor = false;//�J�[�\�����ړ������邽�тɃ{�^���̕\����ς���

    private void OnDestroy()
    {
        _moveAction.action.Dispose();
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
 
        collider = this.GetComponent<BoxCollider2D>();

        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        boardinput = new BoardInput();//�X�N���v�g��ϐ��Ɋi�[

        collider.isTrigger = true;//�R���C�_�[�̃g���K�[��

        holdtime = collecttime;//�������J�E���g���Ԃ�������

        //�S�̂���R�s�[���擾
        ManagerAccessor.Instance.dataManager.board = gameObject;

        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = true;//�J�[�\���ړ����b�N

        //�摜�𔼓����ɂ���
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 128);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�f�[�^�}�l�[�W���[�擾
        DataManager datamanager = ManagerAccessor.Instance.dataManager;
        
        //�v���C���[���Q�[���I�[�o�[�ɂȂ��Ă��Ȃ���Δ̊�{���싖��
        if(!ManagerAccessor.Instance.dataManager.isDeth)
        {
            //���݃J�[�\������I��ł��鎞
            if (ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "Board")
            {
                //�v���C���[1���i���j�ł�������ł��Ȃ�
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    if (firstcorsor)
                    {
                        //��ݒu�������ɉ��{�^���̐����o���\��
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowDown;

                        firstcorsor = false;
                    }

                    // 2�����͓ǂݍ���
                    var inputValue = _moveAction.action.ReadValue<Vector2>();

                    if (!movelock)
                    {
                        // xy�������ňړ�
                        transform.Translate(inputValue * (moveSpeed * Time.deltaTime));
                    }


                    //�Q�[���p�b�h�̉E�{�^�����������Ƃ�
                    if (datamanager.isOwnerInputKey_CB)
                    {
                       

                        if (!pushbutton)
                        {
                            pushbutton = true;
                            pushnum++;
                        }

                    }
                    else
                    {
                        pushbutton = false;
                    }

                    //pushnum��2�Ȃ͔̂������ɉE�{�^���������ꂽ��Ԃ̂��ߏ����l��1�ɂȂ��Ă���
                    if (pushnum == 2)
                    {
                        Debug.Log("Set");
                        photonView.RPC(nameof(Rpc_SetBoard), RpcTarget.All);

                        //��ݒu�������ɉ��{�^���̐����o���\��
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowDown;


                    }


                }


                //�Q�[���p�b�h���{�^���Œu���Ȃ���
                if (datamanager.isOwnerInputKey_CA)
                {
                    //holdtime--;//�������J�E���g�_�E��

                    //�����ł��������o����\�������Ȃ�
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        //��u���Ȃ������Ƃ��E�{�^���̐����o���\��
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                        ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
                    }

                    //�摜�𔼓����ɂ���
                    GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 128);

                    movelock = false;
                    collider.isTrigger = true;//�g���K�[��
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = true;//���̐������~�߂�
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = true;//�J�[�\���ړ����~�߂�
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = true;//���̈ړ������Ȃ�


                }


            }
            else
            {
                firstcorsor = true;
            }

            //�\���L�[���ŃA�C�e�����
            if (datamanager.isOwnerInputKey_C_D_DOWN)
            {
                holdtime--;//�������J�E���g�_�E��

                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = true;//���̐������~�߂�
                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = true;//�J�[�\���ړ����~�߂�
                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = true;//���̈ړ������Ȃ�

                //�Q�[���p�b�h���{�^���������ŉ��
                if (holdtime <= 0)//����J�E���g��0�ɂȂ�Ɖ��
                {
                    DeleteBoard();
                }

            }
            else
            {
                holdtime = collecttime;//�������J�E���g���Z�b�g
            }
        }
        else
        {
            collider.isTrigger = true;//�O�̂��߂Ƀg���K�[��
        }
        

    }


    //���̊֐��͒ʐM�p
    [PunRPC]
    private void Rpc_SetBoard()
    {
        //�摜�̓����x�����ɖ߂�
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

        movelock = true;
        collider.isTrigger = false;//�g���K�[������
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = false;//����������
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = false;//�J�[�\���ړ�����
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = false;//���̈ړ��\
        pushnum = 1;
    }

    //���폜
    private void DeleteBoard()
    {
      //  ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().boxopen = true;//�����J����
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().generatestop = false;//����������
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().cursorlock = false;//�J�[�\���ړ�����
        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock = false;//���̈ړ��\
        Destroy(gameObject);
    }
}
