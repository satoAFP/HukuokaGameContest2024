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

    //�l�Ŏ����Ă���G��Ă��锻��
    private bool islocalUnlockButtonStart = false;

    //�ǂ���̃v���C���[���G��Ă��邩
    private bool isHitPlayer1 = false;
    private bool isHitPlayer2 = false;

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
                //�}�X�^�[���ǂ���
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().isOwnerClear = true;
                }
                else
                {
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().isClientClear = true;

                }
            }



        }

       


    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        //�u���b�N�ɐG��Ă��锻��
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                

                if (!isHitPlayer2)
                {
                    islocalUnlockButtonStart = true;
                    isHitPlayer1 = true;
                }
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                

                if (!isHitPlayer1)
                {
                    islocalUnlockButtonStart = true;
                    isHitPlayer2 = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //�u���b�N���痣�ꂽ����
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
                islocalUnlockButtonStart = false;

                isHitPlayer1 = false;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
                islocalUnlockButtonStart = false;

                isHitPlayer2 = false;
            }
        }
    }
}
