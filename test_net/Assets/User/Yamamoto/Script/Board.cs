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
    //private Rigidbody2D rigid;

    //inputsystem���X�N���v�g�ŌĂяo��
    private BoardInput boardinput;

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
        //Player��Rigidbody2D�R���|�[�l���g���擾����
        //rigid = GetComponent<Rigidbody2D>();

        boardinput = new BoardInput();//�X�N���v�g��ϐ��Ɋi�[

    }

    // Update is called once per frame
    void Update()
    {
        // 2�����͓ǂݍ���
        var inputValue = _moveAction.action.ReadValue<Vector2>();

        // xy�������ňړ�
        transform.Translate(inputValue * (moveSpeed * Time.deltaTime));

    }
}
