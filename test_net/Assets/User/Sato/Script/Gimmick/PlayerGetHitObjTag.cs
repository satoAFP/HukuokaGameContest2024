using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGetHitObjTag : MonoBehaviourPunCallbacks
{
    [System.NonSerialized] public List<string> HitTags;

    [SerializeField, Header("���[�J�����W")] private Vector3 localPos;

    private void Update()
    {
        //�f�[�^�}�l�[�W���[�擾
        DataManager dataManager = ManagerAccessor.Instance.dataManager;

        //�v���C���[����������Ă���Ƃ�
        if (dataManager.player1 != null && dataManager.player2 != null)
        {
            //���ꂼ��̍��W�Ɉړ�������
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                transform.position = dataManager.player1.transform.position + localPos;
            }
            else
            {
                transform.position = dataManager.player2.transform.position + localPos;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�G�ꂽ�I�u�W�F�N�g�̃^�O�L��
        if (photonView.IsMine)
        {
            HitTags.Add(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�o���I�u�W�F�N�g�̃^�O������
        if (photonView.IsMine)
        {
            for (int i = 0; i < HitTags.Count; i++)
            {
                if (HitTags[i] == collision.tag)
                {
                    HitTags.RemoveAt(i);
                }
            }
        }
    }
}
