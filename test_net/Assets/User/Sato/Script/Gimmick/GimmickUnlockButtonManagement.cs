using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickUnlockButtonManagement : CGimmick
{
    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    [SerializeField, Header("入力する数")]
    private int inputKey;

    //回答データ
    private List<int> answer = new List<int>();

    //回答データ生成を1度しかしない用
    private bool isAnswerFirst = true;
    //アンロックボタン起動状態を連続で動かなない用
    private bool isUnlockButtonStartFirst = true;

    private enum Key
    {
        A,B,X,Y
    }

    // Start is called before the first frame update
    void Start()
    {

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

        //isUnlockButtonStartのデータ共有処理
        if (ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
        {
            if(isUnlockButtonStartFirst)
            {
                photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, ManagerAccessor.Instance.dataManager.isUnlockButtonStart);
                isUnlockButtonStartFirst = false;
            }
        }
        else
        {
            if (!isUnlockButtonStartFirst)
            {
                photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, ManagerAccessor.Instance.dataManager.isUnlockButtonStart);
                isUnlockButtonStartFirst = true;
            }
        }


        ////ボタンが押されているオブジェクトの数カウント用
        //int count = 0;

        ////ボタンの数だけ回す
        //for (int i = 0; i < gimmickButton.Count; i++)
        //{
        //    if (gimmickButton[i].GetComponent<GimmickUnlockButton>().isButton == true)
        //    {
        //        count++;
        //    }
        //}

        ////同時押しが成功すると、扉が開く
        //if (gimmickButton.Count == count)
        //{
        //    door.SetActive(false);
        //}


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
}
