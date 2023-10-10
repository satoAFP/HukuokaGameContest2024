using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickButton : CGimmick
{
    //それぞれのボタン入力状況
    [System.NonSerialized] public bool isButton = false;

    //最初しか反応しない処理
    private bool firstPushP1 = true;
    private bool firstPushP2 = true;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //自身のオブジェクトが当たっている時しか反応させない
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    if (firstPushP1)
                    {
                        //ボタン押している判定
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true);
                        firstPushP1 = false;
                    }
                }
                else
                {
                    if (!firstPushP1)
                    {
                        //ボタン離した判定
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                        firstPushP1 = true;
                    }
                }
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            //自身のオブジェクトが当たっている時しか反応させない
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    if (firstPushP2)
                    {
                        //ボタン押している判定
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true);
                        firstPushP2 = false;
                    }
                }
                else
                {
                    if (!firstPushP2)
                    {
                        //ボタン離した判定
                        photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                        firstPushP2 = true;
                    }
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //自身のオブジェクトが当たっている時しか反応させない
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //ボタンから離れた判定
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                firstPushP1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            //自身のオブジェクトが当たっている時しか反応させない
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //ボタンから離れた判定
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false);
                firstPushP2 = true;
            }
        }
    }


    //ボタン入力情報を相手に送信
    [PunRPC]
    protected void RpcButtonCheck(bool onButton)
    {
        isButton = onButton;
    }
}
