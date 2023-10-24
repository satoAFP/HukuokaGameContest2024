using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GimmickUnlockButton : CGimmick
{
    private enum Key
    {
        A, B, X, Y, Right, Left, Up, Down, R1, R2, L1, L2
    }

    //個人で持っている触れている判定
    private bool islocalUnlockButtonStart = false;

    //どちらのプレイヤーが触れているか
    private bool isHitPlayer1 = false;
    private bool isHitPlayer2 = false;

    private Rigidbody2D rb2d;


    //それぞれのボタン入力状況
    [System.NonSerialized] public bool isButton = false;

    //答え
    [System.NonSerialized] public List<int> answer = new List<int>();

    //回答状況
    public List<bool> ClearSituation;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


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
                    //入力情報と一致するかチェック
                    InputAnswer(i);
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



        rb2d.WakeUp();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //プレイヤー2が触れていないとき
            if (!isHitPlayer2)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //タイムリミットと回答データ描画
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(true);
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(true);
                    //オブジェクト触れている状態
                    ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                    islocalUnlockButtonStart = true;
                }

                
                isHitPlayer1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            //プレイヤー1が触れていないとき
            if (!isHitPlayer1)
            {
                if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //タイムリミットと回答データ描画
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(true);
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(true);
                    //オブジェクト触れている状態
                    ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
                    islocalUnlockButtonStart = true;
                }

                
                isHitPlayer2 = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //タイムリミットと回答データ描画終了
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            isHitPlayer1 = false;
        }
        if (collision.gameObject.name == "Player2")
        {
            //タイムリミットと回答データ描画終了
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            isHitPlayer2 = false;
        }

    }

    /// <summary>
    /// 回答入力取得用関数
    /// </summary>
    /// <param name="i">リスト内の何番目を回答中か</param>
    private void InputAnswer(int i)
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
                case (int)Key.Right:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_RIGHT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Left:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_LEFT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Up:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Down:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_DOWN)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R1:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_R1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R2:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_R2)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L1:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L2:
                    if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L2)
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
                case (int)Key.Right:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_RIGHT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Left:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_LEFT)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Up:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.Down:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_DOWN)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R1:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_R1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.R2:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_R2)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L1:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_L1)
                        ClearSituation[i] = true;
                    break;
                case (int)Key.L2:
                    if (ManagerAccessor.Instance.dataManager.isClientInputKey_C_L2)
                        ClearSituation[i] = true;
                    break;
            }
        }
    }

}
