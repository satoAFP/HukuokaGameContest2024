using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickFallStoneManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("����")] private GameObject stone;

    [SerializeField, Header("�����Ԋu")] private int CreateFrame;

    [SerializeField, Header("�ǉ�������")] private int AddCreateNum;

    //�t���[���J�E���g�p
    private int frameCount = 0;

    private void Start()
    {
        //�}�X�^�[�̂ݐ���
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < AddCreateNum; i++)
            {
                //�͈͓��̃����_���ȍ��W
                Vector2 pos = new Vector2(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
                    transform.position.y - stone.GetComponent<GimmickFallStone>().Speed.y * CreateFrame * i);

                //������������
                PhotonNetwork.Instantiate("GimmickStone", pos, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�}�X�^�[�̂ݐ���
        if (PhotonNetwork.IsMasterClient)
        {
            //���Ԋu�Ő���
            frameCount++;
            if (frameCount == CreateFrame)
            {
                //�͈͓��̃����_���ȍ��W
                Vector2 pos = new Vector2(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), transform.position.y);

                //���ΐ���
                PhotonNetwork.Instantiate("GimmickStone", pos, Quaternion.identity);
                frameCount = 0;
            }
        }
    }
}
