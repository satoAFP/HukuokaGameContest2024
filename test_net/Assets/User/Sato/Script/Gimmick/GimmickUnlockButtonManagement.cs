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
                        photonView.RPC(nameof(RpcShareAnswer), RpcTarget.Others, answer[i]);
                    }

                    //答え入力用ブロックに答えデータを渡す
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;
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
                    //答え入力用ブロックに答えデータを渡す
                    for (int i = 0; i < gimmickButton.Count; i++)
                    {
                        gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;
                    }
                    isAnswerFirst = false;
                }
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


}
