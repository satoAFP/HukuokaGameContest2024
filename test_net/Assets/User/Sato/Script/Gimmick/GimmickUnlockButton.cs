using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GimmickUnlockButton : CGimmick
{
    private enum Key
    {
        A, B, X, Y
    }


    private bool islocalUnlockButtonStart = false;

    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public bool isButton = false;

    //����
    [System.NonSerialized] public List<int> answer = new List<int>();
    //�񓚏�
    public List<bool> ClearSituation;



    private void Update()
    {
        //�A�����b�N�{�^���J�n
        if (islocalUnlockButtonStart)
        {
            //�����̐�
            for (int i = 0; i < answer.Count; i++)
            {
                //�N���A���Ă�����͔͂�΂����
                if (ClearSituation[i])
                {
                    continue;
                }
                else
                {
                    //�}�X�^�[���ǂ���
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        //���ꂼ��̓��͂Ƃ����Ă��邩�ǂ���
                        switch (answer[i])
                        {
                            case (int)Key.A:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CA)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.B:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.X:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CX)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.Y:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CY)
                                    ClearSituation[i] = true;
                                break;
                        }
                    }
                    else
                    {
                        switch (answer[i])
                        {
                            case (int)Key.A:
                                if (ManagerAccessor.Instance.dataManager.isClientInputKey_CA)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.B:
                                if (ManagerAccessor.Instance.dataManager.isClientInputKey_CB)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.X:
                                if (ManagerAccessor.Instance.dataManager.isClientInputKey_CX)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.Y:
                                if (ManagerAccessor.Instance.dataManager.isClientInputKey_CY)
                                    ClearSituation[i] = true;
                                break;
                        }
                    }
                    break;
                }
            }

            //�ŏ��̓��͂������̎��A�J�E���g�J�n
            if (ClearSituation[0])
            {
                transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount = true;
            }

            //�Ō�̓��͂��I������Ƃ��N���A���𑗂�
            if (ClearSituation[ClearSituation.Count - 1])
            {
                Debug.Log(ClearSituation.Count - 1);
                //�}�X�^�[���ǂ���
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    Debug.Log("aaa");
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().isOwnerClear = true;
                }
                else
                {
                    Debug.Log("bbb");
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().isClientClear = true;

                }
            }



        }

       


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                islocalUnlockButtonStart = true;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                islocalUnlockButtonStart = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
                islocalUnlockButtonStart = false;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
                islocalUnlockButtonStart = false;
            }
        }
    }
}
