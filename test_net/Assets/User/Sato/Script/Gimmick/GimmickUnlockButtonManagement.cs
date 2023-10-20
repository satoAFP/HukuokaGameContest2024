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

    [SerializeField, Header("���͎��̐�������")]
    private int timeLimit;

    //���͊J�n���
    /*[System.NonSerialized]*/ public bool isStartCount = false;

    //���ꂼ��̓��͏�
    [System.NonSerialized] public bool isOwnerClear = false;
    [System.NonSerialized] public bool isClientClear = false;

    //�񓚃f�[�^
    private List<int> answer = new List<int>();

    private int frameCount = 0;


    //�񓚃f�[�^������1�x�������Ȃ��p
    private bool isAnswerFirst = true;
    //�A�����b�N�{�^���N����Ԃ�A���œ����ȂȂ��p
    private bool isUnlockButtonStartFirst = true;
    //�N���A�󋵋��L��A���œ����Ȃ��p
    private bool isClearFirst = true;


    private enum Key
    {
        A,B,X,Y
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
                        //�A���œ��������ɂȂ�Ȃ����߂̏���
                        while (true)
                        {
                            if (i != 0)
                            {
                                if (answer[i] == answer[i - 1])
                                    answer[i] = Random.Range(0, 4);
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, answer[i]);
                    }
                    ManagerAccessor.Instance.dataManager.chat.text = answer[0].ToString() + ":" + answer[1].ToString() + ":" + answer[2].ToString() + ":" + answer[3].ToString() + ":" + answer[4].ToString();

                    //�������͗p�u���b�N�ɓ����f�[�^��n��
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;

                        //�N���A�󋵏�����
                        for (int j = 0; j < answer.Count; j++)
                        {
                            gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation.Add(false);
                        }
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
                    ManagerAccessor.Instance.dataManager.chat.text = answer[0].ToString() + ":" + answer[1].ToString() + ":" + answer[2].ToString() + ":" + answer[3].ToString() + ":" + answer[4].ToString();
                    //�������͗p�u���b�N�ɓ����f�[�^��n��
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;

                        //�N���A�󋵏�����
                        for(int j=0;j<answer.Count;j++)
                        {
                            gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation.Add(false);
                        }
                    }
                    isAnswerFirst = false;
                }
            }
        }

        //���͊J�n���̎��Ԍv�Z
        if(isStartCount)
        {
            frameCount++;
            if (frameCount == timeLimit * 60) 
            {
                //���͏󋵏�����
                isStartCount = false;
                frameCount = 0;

                //���������
                for (int i = 0; i < gimmickButton.Count; i++)
                {
                    //�N���A�󋵏�����
                    for (int j = 0; j < answer.Count; j++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation.Add(false);
                    }
                }
            }
        }






        //isUnlockButtonStart�̃f�[�^���L����
        if (ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
        {
            if(isUnlockButtonStartFirst)
            {
                photonView.RPC(nameof(RpcShareIsUnlockButtonStart), RpcTarget.Others, ManagerAccessor.Instance.dataManager.isUnlockButtonStart);
                isUnlockButtonStartFirst = false;
            }
        }
        else
        {
            if (!isUnlockButtonStartFirst)
            {
                photonView.RPC(nameof(RpcShareIsUnlockButtonStart), RpcTarget.Others, ManagerAccessor.Instance.dataManager.isUnlockButtonStart);
                isUnlockButtonStartFirst = true;
            }
        }

        //isUnlockButtonStart�̃f�[�^���L����
        if (ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
        {
            if (isUnlockButtonStartFirst)
            {
                photonView.RPC(nameof(RpcShareIsUnlockButtonStart), RpcTarget.Others, ManagerAccessor.Instance.dataManager.isUnlockButtonStart);
                isUnlockButtonStartFirst = false;
            }
        }
        else
        {
            if (!isUnlockButtonStartFirst)
            {
                photonView.RPC(nameof(RpcShareIsUnlockButtonStart), RpcTarget.Others, ManagerAccessor.Instance.dataManager.isUnlockButtonStart);
                isUnlockButtonStartFirst = true;
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

    //isUnlockButtonStart�����L
    [PunRPC]
    private void RpcShareIsUnlockButtonStart(bool data)
    {
        ManagerAccessor.Instance.dataManager.isUnlockButtonStart = data;
    }

    //isStartCount�����L
    [PunRPC]
    private void RpcShareIsStartCount(bool data)
    {
        isStartCount = data;
    }


    //�N���A�󋵂����L
    [PunRPC]
    private void RpcShareIsClear(bool data)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            isOwnerClear = data;
        }
        else
        {
            isClientClear = data;
        }
    }
}
