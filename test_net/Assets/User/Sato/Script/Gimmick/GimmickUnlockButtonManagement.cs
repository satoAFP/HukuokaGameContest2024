using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GimmickUnlockButtonManagement : CGimmick
{
    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    [SerializeField, Header("入力する数")]
    private int inputKey;

    [SerializeField, Header("残り時間")]
    private Text timetext;

    [SerializeField, Header("入力時の制限時間")]
    private int timeLimit;

    //入力開始情報
    /*[System.NonSerialized]*/ public bool isStartCount = false;

    //それぞれの入力状況
    /*[System.NonSerialized]*/ public bool isOwnerClear = false;
    /*[System.NonSerialized]*/ public bool isClientClear = false;

    //入力開始情報
    /*[System.NonSerialized]*/ public bool isAllClear = false;

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
        A,B,X,Y
    }


    // Update is called once per frame
    void Update()
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
                        answer.Add(Random.Range(0, 4));
                        //連続で同じ数字にならないための処理
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
                    //答え入力用ブロックに答えデータを渡す
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;

                        //クリア状況初期化
                        for(int j=0;j<answer.Count;j++)
                        {
                            gimmickButton[i].GetComponent<GimmickUnlockButton>().ClearSituation.Add(false);
                        }
                    }
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
                if(isStartCountFisrt)
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

                timetext.text = frameCount.ToString() + "/" + timeLimit * 60;
            }
            else
            {
                if (!isStartCountFisrt)
                {
                    photonView.RPC(nameof(RpcShareIsStartCount), RpcTarget.Others, isStartCount);
                    isStartCountFisrt = true;
                }
            }




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

            if(isOwnerClear&&isClientClear)
            {
                isAllClear = true;
            }


        }
        else
        {
            door.SetActive(false);
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

    

}
