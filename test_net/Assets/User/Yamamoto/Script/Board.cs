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

    //�ړ����������ϐ�
    private Collider2D collider;//�̃R���C�_�[

    //inputsystem���X�N���v�g�ŌĂяo��
    private BoardInput boardinput;

    //�ړ����~�߂�
    private bool movelock = false;

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
        boardinput = new BoardInput();//�X�N���v�g��ϐ��Ɋi�[

        collider.isTrigger = true;//�R���C�_�[�̃g���K�[��

    }

    // Update is called once per frame
    void Update()
    {
        //�f�[�^�}�l�[�W���[�擾
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        // 2�����͓ǂݍ���
        var inputValue = _moveAction.action.ReadValue<Vector2>();

        if(!movelock)
        {
            // xy�������ňړ�
            transform.Translate(inputValue * (moveSpeed * Time.deltaTime));
        }
       

        if (datamanager.isOwnerInputKey_CB)
        {
            movelock = true;
            collider.isTrigger = false;//�g���K�[������
        }

    }
}
