using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    [SerializeField, Header("持ち上げSE")] AudioClip liftSE;
    [SerializeField, Header("失敗SE")] AudioClip failureSE;

    private AudioSource audioSource;

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

    //成功判定
    private bool isSuccess = false;
    //失敗判定
    private bool isFailure = false;

    //フレームカウント用
    private int count = 0;

    //連続で反応しないための処理
    private bool first = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        dataManager = ManagerAccessor.Instance.dataManager;
        
        //両プレイヤーがいるとき
        if (dataManager.player1 != null && dataManager.player2 != null) 
        {
            //取得すべきプレイヤー
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

             
            //どちらかが持ち上げようとした場合
            if (dataManager.isOwnerInputKey_CB || dataManager.isClientInputKey_CB)
            {
                if (!isSuccess)
                {
                    //両方触れている場合又は、持ち上げが開始した場合
                    if ((hitOwner && hitClient && dataManager.isOwnerHitRight && dataManager.isClientHitLeft) ||
                        (hitOwner && hitClient && dataManager.isOwnerHitLeft && dataManager.isClientHitRight) ||
                        isStart)
                    {
                        if (!isFailure)
                        {
                            //失敗までのフレームまで
                            if (count <= ManagerAccessor.Instance.dataManager.MissFrame)
                            {
                                count++;
                                //1P、2Pが触れているかつ、アクションしているとき持ち上がる
                                if (dataManager.isOwnerInputKey_CB && dataManager.isClientInputKey_CB)
                                {
                                    //成功
                                    isSuccess = true;
                                }

                            }
                            else
                            {
                                //失敗情報送信
                                if (PhotonNetwork.IsMasterClient)
                                {
                                    if (dataManager.isOwnerInputKey_CB)
                                        ManagerAccessor.Instance.dataManager.clientMissCount++;
                                    if (dataManager.isClientInputKey_CB)
                                        ManagerAccessor.Instance.dataManager.ownerMissCount++;
                                }

                                //SE再生
                                audioSource.PlayOneShot(failureSE);

                                isFailure = true;
                                count = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                isSuccess = false;
                isFailure = false;
            }

            //成功時
            if (isSuccess)
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

                        //SE再生
                        audioSource.PlayOneShot(liftSE);
                    }

                    //プレイヤーに追従させる
                    gameObject.transform.position = dis + Player.transform.position;

                    //プレイヤーが動いているとき、ブロックサイドも同期させる
                    if (Player.GetComponent<AvatarTransformView>().isPlayerMove)
                        GetComponent<AvatarOnlyTransformView>().isPlayerMove = true;
                    else
                        GetComponent<AvatarOnlyTransformView>().isPlayerMove = false;

                    liftMode = true;

                    //持ち上げている判定
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

                    if (!((dataManager.isOwnerInputKey_C_L_RIGHT && dataManager.isClientInputKey_C_L_RIGHT) ||
                       (dataManager.isOwnerInputKey_C_L_LEFT && dataManager.isClientInputKey_C_L_LEFT)))
                    {
                        //持ち上げている判定
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (!dataManager.isAppearCopyKey)
                            {
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = false;
                            }
                            else
                            {
                                ManagerAccessor.Instance.dataManager.copyKey.GetComponent<Rigidbody2D>().simulated = false;
                            }
                        }
                        else
                        {
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = false;
                        }
                    }
                    Debug.Log("1"+ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated);
                    Debug.Log("2" + ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated);

                    if ((dataManager.isOwnerInputKey_C_L_RIGHT && dataManager.isClientInputKey_C_L_RIGHT) ||
                       (dataManager.isOwnerInputKey_C_L_LEFT && dataManager.isClientInputKey_C_L_LEFT))
                    {
                        //持ち上げている判定
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (!dataManager.isAppearCopyKey)
                            {
                                ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = true;
                            }
                            else
                            {
                                ManagerAccessor.Instance.dataManager.copyKey.GetComponent<Rigidbody2D>().simulated = true;
                            }
                        }
                        else
                        {
                            ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = true;
                        }
                    }
                }
            }


            //持ち上げ終了時
            if (!dataManager.isOwnerInputKey_CB || !dataManager.isClientInputKey_CB)
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

                    //SE再生
                    audioSource.PlayOneShot(liftSE);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (!dataManager.isAppearCopyKey)
                        {
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = false;
                            ManagerAccessor.Instance.dataManager.player1.GetComponent<Rigidbody2D>().simulated = true;
                        }
                        else
                        {
                            ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().islift = false;
                            ManagerAccessor.Instance.dataManager.copyKey.GetComponent<Rigidbody2D>().simulated = true;
                        }
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = false;
                        ManagerAccessor.Instance.dataManager.player2.GetComponent<Rigidbody2D>().simulated = true;
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
                //押すべきボタンの画像非表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                //押すべきボタンの画像非表示
                collision.transform.GetChild(0).gameObject.SetActive(false);

                hitClient = false;
            }
        }
    }

}
