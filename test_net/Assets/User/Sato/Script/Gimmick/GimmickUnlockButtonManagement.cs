using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickUnlockButtonManagement : MonoBehaviour
{
    [SerializeField, Header("�M�~�b�N�p�{�^��")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("��")]
    private GameObject door;

    [SerializeField, Header("���͂��鐔")]
    private int inputKey;

    public List<int> answer = new List<int>();

    private enum Key
    {
        A,B,X,Y
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for (int i = 0; i < inputKey; i++) 
            {
                answer.Add(Random.Range(0, 4));
            }

            for(int i=0;i<gimmickButton.Count;i++)
            {
                gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////�{�^����������Ă���I�u�W�F�N�g�̐��J�E���g�p
        //int count = 0;

        ////�{�^���̐�������
        //for (int i = 0; i < gimmickButton.Count; i++)
        //{
        //    if (gimmickButton[i].GetComponent<GimmickUnlockButton>().isButton == true)
        //    {
        //        count++;
        //    }
        //}

        ////������������������ƁA�����J��
        //if (gimmickButton.Count == count)
        //{
        //    door.SetActive(false);
        //}


    }
}
