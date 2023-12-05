using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickButton : MonoBehaviourPunCallbacks
{
    //それぞれのボタン入力状況
    [System.NonSerialized] public bool isButton = false;
    [System.NonSerialized] public bool isOwnerOnButton = false;
    [System.NonSerialized] public bool isClientOnButton = false;
    [System.NonSerialized] public bool isOwnerHit = false;
    [System.NonSerialized] public bool isClientHit = false;

    private Rigidbody2D rb2d;


    //最初しか反応しない処理
    private bool firstPushP1 = true;
    private bool firstPushP2 = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //OnCollisionStay2Dを常に動かす処理
        rb2d.WakeUp();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //押すべきボタンの画像表示
            if (PhotonNetwork.IsMasterClient)
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            //自身のオブジェクトが当たっている時しか反応させない
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB)
            {
                if (firstPushP1)
                {
                    //ボタン押している判定
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true, true, true);
                    firstPushP1 = false;
                }
            }
            else
            {
                if (!firstPushP1)
                {
                    //ボタン離した判定
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, true, false);
                    firstPushP1 = true;
                }
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像表示
            if (!PhotonNetwork.IsMasterClient)
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            //自身のオブジェクトが当たっている時しか反応させない
            if (ManagerAccessor.Instance.dataManager.isClientInputKey_CB)
            {
                if (firstPushP2)
                {
                    //ボタン押している判定
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, true, false, true);
                    firstPushP2 = false;
                }
            }
            else
            {
                if (!firstPushP2)
                {
                    //ボタン離した判定
                    photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, false, false);
                    firstPushP2 = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, true, true);
        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, false, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, true, false);

            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(false);


            //自身のオブジェクトが当たっている時しか反応させない
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //ボタンから離れた判定
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, true, false);
                firstPushP1 = true;
            }
        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsOwnerHit), RpcTarget.All, false, false);

            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(false);


            //自身のオブジェクトが当たっている時しか反応させない
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //ボタンから離れた判定
                photonView.RPC(nameof(RpcButtonCheck), RpcTarget.All, false, false, false);
                firstPushP2 = true;
            }
        }
    }


    //ボタン入力情報を相手に送信
    [PunRPC]
    protected void RpcButtonCheck(bool onButton, bool onOwner,bool data)
    {
        isButton = onButton;

        if (onOwner)
            isOwnerOnButton = data;
        else
            isClientOnButton = data;
        
    }

    //ボタン入力情報を相手に送信
    [PunRPC]
    protected void RpcShareIsOwnerHit(bool onOwner, bool data)
    {
        if (onOwner)
            isOwnerHit = data;
        else
            isClientHit = data;

    }
}
