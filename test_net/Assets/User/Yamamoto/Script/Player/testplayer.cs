using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class testplayer : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�ړ����x")]
    private float moveSpeed;

    
    void Start()
    {
        //���O��ID��ݒ�
        gameObject.name = "Player" + photonView.OwnerActorNr;
    }


    void Update()
    {
        //���삪�������Ȃ����߂̐ݒ�
        if (photonView.IsMine)
        {
            // ���͂�x�ɑ��
            float x = Input.GetAxis("Horizontal");

            //Rigidbody2D���擾
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            //x���ɉ����͂��i�[
            Vector2 force = new Vector2(x * 10, 0);

            //Rigidbody2D�ɗ͂�������
            rb.AddForce(force);
        }
           
    }
}
