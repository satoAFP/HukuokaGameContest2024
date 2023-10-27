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

    public int a = 0;

    //inputsystem���X�N���v�g�ŌĂяo��
    private BoardInput boardinput;

    //�ړ����~�߂�
    private bool movelock = false;
    //�{�^���̕�������͂�h��
    private bool pushbutton = false;

    [SerializeField, Header("�A�C�e��������ԁi���60�łP�b�j")]
    private int collecttime;

    [SerializeField]
    private int holdtime;//�ݒ肵���A�C�e��������Ԃ�������

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

        holdtime = collecttime;//�ݒ肵���A�C�e��������Ԃ�������

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�f�[�^�}�l�[�W���[�擾
        DataManager datamanager = ManagerAccessor.Instance.dataManager;
        //�v���C���[1���i���j�ł�������ł��Ȃ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
 
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
                    a++;
                }

            }
            else
            {
                pushbutton = false;
            }

            if (a == 2)
            {
                Debug.Log("Set");
                photonView.RPC(nameof(Rpc_SetBoard), RpcTarget.All);
            }


        }

        if (datamanager.isOwnerInputKey_CB)
        {
            holdtime--;//�������ŃA�C�e�����
            if (holdtime <= 0)//����J�E���g��0�ɂȂ�Ɖ��
            {
                Destroy(gameObject);
            }
        }
        else
        {
            holdtime = collecttime;//�{�^���𗣂��Ɖ���J�E���g���Z�b�g
        }



    }


    //���̊֐��͒ʐM�p
    [PunRPC]
    private void Rpc_SetBoard()
    {
        movelock = true;
        collider.isTrigger = false;//�g���K�[������
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        a = 0;
    }
}
