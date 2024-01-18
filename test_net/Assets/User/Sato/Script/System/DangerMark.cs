using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DangerMark : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("����")] private Transform stoneGenelate;

    [SerializeField, Header("���΂̑O��ǂ̍��W�ɐ������邩")] private float genelatePosX;
    [SerializeField, Header("�v���C���[�̏㉺�ǂ̍��W�ɐ������邩")] private float genelatePosY;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.zero;

        //�\��������W�̐ݒ�
        if (PhotonNetwork.IsMasterClient)
            pos = new Vector3(stoneGenelate.position.x + genelatePosX, ManagerAccessor.Instance.dataManager.player1.transform.position.y + genelatePosY);
        else
            pos = new Vector3(stoneGenelate.position.x + genelatePosX, ManagerAccessor.Instance.dataManager.player2.transform.position.y + genelatePosY);

        //���W�ύX
        transform.position = pos;


    }
}
