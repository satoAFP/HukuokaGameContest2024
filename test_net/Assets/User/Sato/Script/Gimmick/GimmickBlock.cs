using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //オブジェクトが持ち上がっているとき
    [System.NonSerialized] public bool liftMode = false;

    private GameObject Player = null;

    //1P、2Pがそれぞれ当たっている判定
    private bool hitOwner = false;
    private bool hitClient = false;

    //ブロックとプレイヤーの距離
    private Vector3 dis = Vector3.zero;

    //連続で反応しないための処理
    private bool first = true;

    int a = 0;

    private void FixedUpdate()
    {
        if (ManagerAccessor.Instance.dataManager.player1 != null && ManagerAccessor.Instance.dataManager.player2 != null) 
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player = ManagerAccessor.Instance.dataManager.player1;
                Debug.Log(a + "p1");
            }
            else
            {
                Player = ManagerAccessor.Instance.dataManager.player2;
                Debug.Log(a + "p2");
            }

            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ManagerAccessor.Instance.dataManager.chat.text = hitOwner + ":" + hitClient + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + liftMode;
                Debug.Log(hitOwner + ":" + hitClient + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB + ":" + liftMode);

                if (hitClient)
                {
                    Debug.Log(a + "あたってる");
                }
                else
                { 
                    Debug.Log(a + "あたってない");
                }
            }

            //1P、2Pが触れているかつ、アクションしているとき持ち上がる
            if (hitOwner && ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB &&
            hitClient && ManagerAccessor.Instance.dataManager.isClientInputKey_CB)
            {
                if (first)
                {
                    //持ち上がった位置に移動
                    Vector3 input = gameObject.transform.position;
                    input.y += 1.0f;
                    gameObject.transform.localPosition = input;

                    dis = transform.position - Player.transform.position;

                    first = false;
                }

                //プレイヤーに追従させる
                gameObject.transform.position = dis + Player.transform.position;

                //プレイヤーが動いているとき、ブロックサイドも同期させる
                if (Player.GetComponent<AvatarTransformView>().isPlayerMove)
                    GetComponent<AvatarOnlyTransformView>().isPlayerMove = true;
                else
                    GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                liftMode = true;
                Player.GetComponent<PlayerController>().islift = true;
            }
            else
            {
                if (!first)
                {
                    Debug.Log("ccc");
                    //元の高さに戻す
                    Vector3 input = gameObject.transform.position;
                    input.y -= 1.0f;
                    gameObject.transform.localPosition = input;

                    dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                    first = true;
                    photonView.RPC(nameof(RpcShareIsOwner), RpcTarget.All, false);
                    photonView.RPC(nameof(RpcShareIsClient), RpcTarget.All, false);
                }

                //同期解除
                GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                liftMode = false;
                hitOwner = false;
                hitClient = false;
                Player.GetComponent<PlayerController>().islift = false;
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

            photonView.RPC(nameof(RpcShareIsOwner), RpcTarget.All, true);
        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像表示
            collision.transform.GetChild(0).gameObject.SetActive(true);
            collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;

            photonView.RPC(nameof(RpcShareIsClient), RpcTarget.All, true);
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //持ち上げていないとき
        if (!liftMode)
        {
            if (collision.gameObject.name == "Player1")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsOwner), RpcTarget.All, false);
            }

            if (collision.gameObject.name == "Player2")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                photonView.RPC(nameof(RpcShareIsClient), RpcTarget.All, false);
            }
        }
    }

    [PunRPC]
    private void RpcShareIsOwner(bool data)
    {
        hitOwner = data;
    }

    [PunRPC]
    private void RpcShareIsClient(bool data)
    {
        hitClient = data;
    }

}
