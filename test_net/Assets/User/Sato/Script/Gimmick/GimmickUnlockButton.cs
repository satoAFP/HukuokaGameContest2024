using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class GimmickUnlockButton : CGimmick
{
    const int NONE = 999;

    private enum Key
    {
        A, B, X, Y, Right, Left, Up, Down, R1, R2, L1, L2
    }

    [SerializeField, Header("ボタン番号")] private int ObjNum;

    [SerializeField, Header("振動している時間")] private int vibrationTime;

    [SerializeField, Header("振動時の移動量")] private Vector3 vibrationPower;

    [SerializeField, Header("成功SE")] AudioClip successSE;
    [SerializeField, Header("失敗SE")] AudioClip failureSE;

    private AudioSource audioSource;

    private DataManager dataManager = null;

    //個人で持っている触れている判定
    private bool islocalUnlockButtonStart = false;

    //入力が失敗した時リセットするよう
    private bool isAnswerReset = false;

    //失敗時振動
    private bool isVibration = false;
    private int vibrationNum = NONE;
    private int vibrationCount = 0;

    private List<string> HitNames = new List<string>();

    private Rigidbody2D rb2d;


    //それぞれのボタン入力状況
    [System.NonSerialized] public bool isButton = false;

    //自身の担当プレイヤー名
    [System.NonSerialized] public string managementPlayerName = null;

    //答え
    [System.NonSerialized] public List<int> answer = new List<int>();

    //回答状況
    public List<bool> ClearSituation;

    private bool first1 = true;
    private bool firstInput = true;
    private bool firstSet = true;

    private void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        dataManager = ManagerAccessor.Instance.dataManager;

        //クリアすると動かさない
        if (!transform.parent.GetComponent<GimmickUnlockButtonManagement>().isAllClear)
        {
            //アンロックボタン開始
            if (islocalUnlockButtonStart)
            {
                if (InputCheck())
                {
                    if (firstInput)
                    {
                        firstInput = false;

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
                    }
                }
                else
                {
                    firstInput = true;
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

            if(transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount)
            {
                if (firstSet)
                {
                    if (HitNames.Count != 0)
                    {
                        //入力開始前に１回アンロックボタンの担当を設定
                        if (HitNames[0] == "Player1")
                            transform.parent.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayerName(0, ObjNum);
                        else if (HitNames[0] == "Player2")
                            transform.parent.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayerName(1, ObjNum);
                        else if (HitNames[0] == "CopyKey")
                            transform.parent.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayerName(2, ObjNum);

                        firstSet = false;
                    }
                }
            }
            //入力時間終了時、担当のプレイヤーが外される
            else
            {
                managementPlayerName = "";

                if (!transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 ||
                    !transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) 
                {
                    //タイムリミットと回答データ描画終了
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
                    transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
                    ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
                    islocalUnlockButtonStart = false;
                }

                firstSet = true;
            }

            //OnCollisionStay2Dを常に動かす処理
            rb2d.WakeUp();
        }

        //入力失敗時、入力情報リセット
        if(isAnswerReset)
        {
            for (int i = 0; i < ClearSituation.Count; i++) 
            {
                ClearSituation[i] = false;
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

            isAnswerReset = false;
        }

        if(isVibration)
        {
            if (vibrationCount < vibrationTime)
            {
                vibrationCount++;
                if (vibrationCount % 2 == 0)
                {
                    Debug.Log("0");
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[vibrationNum].transform.position += vibrationPower;
                }
                else
                {
                    Debug.Log("1");
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[vibrationNum].transform.position -= vibrationPower;
                }
            }
            else
            {
                isVibration = false;
                vibrationCount = 0;
            }
        }


        //ボタンに誰も触れていないときマネージャーに当たっていない判定を送る
        if (HitNames.Count == 0) 
        {
            if (first1)
            {
                if (ObjNum == 0)
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(true, false);
                else if (ObjNum == 1)
                    transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(false, false);
                first1 = false;
            }
        }
        else
        {
            first1 = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //触れたオブジェクトの名前記憶
            HitNames.Add(collision.gameObject.name);

            if (ObjNum == 0)
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(true, true);
            else if (ObjNum == 1)
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitUnlockButton(false, true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //クリアすると動かさない
        if (!transform.parent.GetComponent<GimmickUnlockButtonManagement>().isAllClear)
        {
            //入力開始時違うキャラが入力しないための処理
            if (collision.gameObject.name == managementPlayerName || managementPlayerName == "") 
            {
                if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
                {
                    //両ボタンにプレイヤーがいるとき
                    if ((transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 &&
                        transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) ||
                        transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount)
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
                    }
                }
                if (collision.gameObject.name == "Player2")
                {
                    //プレイヤー1が触れていないとき
                    if ((transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton1 &&
                        transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().isHitUnlockButton2) ||
                        transform.parent.GetComponent<GimmickUnlockButtonManagement>().isStartCount)
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
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //タイムリミットと回答データ描画終了
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayer(true, false);
        }
        if (collision.gameObject.name == "Player2")
        {
            //タイムリミットと回答データ描画終了
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().answerArea.SetActive(false);
            transform.parent.GetComponent<GimmickUnlockButtonManagement>().timeLimitSlider.SetActive(false);
            ManagerAccessor.Instance.dataManager.isUnlockButtonStart = false;
            islocalUnlockButtonStart = false;

            transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareHitPlayer(false, false);
        }


        //出たオブジェクトのタグを消去
        for (int i = 0; i < HitNames.Count; i++)
        {
            if (HitNames[i] == collision.gameObject.name)
            {
                HitNames.RemoveAt(i);
            }
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
                    CheckInputButton(dataManager.isOwnerInputKey_CA, i);
                    break;
                case (int)Key.B:
                    CheckInputButton(dataManager.isOwnerInputKey_CB, i);
                    break;
                case (int)Key.X:
                    CheckInputButton(dataManager.isOwnerInputKey_CX, i);
                    break;
                case (int)Key.Y:
                    CheckInputButton(dataManager.isOwnerInputKey_CY, i);
                    break;
                case (int)Key.Right:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_RIGHT, i);
                    break;
                case (int)Key.Left:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_LEFT, i);
                    break;
                case (int)Key.Up:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_UP, i);
                    break;
                case (int)Key.Down:
                    CheckInputButton(dataManager.isOwnerInputKey_C_D_DOWN, i);
                    break;
                case (int)Key.R1:
                    CheckInputButton(dataManager.isOwnerInputKey_C_R1, i);
                    break;
                case (int)Key.R2:
                    CheckInputButton(dataManager.isOwnerInputKey_C_R2, i);
                    break;
                case (int)Key.L1:
                    CheckInputButton(dataManager.isOwnerInputKey_C_L1, i);
                    break;
                case (int)Key.L2:
                    CheckInputButton(dataManager.isOwnerInputKey_C_L2, i);
                    break;
            }
        }
        else
        {
            switch (answer[i])
            {
                case (int)Key.A:
                    CheckInputButton(dataManager.isClientInputKey_CA, i);
                    break;
                case (int)Key.B:
                    CheckInputButton(dataManager.isClientInputKey_CB, i);
                    break;
                case (int)Key.X:
                    CheckInputButton(dataManager.isClientInputKey_CX, i);
                    break;
                case (int)Key.Y:
                    CheckInputButton(dataManager.isClientInputKey_CY, i);
                    break;
                case (int)Key.Right:
                    CheckInputButton(dataManager.isClientInputKey_C_D_RIGHT, i);
                    break;
                case (int)Key.Left:
                    CheckInputButton(dataManager.isClientInputKey_C_D_LEFT, i);
                    break;
                case (int)Key.Up:
                    CheckInputButton(dataManager.isClientInputKey_C_D_UP, i);
                    break;
                case (int)Key.Down:
                    CheckInputButton(dataManager.isClientInputKey_C_D_DOWN, i);
                    break;
                case (int)Key.R1:
                    CheckInputButton(dataManager.isClientInputKey_C_R1, i);
                    break;
                case (int)Key.R2:
                    CheckInputButton(dataManager.isClientInputKey_C_R2, i);
                    break;
                case (int)Key.L1:
                    CheckInputButton(dataManager.isClientInputKey_C_L1, i);
                    break;
                case (int)Key.L2:
                    CheckInputButton(dataManager.isClientInputKey_C_L2, i);
                    break;
            }
        }
    }

    //回答状況更新
    private void CheckInputButton(bool inputkey,int ansnum)
    {
        //成功
        if (inputkey)
        {
            ClearSituation[ansnum] = true;
            transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().clone[ansnum].GetComponent<Image>().color = new Color32(128, 128, 128, 255);

            //SE再生
            audioSource.PlayOneShot(successSE);
        }
        //失敗
        else
        {
            //答えのリセット
            isAnswerReset = true;

            //バイブレーション開始
            isVibration = true;
            vibrationNum = ansnum;

            //ミス情報カウント
            if (PhotonNetwork.IsMasterClient)
                ManagerAccessor.Instance.dataManager.ownerMissCount++;
            else
                transform.parent.gameObject.GetComponent<GimmickUnlockButtonManagement>().CallRpcShareInputMiss();

            //SE再生
            audioSource.PlayOneShot(failureSE);
        }
    }

    //コントローラーが入力されているかどうか
    private bool InputCheck()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (dataManager.isOwnerInputKey_CA || dataManager.isOwnerInputKey_CB || dataManager.isOwnerInputKey_CX || dataManager.isOwnerInputKey_CY ||
                dataManager.isOwnerInputKey_C_D_RIGHT || dataManager.isOwnerInputKey_C_D_LEFT || dataManager.isOwnerInputKey_C_D_UP || dataManager.isOwnerInputKey_C_D_DOWN ||
                dataManager.isOwnerInputKey_C_R1 || dataManager.isOwnerInputKey_C_R2 || dataManager.isOwnerInputKey_C_L1 || dataManager.isOwnerInputKey_C_L2) 
            {
                return true;
            }
        }
        else
        {
            if (dataManager.isClientInputKey_CA || dataManager.isClientInputKey_CB || dataManager.isClientInputKey_CX || dataManager.isClientInputKey_CY ||
                dataManager.isClientInputKey_C_D_RIGHT || dataManager.isClientInputKey_C_D_LEFT || dataManager.isClientInputKey_C_D_UP || dataManager.isClientInputKey_C_D_DOWN ||
                dataManager.isClientInputKey_C_R1 || dataManager.isClientInputKey_C_R2 || dataManager.isClientInputKey_C_L1 || dataManager.isClientInputKey_C_L2)
            {
                return true;
            }
        }

        return false;
    }

}
