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

    //個人で持っている触れている判定
    private bool islocalUnlockButtonStart = false;

    //どちらのプレイヤーが触れているか
    private bool isHitPlayer1 = false;
    private bool isHitPlayer2 = false;

    //それぞれのボタン入力状況
    [System.NonSerialized] public bool isButton = false;

    //答え
    [System.NonSerialized] public List<int> answer = new List<int>();

    //回答状況
    public List<bool> ClearSituation;



    private void Update()
    {
        //アンロックボタン開始
        if (islocalUnlockButtonStart)
        {
            //答えの数
            for (int i = 0; i < answer.Count; i++)
            {
                //クリアしている入力は飛ばされる
                if (ClearSituation[i])
                {
                    continue;
                }
                else
                {
                    //マスターかどうか
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        //それぞれの入力とあっているかどうか
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

            //最初の入力が正解の時、カウント開始
            if (ClearSituation[0])
            {
                transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount = true;
            }

            //最後の入力が終わったときクリア情報を送る
            if (ClearSituation[ClearSituation.Count - 1])
            {
                //マスターかどうか
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
        //ブロックに触れている判定
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
        //ブロックから離れた判定
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
