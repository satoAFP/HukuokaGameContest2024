using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControl : MonoBehaviour
{
    private InputAction inputaction;//InputSystem���g�����߂ɕK�v�ȃX�N���v�g

    [SerializeField,Header("�v���C���[�̃X�s�[�h")]
    private float speed = 0;

    private Rigidbody2D rb;//�v���C���[�̃��W�b�g�{�f�B

    //���͂̏㉺�ړ��̔��ʂ�����
    private float movementX;
    private float movementY;


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�ɃA�^�b�`����Ă���Rigidbody���擾
        rb = gameObject.GetComponent<Rigidbody2D>();

        inputaction = new InputAction();
        inputaction.Enable();//�C���X�^���X������InputSystem�����p�\��
    }

    /// <summary>
    /// �ړ�����i�㉺���E�L�[�Ȃǁj���擾
    /// </summary>
    /// <param name="movementValue"></param>
    private void OnMove(InputValue movementValue)
    {
        // Move�A�N�V�����̓��͒l���擾
        Vector2 movementVector = movementValue.Get<Vector2>();

        // x,y�������̓��͒l��ϐ��ɑ��
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // ���͒l������3���x�N�g�����쐬
        Vector3 movement = new Vector3(movementX / 5, 0.0f, movementY / 5);

        // rigidbody��AddForce���g�p���ăv���C���[�𓮂����B
       // rb.AddForce(movement * speed);

        transform.position += movement;

    }
}
