using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    
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

    private bool left1P = false;//左向きに移動したときのフラグ(1P用）
    private bool left2P = false;//左向きに移動したときのフラグ(2P用）

    private bool B_instantiatefirst = true;//連続でアイテムを生成させない(板）

    private bool CK_instantiatefirst = true;//連続でアイテムを生成させない(鍵）

    private bool firstboxopen = true;//箱の閉じるフラグ共有を一度だけする

    private bool firstLR_1P = true;//左右移動一度だけ処理を行う(1P用）
    private bool firstLR_2P = true;//左右移動一度だけ処理を行う(2P用）

    private bool firstchange_boximage = true;//一度だけ箱を開くフラグ共有をさせる

    private bool firstmovelock = true;//一度だけ移動制限を行う

    [System.NonSerialized] public bool boxopen = false;//箱の開閉を許可するフラグ

    [System.NonSerialized] public bool cursorlock = true;//UIカーソルの移動を制限する

    [System.NonSerialized] public string choicecursor;//UIカーソルが現在選択している生成可能アイテム

    [System.NonSerialized] public bool generatestop = false;//生成を制御する

    [System.NonSerialized] public bool keymovelock = false;//生成した鍵の移動を制御]


    [System.NonSerialized] public bool change_boxopenimage = false;//プレイヤー画像を箱を空ける画像に変更
    [System.NonSerialized] public bool change_liftimage = false;//プレイヤーの画像をブロックを持ち上げたときの画像に変更
    [System.NonSerialized] public bool change_unloadimage = false;//ブロックをおろした時プレイヤーの画像を元に戻す

    [System.NonSerialized] public bool animplay = false;//アニメーションを再生
     private bool firstanimplay = true;

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //各プレイヤーの座標
    private Vector2 p1pos;
    private Vector2 p2pos;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    [System.NonSerialized] public bool bjump;//連続でジャンプさせないフラグ

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
           // GetComponent<SpriteRenderer>().sprite = p1Image;
            ManagerAccessor.Instance.dataManager.player1 = gameObject;
        }
        if (gameObject.name == "Player2")
        {
            //GetComponent<SpriteRenderer>().sprite = p2Image;
            ManagerAccessor.Instance.dataManager.player2 = gameObject;
        }
        if (gameObject.name == "CopyKey")
        {
            //GetComponent<SpriteRenderer>().sprite = p2Image;
            ManagerAccessor.Instance.dataManager.copyKey = gameObject;
        }

        test_net = new Test_net();//スクリプトを変数に格納
    }
    void FixedUpdate()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        if (firstLR_1P)
        {
            //プレイヤーの左右の向きを変える
            if (datamanager.isOwnerInputKey_C_L_LEFT && !movelock)
            {
                Debug.Log("左いいいい");
                left1P = true;
                firstLR_1P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
            else if (datamanager.isOwnerInputKey_C_L_RIGHT && !movelock)
            {
                Debug.Log("右いいいい");
                left1P = false;
                firstLR_1P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
 
        }

        if (firstLR_2P)
        {
            if (datamanager.isClientInputKey_C_L_LEFT)
            {
                Debug.Log("2P左いいいい");
                left2P = true;
                firstLR_2P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
            else if (datamanager.isClientInputKey_C_L_RIGHT)
            {
                Debug.Log("2P右いいいい");
                left2P = false;
                firstLR_2P = false;
                photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
            }
        }

          

        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            //持ち上げていないときは普通に移動させる
            if (!islift)
            {
                //飛ぶ状態に入っていない時
                if (!isFly)
                {
                    Move();//移動処理をON
                }

                //宝箱が空いていないときは通常の画像に戻す
                if (!change_boxopenimage && !movelock)
                {
                   // Debug.Log("ashidaka");
                    photonView.RPC(nameof(RpcChangeUnloadImage), RpcTarget.All);
                }

                distanceFirst = true;
            }
            else
            {
                //各プレイヤーを持ち上げイラストに変更
                photonView.RPC(nameof(RpcChangeLiftImage), RpcTarget.All);

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
                            if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                                dis = datamanager.player1.transform.position - gameObject.transform.position;
                            else
                                dis = datamanager.copyKey.transform.position - gameObject.transform.position;

                            distanceFirst = false;  
                        }

                        //2Pが1Pに追従するようにする
                        if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                            transform.position = datamanager.player1.transform.position - dis;
                        else
                            transform.position = datamanager.copyKey.transform.position - dis;
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
                        if(firstboxopen)
                        {
                            //boxopen関数を共有する
                           // Debug.Log("firstboxopenが入ってる");
                            photonView.RPC(nameof(RpcShareBoxOpen), RpcTarget.All,true);
                            firstboxopen = false;
                        }
                       
                    }
                    else
                    {
                        if (!firstboxopen)
                        {
                            //boxopen関数を共有する
                            photonView.RPC(nameof(RpcShareBoxOpen), RpcTarget.All, false);
                            firstboxopen = true;
                        }
                           
                    }

                    //コントローラーの下ボタンを押したとき箱を閉じる
                    if (datamanager.isOwnerInputKey_C_D_DOWN
                         && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
                    {
                        //箱を閉じて移動ロックを解除
                        if (gameObject.name == "Player1" && boxopen)
                        {
                            //Debug.Log("おぺん");
                            change_boxopenimage = false;//箱を閉じた画像にする
                            cursorlock = true;//カーソル移動を止める
                            if(!firstmovelock)
                            {
                                photonView.RPC(nameof(RpcShareMoveLock), RpcTarget.All, false);//箱の移動の制限解除
                                firstmovelock = true;
                            }
                            //movelock = false;
                            GetComponent<PlayerGetHitObjTagManagement>().isMotion = true;//箱の周りの判定をとるのを再開
                            firstboxopen = true;//boxopenフラグ共有再会
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
                               // movelock = true;
                                //Debug.Log("板だす");

                                //先に鍵が生成されていた場合
                                if (currentCopyKeyObject!=null)
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
                                //movelock = true;

                                //コピー鍵出現中フラグ
                                ManagerAccessor.Instance.dataManager.isAppearCopyKey = true;
                               // Debug.Log("鍵だす");

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

            if (movelock)
            {
                //コントローラーの下ボタンを押したとき箱処理中断（相手側）
                if (datamanager.isOwnerInputKey_CA)
                {

                    //同時に上ボタンを押していないときは画像を元に戻す
                    if (gameObject.name == "Player1" && boxopen)
                    {
                       // Debug.Log("おぺん22");
                        //GetComponent<SpriteRenderer>().sprite = p1Image;
                        //change_boxopenimage = false;//箱を閉じた画像にする
                        //boxopen = false;
                    }
                }
            }
        }

       

        //各プレイヤーの現在座標を取得
        p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;

        if (ManagerAccessor.Instance.dataManager.player2 != null)
            p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;
      
        //箱と鍵の二点間距離を取って一定の値なら箱オープン可能
        if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
        {
            //上ボタンの同時押しで箱オープン
            if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP
                && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
            {
                //Debug.Log("上キー両押し");
                //宝箱のプレイヤーの時、空いている箱のイラストに変更
                if (gameObject.name == "Player1")
                {
                    if(firstchange_boximage)
                    {
                        photonView.RPC(nameof(RpcChangeBoxOpenImage), RpcTarget.All);//箱を空けるイラスト変更フラグを送信
                        firstchange_boximage = false;
                    }
                   
                    //movelock = true;//箱の移動を制限
                    if(firstmovelock)
                    {
                        photonView.RPC(nameof(RpcShareMoveLock), RpcTarget.All, true);//箱の移動を制限
                        firstmovelock = false;
                    }
                   

                    cursorlock = false;//UIカーソル移動を許可
                    GetComponent<PlayerGetHitObjTagManagement>().isMotion = false;//箱の周りの判定をとるのをやめる
                }

            }
            else
            {
                firstchange_boximage = true;
            }
        }

    }




    private void Move()//移動処理（計算部分）
    {
        //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);

       // Debug.Log("移動量"+inputDirection.x);

        if(inputDirection.x == 0)
        {
            //現在アニメーションを再生している時
            if(!firstanimplay)
            {
                photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All);
              //  Debug.Log("steam");
            }
           
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
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            //移動ロックがかかっていなければ移動
            if(!movelock)
            {
                if (firstanimplay)
                {
                    Debug.Log("アニメ送信");
                    photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
                    firstanimplay = false;
                }

                //Debug.Log("スティック動かして移動している");
                //移動方向の入力情報がInputdirectionの中に入るようになる
                inputDirection = context.ReadValue<Vector2>();
            }

        }
    }

    //ジャンプ
    public void Onjump(InputAction.CallbackContext context)
    {
        //アンロックボタン、ロケットが起動中でない時
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock && !isFly) 
        {

            photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All);//ジャンプしている時は移動アニメーションを止める

            //Input Systemからジャンプの入力があった時に呼ばれる
            //連続でジャンプできないようにする
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
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


    [PunRPC]
    //boxopen変数を共有する
    private void RpcShareBoxOpen(bool data)
    {
        boxopen = data;
    }

    [PunRPC]
    //movelock変数を共有する
    private void RpcShareMoveLock(bool data)
    {
        movelock = data;
    }

    [PunRPC]
    private void RpcChangeLiftImage()
    {
        //プレイヤーを持ち上げ時のイラストに変更
        if (gameObject.name == "Player1")
        {
            Debug.Log("QQQP1持ち上げ画像");
            change_unloadimage = false;//通常画像から持ち上げ画像に
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p1LiftImage;

        }
        else if (gameObject.name == "Player2")
        {
            Debug.Log("QQQP2持ち上げ画像");
            change_unloadimage = false;//通常画像から持ち上げ画像に
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p2LiftImage;
        }
    }

    [PunRPC]
    private void RpcChangeBoxOpenImage()
    {
        change_unloadimage = false;//ここでfalseにしないと箱が空くイラストに変わらないので注意
        change_boxopenimage = true;//箱プレイヤーの画像変更
    }


    [PunRPC]
    private void RpcChangeUnloadImage()
    {
        //プレイヤーがブロックを降ろしたときイラスト変更
        if (gameObject.name == "Player1")
        {
           // Debug.Log("P1降ろす画像");
            change_liftimage = false;//持ち上げ画像から元の画像に戻す
            change_unloadimage = true;
        }
        else if (gameObject.name == "Player2")
        {
          //  Debug.Log("P2降ろす画像");
            change_liftimage = false;//持ち上げ画像から元の画像に戻す
            change_unloadimage = true;
        }
    }

    [PunRPC]
    private void RpcMoveLeftandRight()
    {
        //プレイヤーが左右に移動した時その方向に対応してプレイヤーの向きを変える
        if (gameObject.name == "Player1")
        {
            if (left1P)
            {
                Debug.Log("player1の左");
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                firstLR_1P = true;
            }
            else
            {
                 Debug.Log("player1の右");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                firstLR_1P = true;
            }

        }
        else if (gameObject.name == "Player2")
        {
            if (left2P)
            {
                //Debug.Log("player2の左");
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                firstLR_2P = true;
            }
            else
            {
                //Debug.Log("player2の右");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                firstLR_2P = true;
            }
        }
    }

    [PunRPC]
    private void RpcMoveAnimPlay()
    {
       // Debug.Log("アニメ再生");
        animplay = true;
    }

    [PunRPC]
    private void RpcMoveAnimStop()
    {
        //Debug.Log("アニメstop");
        animplay = false;
        firstanimplay = true;
    }
}
