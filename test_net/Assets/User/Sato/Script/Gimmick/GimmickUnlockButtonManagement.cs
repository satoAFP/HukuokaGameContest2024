using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GimmickUnlockButtonManagement : CGimmick
{
    [SerializeField, Header("どのギミックにするか")]
    [Header("0:オブジェクト消失 / 1:オブジェクト出現")]
    private int gimmickNum;

    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    [SerializeField, Header("回答画像を格納するオブジェクト")]
    public GameObject answerArea;

    [SerializeField, Header("回答画像を格納するオブジェクト")]
    public GameObject timeLimitSlider;

    [SerializeField, Header("回答画像を複製するオブジェクト")]
    private GameObject initAnswer;

    [SerializeField, Header("入力する数")]
    private int inputKey;

    [SerializeField, Header("残り時間")]
    private Text timetext;

    [SerializeField, Header("入力時の制限時間")]
    private int timeLimit;

    [SerializeField, Header("答えを隠すかどうか")]
    private bool isHideAnswer;

    [SerializeField, Header("隠す場合どこを隠すか(チェック入れるとPlayer1の答えが隠される)")]
    private List<bool> isWhareHideAnswer;

    //どちらのプレイヤーが触れているか
    [System.NonSerialized] public bool isHitPlayer1 = false;
    [System.NonSerialized] public bool isHitPlayer2 = false;

    [System.NonSerialized] public bool isHitUnlockButton1 = false;
    [System.NonSerialized] public bool isHitUnlockButton2 = false;

    //入力開始情報
    [System.NonSerialized] public bool isStartCount = false;

    //それぞれの入力状況
    [System.NonSerialized] public bool isOwnerClear = false;
    [System.NonSerialized] public bool isClientClear = false;

    //入力開始情報
    [System.NonSerialized] public bool isAllClear = false;

    //回答データ
    private List<int> answer = new List<int>();

    private int frameCount = 0;


    //回答データ生成を1度しかしない用
    private bool isAnswerFirst = true;
    //アンロックボタン起動状態を連続で動かなない用
    private bool isUnlockButtonStartFirst = true;
    //クリア状況共有を連続で動かない用
    private bool isOwnerClearFirst = true;
    private bool isClientClearFirst = true;
    private bool isStartCountFisrt = true;


    private enum Key
    {
        A, B, X, Y, Right, Left, Up, Down, R1, R2, L1, L2
    }

    private void Start()
    {
        timeLimitSlider.GetComponent<Slider>().value = 1;

        //Gimmickによって扉の開閉を決める
        if (gimmickNum == 0)
            door.SetActive(true);
        if (gimmickNum == 1)
            door.SetActive(false);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //マスターの時答えを設定してデータを渡す
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //2Pがいない時は作らない
            if (ManagerAccessor.Instance.dataManager.player2 != null)
            {
                //最初の一回だけ
                if (isAnswerFirst)
                {
                    //答えの生成とデータの受け渡し
                    for (int i = 0; i < inputKey; i++)
                    {
                        answer.Add(Random.Range(0, 12));
                        //連続で同じ数字にならないための処理
                        while (true)
                        {
                            if (i != 0)
                            {
                                if (answer[i] == answer[i - 1])
                                    answer[i] = Random.Range(0, 12);
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, answer[i]);
                    }
                    ManagerAccessor.Instance.dataManager.chat.text = answer[0].ToString() + ":" + answer[1].ToString() + ":" + answer[2].ToString() + ":" + answer[3].ToString() + ":" + answer[4].ToString();

                    //答え設定
                    AnswerSet();

                    isAnswerFirst = false;
                }
            }
        }
        //マスターでない時、答えデータを受け取るまで待機
        else
        {
            if (answer.Count != 0)
            {
                //最初の一回だけ
                if (isAnswerFirst)
                {
                    ManagerAccessor.Instance.dataManager.chat.text = answer[0].ToString() + ":" + answer[1].ToString() + ":" + answer[2].ToString() + ":" + answer[3].ToString() + ":" + answer[4].ToString();
                    //答え設定
                    AnswerSet();

                    isAnswerFirst = false;
                }
            }
        }

        //クリアしていないとき
        if (!isAllClear)
        {
            //入力開始時の時間計算
            if (isStartCount)
            {
                if (isStartCountFisrt)
                {
                    photonView.RPC(nameof(RpcShareIsStartCount), RpcTarget.Others, isStartCount);
                    isStartCountFisrt = false;
                }

                frameCount++;
                if (frameCount == timeLimit * 60)
                {
                    //入力状況初期化
                    isStartCount = false;
                    frameCount = 0;

                    //ミス情報カウント
                    if (!isOwnerClear)
                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                    if (!isClientClear)
                        CallRpcShareInputMiss();

                    //クリア状況初期化
                    isOwnerClear = false;
                    isClientClear = false;

                    //二つ分初期化
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        //クリア状況初期化
                        for (int j = 0; j < answer.Count; j++)
                        {
                            gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation[j] = false;
                        }
                    }
                }

                //残り時間表示
                timeLimitSlider.GetComponent<Slider>().value = 1 - (float)frameCount / (float)(timeLimit * 60);
                //timetext.text = frameCount.ToString() + "/" + timeLimit * 60;
            }
            else
            {
                if (!isStartCountFisrt)
                {
                    photonView.RPC(nameof(RpcShareIsStartCount), RpcTarget.Others, isStartCount);
                    isStartCountFisrt = true;
                }
            }



            //Player1のクリア情報送信
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (isOwnerClear)
                {
                    if (isOwnerClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, true);
                        isOwnerClearFirst = false;
                    }
                }
                else
                {
                    if (!isOwnerClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, false);
                        isOwnerClearFirst = true;
                    }
                }
            }
            //Player2のクリア情報送信
            else
            {
                if (isClientClear)
                {
                    if (isClientClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, true);
                        isClientClearFirst = false;
                    }
                }
                else
                {
                    if (!isClientClearFirst)
                    {
                        photonView.RPC(nameof(RpcShareIsClear), RpcTarget.Others, false);
                        isClientClearFirst = true;
                    }
                }
            }

            //両方クリアしたらこの処理を抜ける
            if (isOwnerClear && isClientClear)
            {
                isAllClear = true;
            }


        }
        else
        {
            //解除成功
            if (gimmickNum == 0)
                door.SetActive(false);
            if (gimmickNum == 1)
                door.SetActive(true);

            answerArea.SetActive(false);
            timeLimitSlider.SetActive(false);
        }

    }


    //答え設定用関数
    private void AnswerSet()
    {
        GameObject clone = null;
        SpriteManager spriteManager = ManagerAccessor.Instance.spriteManager;

        //答え入力用ブロックに答えデータを渡す
        for (int i = 0; i < gimmickButton.Count; i++)
        {
            gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;

            //クリア状況初期化
            for (int j = 0; j < answer.Count; j++)
            {
                gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation.Add(false);
            }
        }

        //回答描画用
        for (int i = 0; i < answer.Count; i++)
        {
            switch (answer[i])
            {
                case (int)Key.A:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowDown;
                    break;
                case (int)Key.B:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowRight;
                    break;
                case (int)Key.X:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowLeft;
                    break;
                case (int)Key.Y:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.ArrowUp;
                    break;
                case (int)Key.Right:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossRight;
                    break;
                case (int)Key.Left:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossLeft;
                    break;
                case (int)Key.Up:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossUp;
                    break;
                case (int)Key.Down:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.CrossDown;
                    break;
                case (int)Key.R1:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.R1;
                    break;
                case (int)Key.R2:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.R2;
                    break;
                case (int)Key.L1:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.L1;
                    break;
                case (int)Key.L2:
                    clone = Instantiate(initAnswer);
                    clone.gameObject.transform.parent = answerArea.transform;
                    clone.GetComponent<Image>().sprite = spriteManager.L2;
                    break;
            }
        }
    }


    //マスターサイドで決めた答えを共有
    [PunRPC]
    private void RpcShareAnswer(int ans)
    {
        answer.Add(ans);
    }

    //isUnlockButtonStartを共有
    [PunRPC]
    private void RpcShareIsUnlockButtonStart(bool data)
    {
        ManagerAccessor.Instance.dataManager.isUnlockButtonStart = data;
    }

    //isStartCountを共有
    [PunRPC]
    private void RpcShareIsStartCount(bool data)
    {
        isStartCount = data;
    }


    //クリア状況を共有
    [PunRPC]
    private void RpcShareIsClear(bool data)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            isClientClear = data;
        }
        else
        {
            isOwnerClear = data;
        }
    }

    //ボタンにプレイヤーが触れているかどうか
    public void CallRpcShareHitUnlockButton(bool button1, bool data)
    {
        photonView.RPC(nameof(RpcShareHitUnlockButton), RpcTarget.All, button1, data);
    }
    [PunRPC]
    private void RpcShareHitUnlockButton(bool button1, bool data)
    {
        if (button1)
            isHitUnlockButton1 = data;
        else
            isHitUnlockButton2 = data;
    }

    //プレイヤーが触れている状態かどうか共有
    public void CallRpcShareHitPlayer(bool isowner, bool data)
    {
        photonView.RPC(nameof(RpcShareHitPlayer), RpcTarget.All, isowner, data);
    }
    [PunRPC]
    private void RpcShareHitPlayer(bool isowner, bool data)
    {
        if (isowner)
            isHitPlayer1 = data;
        else
            isHitPlayer2 = data;
    }

    //プレイヤーが触れている状態かどうか共有
    public void CallRpcShareHitPlayerName(int name,int objNum)
    {
        photonView.RPC(nameof(RpcShareHitPlayerName), RpcTarget.All, name, objNum);
    }
    [PunRPC]
    private void RpcShareHitPlayerName(int name, int objNum)
    {
        if (name == 0)
            gimmickButton[objNum].GetComponent<GimmickUnlockButton>().managementPlayerName = "Player1";
        if (name == 1)
            gimmickButton[objNum].GetComponent<GimmickUnlockButton>().managementPlayerName = "Player2";
        if (name == 2)
            gimmickButton[objNum].GetComponent<GimmickUnlockButton>().managementPlayerName = "CopyKey";
    }


    //クライアント側でミスったらオーナー側で加算
    public void CallRpcShareInputMiss()
    {
        photonView.RPC(nameof(RpcShareInputMiss), RpcTarget.Others);
    }
    [PunRPC]
    private void RpcShareInputMiss()
    {
        ManagerAccessor.Instance.dataManager.clientMissCount++;
    }
}
