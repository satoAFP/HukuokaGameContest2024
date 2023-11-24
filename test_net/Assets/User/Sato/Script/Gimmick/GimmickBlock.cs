using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //オブジェクトが持ち上がっているとき
    [System.NonSerialized] public bool liftMode = false;

    //両者の持ち上げ準備完了フラグ
    private bool isStart = false;

    //データマネージャー取得
    DataManager dataManager = null;

    private GameObject Player = null;

    //1P、2Pがそれぞれ当たっている判定
    private bool hitOwner = false;
    private bool hitClient = false;

    //ブロックとプレイヤーの距離
    private Vector3 dis = Vector3.zero;

    //連続で反応しないための処理
    private bool first = true;

    private void FixedUpdate()
    {
        dataManager = ManagerAccessor.Instance.dataManager;
        
        if (dataManager.player1 != null && dataManager.player2 != null) 
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (!dataManager.isAppearCopyKey)
                {
                    Player = dataManager.player1;
                }
                else
                {
                    Player = dataManager.copyKey;
                }
            }
            else
            {
                Player = dataManager.player2;
            }
            
            //1P、2Pが触れているかつ、アクションしているとき持ち上がる
            if (dataManager.isOwnerInputKey_CB && dataManager.isClientInputKey_CB) 
            {
                //持ち上げ準備完了
                if ((hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitLeft) ||
                    (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitRight))
                {
                    isStart = true;
                }

                //持ち上げ開始
                if (isStart)
                {
                    if (first)
                    {
                        //持ち上がった位置に移動
                        Vector3 input = gameObject.transform.position;
                        input.y += 1.2f;
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

                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!dataManager.isAppearCopyKey)
                        {
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = true;
                        }
                        else
                        {
                            ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().islift = true;
                        }
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = true;
                    }
                }
            }
            else
            {
                if (!first)
                {
                    //元の高さに戻す
                    Vector3 input = gameObject.transform.position;
                    input.y -= 1.2f;
                    gameObject.transform.localPosition = input;

                    dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                    first = true;
                    hitOwner = false;
                    hitClient = false;
                    isStart = false;

                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!dataManager.isAppearCopyKey)
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = false;
                        else
                            ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().islift = false;
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = false;
                    }
                }

                //同期解除
                GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                liftMode = false;
                
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
        {
            //押すべきボタンの画像表示
            if (PhotonNetwork.IsMasterClient && (dataManager.isOwnerHitRight || dataManager.isOwnerHitLeft)) 
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            //押すべきボタンの画像表示
            if (!PhotonNetwork.IsMasterClient && (dataManager.isClientHitRight || dataManager.isClientHitLeft))
            {
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.ArrowRight;
            }

            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //持ち上げていないとき
        if (!liftMode)
        {
            if (collision.gameObject.name == "Player1" || collision.gameObject.name == "CopyKey")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                //押すべきボタンの画像表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                hitClient = false;
            }
        }
    }

}
