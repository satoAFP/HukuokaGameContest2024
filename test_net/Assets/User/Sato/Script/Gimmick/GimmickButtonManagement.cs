using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButtonManagement : CGimmick
{
    [SerializeField, Header("�M�~�b�N�p�{�^��")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("��")]
    private GameObject door;


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
            door.SetActive(false);
        }

    }
}
