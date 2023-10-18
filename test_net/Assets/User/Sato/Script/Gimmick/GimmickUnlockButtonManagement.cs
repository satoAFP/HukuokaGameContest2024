using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickUnlockButtonManagement : CGimmick
{
    [SerializeField, Header("�M�~�b�N�p�{�^��")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("��")]
    private GameObject door;

    [SerializeField, Header("���͂��鐔")]
    private int inputKey;

    public List<int> answer = new List<int>();

    private bool isAnswerFirst = true;

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
                photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, answer[i]);
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
        //�}�X�^�[�̎�������ݒ肵�ăf�[�^��n��
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //2P�����Ȃ����͍��Ȃ�
            if (ManagerAccessor.Instance.dataManager.player2 != null)
            {
                //�ŏ��̈�񂾂�
                if (isAnswerFirst)
                {
                    //�����̐����ƃf�[�^�̎󂯓n��
                    for (int i = 0; i < inputKey; i++)
                    {
                        answer.Add(Random.Range(0, 4));
                        photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, answer[i]);
                    }

                    //�������͗p�u���b�N�ɓ����f�[�^��n��
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;
                    }
                    isAnswerFirst = false;
                }
            }
        }
        //�}�X�^�[�łȂ����A�����f�[�^���󂯎��܂őҋ@
        else
        {
            if (answer.Count != 0)
            {
                //�ŏ��̈�񂾂�
                if (isAnswerFirst)
                {
                    //�������͗p�u���b�N�ɓ����f�[�^��n��
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;
                    }
                    isAnswerFirst = false;
                }
            }
        }


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

    //�}�X�^�[�T�C�h�Ō��߂����������L
    [PunRPC]
    private void RpcShareAnswer(int ans)
    {
        answer.Add(ans);
    }


}
