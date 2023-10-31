using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButtonManagement : CGimmick
{
    [SerializeField, Header("�M�~�b�N�p�{�^��")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("��")]
    private GameObject door;

    [SerializeField, Header("�ǂ̃M�~�b�N�ɂ��邩")]
    [Header("0:�I�u�W�F�N�g���� / 1:�I�u�W�F�N�g�o��")]
    private int gimmickNum;

    private void Start()
    {
        if (gimmickNum == 0)
            door.SetActive(true);
        if (gimmickNum == 1)
            door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�{�^����������Ă���I�u�W�F�N�g�̐��J�E���g�p
        int count = 0;

        //�{�^���̐�������
        for (int i = 0; i < gimmickButton.Count; i++)
        {
            if (gimmickButton[i].GetComponent<GimmickButton>().isButton == true)
            {
                count++;
            }
        }


        //������������������ƁA�����J��
        if (gimmickButton.Count == count) 
        {
            if (gimmickNum == 0)
                door.SetActive(false);
            if (gimmickNum == 1)
                door.SetActive(true);
        }

    }
}
