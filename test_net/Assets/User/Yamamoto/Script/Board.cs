using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{

    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    //�ړ����������ϐ�
    private Rigidbody2D rigid;

    private BoardInput boardinput;//inputsystem���X�N���v�g�ŌĂяo��

    // Start is called before the first frame update
    void Start()
    {
        //Player��Rigidbody2D�R���|�[�l���g���擾����
        rigid = GetComponent<Rigidbody2D>();

        boardinput = new BoardInput();//�X�N���v�g��ϐ��Ɋi�[

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
