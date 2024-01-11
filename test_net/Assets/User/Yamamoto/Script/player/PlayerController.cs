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

    private DataManager datamanager;

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

    [System.NonSerialized] public bool imageleft = false;//画像を左向きにするフラグ
    
    [System.NonSerialized] public bool animplay = false;//アニメーションを再生

    private bool firstanimplay = true;//複数アニメ起動をさせないフラグ

    private bool firststickprocess = true;//スティックを動かしたときの移動処理を一回ずつ行う

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


    private bool firstdeathjump = true;//死亡時のノックバックジャンプを一回だけさせる
    private float knockbacktime = 1.0f;//ノックバックするときのＸ座標も移動
    private float timer = 0f;//時間をカウント
    private bool firstdeathknockback = true;
    private bool knockbackmove = true;

    [System.NonSerialized] public bool knockback_finish = false;//ノックバック終了

    [System.NonSerialized] public bool copykeydelete = false;//コピーキーが岩に当たって消えたとき


    private AudioSource audiosource = null;//オーディオソース
    [SerializeField, Header("プレイヤー標準SE")]   private AudioClip[] StandardSE;
    [SerializeField, Header("箱プレイヤー専用SE")] private AudioClip[] BoxplayerSE;
    private bool oneSE = true;
    private int walkseframe = 0;//se再生時に測るフレーム
    private bool oneboxopenSE = true;//一度だけ箱をあけるSE
    
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

        audiosource = GetComponent<AudioSource>();//AudioSourceを取得

    }
    void FixedUpdate()
    {
        //データマネージャー取得
        datamanager = ManagerAccessor.Instance.dataManager;


        //死亡時とポーズ時に全ての処理を止める
        if (datamanager.isDeth || ManagerAccessor.Instance.dataManager.isPause)
        {
            //死亡時
            if(datamanager.isDeth)
            {
                timer += Time.deltaTime;

                //1秒ぐらい経過したら再度RpcDeathKnockBackを呼び出す
                if (knockbacktime <= timer)
                {
                    knockbackmove = false;
                    firstdeathknockback = true;
                }

                if (firstdeathknockback)
                {
                    Debug.Log("Rpc返す");

                    photonView.RPC(nameof(RpcDeathKnockBack), RpcTarget.All);//死亡時のノックバック

                    firstdeathknockback = false;
                }

                if (!knockbackmove)
                {
                    rigid.constraints = RigidbodyConstraints2D.FreezePositionX|
                                        RigidbodyConstraints2D.FreezeRotation;//FreezePositionXをオンにする
                    knockback_finish = true;
                }
            }

            //ポーズ時
            if(ManagerAccessor.Instance.dataManager.isPause)
            {
                inputDirection.x = 0;
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX|
                                    RigidbodyConstraints2D.FreezeRotation;//移動量を0にする
            }
           

        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;//回転を止める

            if (firstLR_1P)
            {
                //プレイヤーの左右の向きを変える
                if (datamanager.isOwnerInputKey_C_L_LEFT && !movelock)
                {
                    //Debug.Log("左いいいい");
                    left1P = true;
                    firstLR_1P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }
                else if (datamanager.isOwnerInputKey_C_L_RIGHT && !movelock)
                {
                    //Debug.Log("右いいいい");
                    left1P = false;
                    firstLR_1P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }

            }

            if (firstLR_2P)
            {
                if (datamanager.isClientInputKey_C_L_LEFT)
                {
                    // Debug.Log("2P左いいいい");
                    left2P = true;
                    firstLR_2P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }
                else if (datamanager.isClientInputKey_C_L_RIGHT)
                {
                    // Debug.Log("2P右いいいい");
                    left2P = false;
                    firstLR_2P = false;
                    photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                }
            }

            //移動アニメーションが再生されているとき効果音を鳴らす
            if(animplay)
            {

                if(oneSE)
                {
                    audiosource.PlayOneShot(StandardSE[0]);//歩く効果音
                    oneSE = false;
                }
                else
                {
                    walkseframe++;//大体の効果音の再生時間を計測する

                    //効果音が鳴りやんだタイミングで再度効果音を鳴らす
                    if (walkseframe >= 30)
                    {
                        oneSE = true;
                        walkseframe = 0;//ここでフレーム計算をリセット
                    }
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
                    if ((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
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

                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //プレイヤー1（箱）の移動が制限されているとき（箱が空いている時）
                    if (movelock)
                    {
                        //生成アイテムがマップ上にないときのみ箱を閉じる（移動制限解除）
                        if (currentBoardObject == null &&
                             currentCopyKeyObject == null)
                        {
                            if (firstboxopen)
                            {
                                //boxopen関数を共有する
                                // Debug.Log("firstboxopenが入ってる");
                                photonView.RPC(nameof(RpcShareBoxOpen), RpcTarget.All, true);
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

                        //コントローラーの下ボタンを押したとき箱を閉じる・またはコピーキーが死んだとき板を出していなければ閉じる
                        if (datamanager.isOwnerInputKey_C_D_DOWN
                             && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart
                             || copykeydelete)
                        {
                            //箱を閉じて移動ロックを解除
                            if (gameObject.name == "Player1" && boxopen)
                            {
                                change_boxopenimage = false;//箱を閉じた画像にする
                                cursorlock = true;//カーソル移動を止める

                                //当たり判定を戻す
                                GetComponent<BoxCollider2D>().isTrigger = false;
                                GetComponent<Rigidbody2D>().simulated = true;

                                if (!firstmovelock)
                                {
                                    photonView.RPC(nameof(RpcShareMoveLock), RpcTarget.All, false);//箱の移動の制限解除
                                    firstmovelock = true;
                                }
                                //movelock = false;
                                GetComponent<PlayerGetHitObjTagManagement>().isMotion = true;//箱の周りの判定をとるのを再開
                                copykeydelete = false;//コピーキー削除済みフラグリセット
                                firstboxopen = true;//boxopenフラグ共有再会
                            }
                        }

                        //ゲームパッド右ボタンでアイテム生成
                        //板
                        if (datamanager.isOwnerInputKey_CB &&
                            choicecursor == "Board")
                        {
                            if (B_instantiatefirst)
                            {
                                if (currentBoardObject == null)
                                {
                                    currentBoardObject = PhotonNetwork.Instantiate("Board", new Vector2(p1pos.x, p1pos.y + 1.0f), Quaternion.identity);
                                    // movelock = true;
                                    //Debug.Log("板だす");

                                    //先に鍵が生成されていた場合
                                    if (currentCopyKeyObject != null)
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

                                    copykeydelete = false;//鍵削除フラグをfalse

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
            }



            //各プレイヤーの現在座標を取得
            if (ManagerAccessor.Instance.dataManager.player1 != null)
                p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;

            if (ManagerAccessor.Instance.dataManager.player2 != null)
                p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;

            //箱と鍵の二点間距離を取って一定の値なら箱オープン可能
            if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
            {
                //上ボタンの同時押しで箱オープン
                if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP
                    && !ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !ManagerAccessor.Instance.dataManager.isNotOpenBox) 
                {
                    //Debug.Log("上キー両押し");
                    //宝箱のプレイヤーの時、空いている箱のイラストに変更
                    if (gameObject.name == "Player1")
                    {
                        if (firstchange_boximage)
                        {
                            Debug.Log("ソルジャー1st");
                            audiosource.PlayOneShot(BoxplayerSE[0]);//箱を開ける効果音
                            photonView.RPC(nameof(RpcChangeBoxOpenImage), RpcTarget.All);//箱を空けるイラスト変更フラグを送信
                            firstchange_boximage = false;
                        }

                        //movelock = true;//箱の移動を制限
                        if (firstmovelock)
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


        
    }




    private void Move()//移動処理（計算部分）
    {

        //ゲームオーバーまたはクリア処理を返すまで移動の計算をする
        if(!ManagerAccessor.Instance.dataManager.isDeth 
            || !ManagerAccessor.Instance.dataManager.isClear
            || !ManagerAccessor.Instance.dataManager.isPause)
        {
            rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        }


        ////一定の移動量が無いと進まないようにする
        //if (Mathf.Abs(inputDirection.x) > 0.08f
        //    && firststickprocess)
        //{
        //    Debug.Log("あの丘");
        //    //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
            
        //    firststickprocess = false;//スティックが再入力されなければ処理を行わない
        //}


        if (inputDirection.x == 0)
        {
           // firststickprocess = true;
            //現在アニメーションを再生している時
            if (!firstanimplay)
            {
                photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All); 
              //  Debug.Log("steam");
            }
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //プレイヤーが床または着地出来るものに乗っている時、再ジャンプ可能にする
        if (collision.gameObject.tag == "Floor")
        {
            bjump = false;
        }

        //プレイヤーが落下した時、ゲームオーバーの処理をする
        if (collision.gameObject.tag == "DeathField")
        {
            //Debug.Log("si");
            datamanager.isDeth = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //落石エリアに入るとゲームオーバーのシーン
        if (collision.gameObject.tag == "DeathErea")
        {
             

            datamanager.DeathPlayerName = this.gameObject.name;

            Debug.Log("死んだ奴の名前"+ datamanager.DeathPlayerName);

            datamanager.isDeth = true;
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
                   // Debug.Log("アニメ送信");
                    photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
                    firstanimplay = false;
                }

                //Debug.Log("スティック動かして移動している");
                //移動方向の入力情報がInputdirectionの中に入るようになる
                if (islift &&
                    !((datamanager.isOwnerInputKey_C_L_RIGHT && datamanager.isClientInputKey_C_L_RIGHT) ||
                    (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT)))
                {

                    inputDirection.x = 0;
                }
                else
                    inputDirection = context.ReadValue<Vector2>();
            }

        }
    }

    //ジャンプ
    public void Onjump(InputAction.CallbackContext context)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //アンロックボタン、ロケットが起動中でない時 死亡してない時
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock && !isFly
          &&!islift  && !ManagerAccessor.Instance.dataManager.isDeth
          && !ManagerAccessor.Instance.dataManager.isClear
          && !ManagerAccessor.Instance.dataManager.isPause) 
        {
            Debug.Log("ジャンプできる");

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
                photonView.RPC(nameof(RpcPlayJumpSE), RpcTarget.All);//ジャンプの効果音
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
            //Debug.Log("QQQP1持ち上げ画像");
            change_unloadimage = false;//通常画像から持ち上げ画像に
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p1LiftImage;

        }
        else if (gameObject.name == "Player2")
        {
           // Debug.Log("QQQP2持ち上げ画像");
            change_unloadimage = false;//通常画像から持ち上げ画像に
            change_liftimage = true;
            //GetComponent<SpriteRenderer>().sprite = p2LiftImage;
        }
    }

    [PunRPC]
    private void RpcChangeBoxOpenImage()
    {

        Debug.Log("ake");
        //当たり判定を切る
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().simulated = false;

       

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
               // Debug.Log("player1の左");
                imageleft = true;
                firstLR_1P = true;
            }
            else
            {
                 //Debug.Log("player1の右");
                imageleft = false;
                firstLR_1P = true;
            }

        }
        else if (gameObject.name == "Player2")
        {
            if (left2P)
            {
                //Debug.Log("player2の左");
                imageleft = true;
                firstLR_2P = true;
            }
            else
            {
                //Debug.Log("player2の右");
                imageleft = false;
                firstLR_2P = true;
            }
        }
    }

    [PunRPC]
    private void RpcDeathKnockBack()
    {
        Debug.Log("ノックバック処理はいる");

        if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player1")
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //ノックバック処理
                //ここはノックバックしたとき一度跳ねる処理
                if (firstdeathjump)
                {
                    //Debug.Log("Takeru");
                    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    firstdeathjump = false;
                }

                //ここは1秒ぐらい横に移動する処理
                if (knockbackmove)
                {
                    rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
                }
                //else
                //{
                //    rigid.constraints = RigidbodyConstraints2D.FreezePositionX;//FreezePositionXをオンにする
                //    knockback_finish = true;
                //}
            }
        }
        else if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player2")
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //ノックバック処理
                //ここはノックバックしたとき一度跳ねる処理
                if (firstdeathjump)
                {
                    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    firstdeathjump = false;
                }

                //ここは1秒ぐらい横に移動する処理
                if (knockbackmove)
                {
                    rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
                }
                //else
                //{
                //    rigid.constraints = RigidbodyConstraints2D.FreezePositionX;//FreezePositionXをオンにする
                //    knockback_finish = true;
                //}
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

    [PunRPC]
    private void RpcPlayJumpSE()
    {
        audiosource.PlayOneShot(StandardSE[1]);//ジャンプ効果音
    }


}
