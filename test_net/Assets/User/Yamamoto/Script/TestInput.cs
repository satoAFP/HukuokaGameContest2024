using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class TestInput : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputAction m_inputmover;//�C���v�b�g�A�N�V����
    //���͂��ꂽ����������ϐ�
    private Vector2 inputDirection;
    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    //playerinput���K�v�Ȏ��ɌĂяo��
    private void OnEnable()
    {
        m_inputmover.Enable();
    }
    private void OnDisable()
    {
        m_inputmover.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        //���O��ID��ݒ�
        gameObject.name = "Player" + photonView.OwnerActorNr;

        // �f�o�C�X�ꗗ���擾
        foreach (var device in InputSystem.devices)
        {
            // �f�o�C�X�������O�o��
            Debug.Log(device.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
             inputDirection = m_inputmover.ReadValue<Vector2>();
             transform.Translate
        (
            inputDirection.x * moveSpeed,
            inputDirection.y * moveSpeed,
            0.0f);
        }
       

    }
}
