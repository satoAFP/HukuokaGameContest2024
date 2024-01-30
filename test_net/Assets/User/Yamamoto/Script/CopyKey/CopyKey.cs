using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
    //[SerializeField, Header("死亡時のコピーキー")]
    //private Sprite DeathImage;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    [System.NonSerialized] public bool bjump;//連続でジャンプさせないフラグ

    [SerializeField, Header("鍵回収時間（大体60で１秒）")]
    private int collecttime;

    [SerializeField]
    public int holdtime;//設定したアイテム回収時間を代入する

    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    [System.NonSerialized] public bool copykey_death = false;//コピーキーが死亡した時のフラグ
    private bool firstdeathjump = true;//死亡時のノックバックジャンプを一回だけさせる
    private float knockbacktime = 1.0f;//ノックバックするときのＸ座標も移動
    private float timer = 0f;//時間をカウント

    //ブロック持ち上げに使う変数
    [System.NonSerialized] public bool islift = false;//持ち上げフラグ
    [System.NonSerialized] public bool isliftfirst = true;//持ち上げフラグの状態を送信するとき一回しか送信しないため                                                      
    private bool distanceFirst = true;//物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
    private Vector3 dis = Vector3.zero;

    private bool firstDeathEreaHit = true;//一度だけゲームオーバーエリアに当たった処理をする
    private bool fallingdeath = false;//落下死したときの処理
    private bool firstfallingdeath = true;//一度だけ落下死の処理を行う

    private bool firstLR = true;//左右移動一度だけ処理を行う
    private bool left = false;//コピーキーが左に向いているとき
    [System.NonSerialized] public bool imageleft = false;//画像を左向きにするフラグ

    private bool firstChangeLiftImage = true;
    [System.NonSerialized] public bool changeliftimage = false;//コピーキーを持ち上げ画像変更
    [System.NonSerialized] public bool standardCopyKeyImage = false;//標準コピーキー画像

    [System.NonSerialized] public bool animplay = false;//アニメーションを再生
    private bool firstanimplay = true;//複数アニメ起動をさせないフラグ

    private AudioSource audiosource = null;//オーディオソース
    [SerializeField, Header("コピーキー標準SE")] private AudioClip[] StandardSE;
    private bool oneSE = true;//各処理一度だけ歩行SEを鳴らす
    private int walkseframe = 0;//se再生時に測るフレーム
    private bool oneDeathSE = true;//各処理一度だけ死亡SEを鳴らす

    // Start is called before the first frame update
    void Start()
    {
        //名前を設定
        gameObject.name = "CopyKey";

        //全体からコピー鍵取得
        ManagerAccessor.Instance.dataManager.copyKey = gameObject;

        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        ManagerAccessor.Instance.dataManager.isAppearCopyKey = true;

        test_net = new Test_net();//スクリプトを変数に格納

        holdtime = collecttime;//長押しカウント時間を初期化

        audiosource = GetComponent<AudioSource>();//AudioSourceを取得
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        if (ManagerAccessor.Instance.dataManager.isDeth)
        {
            Destroy(gameObject);//プレイヤーが死亡したときコピーキー削除
        }

        //プレイヤーがゲームオーバーになっていなければコピーキーの基本操作許可
        if (!ManagerAccessor.Instance.dataManager.isDeth
            || !copykey_death
            || !ManagerAccessor.Instance.dataManager.isPause)
        {
            //カーソルが鍵を選んでいるとき操作可能
            if ( !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
                &&ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")
            {

                if (firstLR)
                {
                    //コピーキーのの左右の向きを変える
                    if (datamanager.isOwnerInputKey_C_L_LEFT)
                    {
                       
                        left = true;
                        firstLR = false;
                        photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                    }
                    else if (datamanager.isOwnerInputKey_C_L_RIGHT)
                    {
                      
                        left = false;
                        firstLR = false;
                        photonView.RPC(nameof(RpcMoveLeftandRight), RpcTarget.All);
                    }
                }


                //移動アニメーションが再生されているとき効果音を鳴らす
                if (animplay)
                {

                    if (oneSE)
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

                    //コピーキーを選択しているとき十字キー下の吹き出し表示
                    ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(true);
                    ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossDown;

                    //持ち上げていないときは普通に移動させる
                    if (!islift)
                    {
                        Move();//移動処理をON
                        distanceFirst = true;

                        photonView.RPC(nameof(RpcChangeStandardImage), RpcTarget.All, true);

                    }
                    else
                    {
                        photonView.RPC(nameof(RpcChangeLiftImege), RpcTarget.All, true);

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
                }

            }

            //ゲームパッド下ボタンで置きなおし
            if (datamanager.isOwnerInputKey_C_D_DOWN)
            {
                holdtime--;//長押しカウントダウン

                //ゲームパッド下ボタン長押しで回収
                if (holdtime <= 0)//回収カウントが0になると回収
                {
                    Destroy(gameObject);

                    //コピー鍵出現中フラグ
                    ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                    ManagerAccessor.Instance.dataManager.copyKey = null;
                }
            }
            else
            {
                holdtime = collecttime;//長押しカウントリセット
            }
        }
      
          
        if (copykey_death)
        {
            // 画像を切り替えます
            //GetComponent<SpriteRenderer>().sprite = DeathImage;

            if (fallingdeath)
            {
                Debug.Log("コピーキー落下");
                //コピー鍵出現中フラグ
                ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                ManagerAccessor.Instance.dataManager.copyKey = null;

                //コピーキーが消えたことをプレイヤー側に返す
                ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().copykeydelete = true;

                Destroy(gameObject);//念のためにコピーキーを削除
            }
            else
            {
                Debug.Log("コピーキー落下ではない");
                if (oneDeathSE)
                {
                    audiosource.PlayOneShot(StandardSE[2]);//死亡時のSEを鳴らす
                    oneDeathSE = false;
                }

                timer += Time.deltaTime;//時間計測

                //ノックバック処理
                //ここはノックバックしたとき一度跳ねる処理
                if (firstdeathjump)
                {
                    Debug.Log("copykey_deathjump");
                    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                    firstdeathjump = false;
                }

                //ここは1秒ぐらい横に移動する処理
                if (timer <= knockbacktime)
                {
                    Debug.Log("copykey_deathmove");
                    rigid.velocity = new Vector2(0.5f * moveSpeed, rigid.velocity.y);
                }
                else if (timer >= 2.0f)
                {
                    //コピー鍵出現中フラグ
                    ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                    ManagerAccessor.Instance.dataManager.copyKey = null;

                    //コピーキーが消えたことをプレイヤー側に返す
                    ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().copykeydelete = true;

                    //エフェクト生成
                    GameObject clone = Instantiate(ManagerAccessor.Instance.dataManager.StarEffect);
                    clone.transform.position = transform.position;

                    Destroy(gameObject);//念のためにコピーキーを削除
                }
            }

           
        }
       
    }


    private void Move()//移動処理（計算部分）
    {
        if(!copykey_death
            ||!ManagerAccessor.Instance.dataManager.isPause)
        {
            //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
            rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
        }

        if (inputDirection.x == 0)
        {
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
        //プレイヤーが床または着地出来るものに乗っている時、再ジャンプ可能にする
        if (collision.gameObject.tag == "Floor")
        {
            bjump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //プレイヤーが落下した時、ゲームオーバーの処理をする
        if (collision.gameObject.tag == "DeathField")
        {
            if (firstfallingdeath)
            {
                photonView.RPC(nameof(RpcCopyKeyDeath), RpcTarget.All, true);//コピーキー死亡処理

                fallingdeath = true;//落下死用の死亡処理を入れる
                firstfallingdeath = false;
            }
        }

        //落石エリアに入るとコピーキー死亡の処理
        if (collision.gameObject.tag == "DeathErea")
        {
            if(firstDeathEreaHit)
            {
                Debug.Log("コピーキー当たる");
                photonView.RPC(nameof(RpcCopyKeyDeath), RpcTarget.All, true);//コピーキー死亡処理
                firstDeathEreaHit = false;
            }
           
        }
    }

    //playerinputで起動させる関数
    //移動処理
    public void OnMove(InputAction.CallbackContext context)
    {

        if (firstanimplay)
        {
            Debug.Log("アニメ送信");
            photonView.RPC(nameof(RpcMoveAnimPlay), RpcTarget.All);
            firstanimplay = false;
        }

        //移動方向の入力情報がInputdirectionの中に入るようになる
        inputDirection = context.ReadValue<Vector2>();
    }

    //ジャンプ
    public void Onjump(InputAction.CallbackContext context)
    {
        if (!copykey_death)
        {
            //カーソルが鍵を選択している時
            if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
           && ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey"
           && !islift
           && !ManagerAccessor.Instance.dataManager.isPause)
            {
                //1P（箱側）での操作しか受け付けない
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    //アンロックボタンが起動中
                    if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
                    {
                        Debug.Log("コピーキージャンプ");

                        photonView.RPC(nameof(RpcMoveAnimStop), RpcTarget.All);//ジャンプしている時は移動アニメーションを止める

                        //Input Systemからジャンプの入力があった時に呼ばれる
                        if (!context.performed || bjump)
                        {
                            return;
                        }
                        else
                        {
                            photonView.RPC(nameof(RpcPlayJumpSE), RpcTarget.All);//ジャンプの効果音
                            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                            bjump = true;//一度ジャンプしたら着地するまでジャンプできなくする
                        }


                    }
                }
            }

            ////Input Systemからジャンプの入力があった時に呼ばれる
            //if (!context.performed || bjump)
            //{
            //    return;
            //}
            //else
            //{
            //    rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            //    bjump = true;//一度ジャンプしたら着地するまでジャンプできなくする
            //}

        }
    }


    [PunRPC]
    private void RpcCopyKeyDeath(bool data)
    {
        copykey_death = data;//copykey_death変数を共有する
    }

    [PunRPC]
    private void RpcChangeLiftImege(bool data)
    {
        standardCopyKeyImage = false;
        changeliftimage = data;//changeliftimageを共有する
        //firstChangeLiftImage = true;
    }

    [PunRPC]
    private void RpcChangeStandardImage(bool data)
    {
        changeliftimage = false;
        standardCopyKeyImage = data;//standardCopyKeyImageを共有する
    }

    [PunRPC]
    private void RpcMoveLeftandRight()
    {
        if (left)
        {
          
            imageleft = true;
            firstLR = true;
        }
        else
        {
           
            imageleft = false;
            firstLR = true;
        }
    }

    [PunRPC]
    private void RpcMoveAnimPlay()
    {
         Debug.Log("コピーキーアニメ再生");
        animplay = true;
    }

    [PunRPC]
    private void RpcMoveAnimStop()
    {
        Debug.Log("コピーキーアニメstop");
        animplay = false;
        firstanimplay = true;
    }

    [PunRPC]
    private void RpcPlayJumpSE()
    {
        audiosource.PlayOneShot(StandardSE[1]);//ジャンプ効果音
    }

}


