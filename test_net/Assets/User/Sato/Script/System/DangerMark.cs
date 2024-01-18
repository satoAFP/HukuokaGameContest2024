using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DangerMark : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("����")] private Transform stoneGenelate;
    [SerializeField, Header("�J����")] private Transform cameraGenelate;

    [SerializeField, Header("���΂̑O��ǂ̍��W�ɐ������邩")] private float genelatePosX;
    [SerializeField, Header("�v���C���[�̏㉺�ǂ̍��W�ɐ������邩")] private float genelatePosY;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.zero;

        //�\��������W�̐ݒ�
        pos = new Vector3(stoneGenelate.position.x + genelatePosX, cameraGenelate.position.y + genelatePosY);

        //���W�ύX
        transform.position = pos;


    }
}
