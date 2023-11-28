using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIUnderbar : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("���v���C���[�A�C�R��")]
    private GameObject P1Icon;

    [SerializeField, Header("���v���C���[�A�C�R��")]
    private GameObject P2Icon;

    private Vector2 UnderbarPos;//���g�����삵�Ă���L�����A�C�R���̉��ɕ\������o�[�̍��W

    // Start is called before the first frame update
    void Start()
    {
        UnderbarPos.y = transform.position.y;

        //���삵�Ă���L�����ɂ���ăL�����A�C�R���̉��ɕ\������o�[�̈ʒu��ς���
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //Debug.Log("�ق�");
            UnderbarPos.x = P1Icon.transform.position.x;
        }
        else
        {
            ///Debug.Log("�ق�1");
            UnderbarPos.x = P2Icon.transform.position.x;
        }

        transform.position = UnderbarPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
