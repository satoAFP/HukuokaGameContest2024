using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickFallStoneManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("����")] private GameObject stone;

    [SerializeField, Header("�����Ԋu")] private int CreateFrame;

    //�t���[���J�E���g�p
    private int frameCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ManagerAccessor.Instance.dataManager.isFlyStart)
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
}
