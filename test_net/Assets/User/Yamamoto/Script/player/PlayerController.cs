using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("宝箱")]
    private Sprite p1Image;
    [SerializeField, Header("空いた宝箱")]
    private Sprite p1OpenImage;
   
    [SerializeField, Header("鍵")]
    private Sprite p2Image;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;

    [SerializeField, Header("板オブジェクト")]
    private GameObject boardobj;

    [SerializeField, Header("鍵オブジェクト")]
    private GameObject copykeyobj;

    [SerializeField]
    private GameObject currentBoardObject;// 現在の生成された板オブジェクト

    [SerializeField]
    private GameObject currentCopyKeyObject;// 現在の生成された鍵オブジェクト

    private bool movelock = false;//移動処理を停止させる

    private bool left = false;//左向きに移動したときのフラグ

    private bool B_instantiatefirst = true;//連続でアイテムを生成させない(板）

    private bool CK_instantiatefirst = true;//連続でアイテムを生成させない(鍵）

    [System.NonSerialized] public bool boxopen = false;//箱の開閉を許可するフラグ

    [System.NonSerialized] public bool cursorlock = true;//UIカーソルの移動を制限する

    [System.NonSerialized] public string choicecursor;//UIカーソルが現在選択している生成可能アイテム

    [System.NonSerialized] public bool generatestop = false;//生成を制御する

    [System.NonSerialized] public bool keymovelock = false;//生成した鍵の移動を制御

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //各プレイヤーの座標
    private Vector2 p1pos;
    private Vector2 p2pos;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    private bool bjump;//連続でジャンプさせないフラグ

    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    [System.NonSerialized] public bool islift = false;//持ち上げフラグ

    [System.NonSerialized] public bool isliftfirst = true;//持ち上げフラグの状態を送信するとき一回しか送信しないため

    [System.NonSerialized] public bool isFly = false;//ロケット乗り込みフラグフラグ

    //物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
    private bool distanceFirst = true;
    private Vector3 dis = Vector3.zero;

    void Start()
    {

        choicecursor = "Board";//生成可能アイテム初期化

        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        //名前とIDを設定
        gameObject.name = "Player" + photonView.OwnerActorNr;

        //プレイヤーによってイラストを変える＆データマネージャー設定
        if (gameObject.name == "Player1")
        {
            GetComponent<SpriteRenderer>().sprite = p1Image;
            ManagerAccessor.Instance.dataManager.player1 = ManagerAccessor.Instance.dataManager.GetPlyerObj("Player1");
        }
        if (gameObject.name == "Player2")
        {
            GetComponent<SpriteRenderer>().sprite = p2Image;
            ManagerAccessor.Instance.dataManager.player2 = ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2");
        }

        test_net = new Test_net();//スクリプトを変数に格納

      
    }
    void FixedUpdate()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //Debug.Log("isOwnerHitDown=" + ManagerAccessor.Instance.dataManager.isOwnerHitDown);
        //Debug.Log("isClientHitDown=" + ManagerAccessor.Instance.dataManager.isClientHitDown);


        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            ////1Pの画面の2Pの情報更新
            //if (PhotonNetwork.LocalPlayer.IsMasterClient)
            //    if (ManagerAccessor.Instance.dataManager.player2 != null)
            //        ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = islift;

            ////1Pの画面の2Pの情報更新
            //if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            //    if (ManagerAccessor.Instance.dataManager.player1 != null)
            //        ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().islift = islift;

            //持ち上げていないときは普通に移動させる
            if (!islift)
            {
                //飛ぶ状態に入っていない時
                if (!isFly)
                {
                    Move();//移動処理をON
                }
                distanceFirst = true;
            }
            else
            {
                //持ち上げている時は2プレイヤーが同じ移動方向を入力時移動
                if ((datamanager.isOwnerInputKey_C_L_RIGHT&& datamanager.isClientInputKey_C_L_RIGHT)||
                   (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT))
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        Move();
                    }
                    else
                    {
                        //物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
                        if (distanceFirst)
                        {
                            //1Pと2Pの座標の差を記憶
                            dis = datamanager.player1.transform.position - datamanager.player2.transform.position;
                            distanceFirst = false;
                        }

                        //2Pが1Pに追従するようにする
                        transform.position = datamanager.player1.transform.position - dis;
                    }
                }
            }

            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //プレイヤー1（箱）の移動が制限されているとき（箱が空いている時）
                if(movelock)
                {
                    //生成アイテムがマップ上にないときのみ箱を閉じる（移動制限解除）
                    if (currentBoardObject == null &&
                         currentCopyKeyObject == null)
                    {
                        boxopen = true;
                    }
                    else
                    {
                        boxopen = false;
                    }

                        //コントローラーの下ボタンを押したとき箱を閉じる
                        if (datamanager.isOwnerInputKey_CA)
                    {
                        //箱を閉じて移動ロックを解除
                        if (gameObject.name == "Player1" && boxopen)
                        {
                            Debug.Log("おぺん");
                            GetComponent<SpriteRenderer>().sprite = p1Image;
                            cursorlock = true;//カーソル移動を止める
                            movelock = false;
                        }
                    }
                  
                    //ゲームパッド右ボタンでアイテム生成
                    //板
                    if (datamanager.isOwnerInputKey_CB &&
                        choicecursor== "Board")
                    {
                        if (B_instantiatefirst)
                        {
                            if (currentBoardObject == null)
                            {
                                currentBoardObject = PhotonNetwork.Instantiate("Board", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                                movelock = true;

                                //先に鍵が生成されていた場合
                                if(currentCopyKeyObject!=null)
                                {
                                    keymovelock = true;//板にオブジェクト移動の主導権を渡す
                                }
                                else
                                {
                                    generatestop = true;//板が先の場合、板の移動が終わるまで鍵生成をさせない
                                }
                              
                              //  Debug.Log("p1側生成");
                            }
                            B_instantiatefirst = false;
                        }

                    }
                    else
                    {
                        B_instantiatefirst = true;
                    }
                    //鍵
                    if (datamanager.isOwnerInputKey_CB &&
                       choicecursor == "CopyKey")
                    {
                        if (CK_instantiatefirst && !generatestop)
                        {
                            if (currentCopyKeyObject == null)
                            {
                                currentCopyKeyObject = PhotonNetwork.Instantiate("CopyKey", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                                movelock = true;

                            }
                            CK_instantiatefirst = false;
                        }

                    }
                    else
                    {
                        CK_instantiatefirst = true;
                    }
                }
               
            }
 
        }
        else
        {
            //持ち上げている時は2プレイヤーが同じ移動方向を入力時移動
            if ((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
               (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT))
            {
                if (islift)
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        //物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
                        if (distanceFirst)
                        {
                            //1Pと2Pの座標の差を記憶
                            dis = datamanager.player1.transform.position - datamanager.player2.transform.position;
                            distanceFirst = false;
                        }

                        //2Pが1Pに追従するようにする
                        transform.position = datamanager.player1.transform.position - dis;
                    }

                }
            }
            else
            {
                distanceFirst = true;
            }

            //コントローラーの下ボタンを押したとき箱処理中断（相手側）
            if (datamanager.isOwnerInputKey_CA &&　movelock)
            {
                //同時に上ボタンを押していないときは画像を元に戻す
                if (gameObject.name == "Player1"&& boxopen)
                {
                   // Debug.Log("おぺん22");
                    GetComponent<SpriteRenderer>().sprite = p1Image;
                    boxopen = false;
                }
            }
         
        }

        //各プレイヤーの現在座標を取得
        p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;
        //Debug.Log("p1現在地=" + p1pos);
        if (ManagerAccessor.Instance.dataManager.player2 != null)
            p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;
        //Debug.Log("p2現在地=" + p2pos);

        // Debug.Log(Mathf.Abs(p1pos.x - p2pos.x));


        //箱と鍵の二点間距離を取って一定の値なら箱オープン可能
        if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
        {
            //上ボタンの同時押しで箱オープン
            if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP)
            {
                //Debug.Log("上キー両押し");
                //宝箱のプレイヤーの時、空いている箱のイラストに変更
                if (gameObject.name == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1OpenImage;
                    movelock = true;//箱の移動を制限
                    cursorlock = false;//UIカーソル移動を許可
                }

            }
        }

    }




    private void Move()//移動処理（計算部分）
    {
        //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        //Debug.Log(inputDirection.x);
        //移動方向によって画像の向きを変える
        if(inputDirection.x < 0)
        {
           // left = true;
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            //left = false;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーが床または着地出来るものに乗っている時、再ジャンプ可能にする
        if (collision.gameObject.tag == "Floor")
        {
            bjump = false;
        }
    }

    //playerinputで起動させる関数
    //移動処理
    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameObject.name == "Player2")
        {
            Debug.Log("プレイヤー2認識");
        }


        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            //移動ロックがかかっていなければ移動
            if(!movelock)
            {
                Debug.Log("スティック動かして移動している");
                //移動方向の入力情報がInputdirectionの中に入るようになる
                inputDirection = context.ReadValue<Vector2>();

            }

        }
        else
        {
            Debug.Log("識別できてない");
        }
    }

    //ジャンプ
    public void Onjump(InputAction.CallbackContext context)
    {
        //アンロックボタン、ロケットが起動中でない時
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock && !isFly) 
        {
            //Input Systemからジャンプの入力があった時に呼ばれる
            //連続でジャンプできないようにする
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (!context.performed || !ManagerAccessor.Instance.dataManager.isOwnerHitDown)
                {
                    return;
                }
            }
            else
            {
                if (!context.performed || !ManagerAccessor.Instance.dataManager.isClientHitDown)
                {
                    return;
                }
            }
           

            //操作が競合しないための設定
            if (photonView.IsMine)
            {
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                bjump = true;//一度ジャンプしたら着地するまでジャンプできなくする
            }
        }
    }
}
