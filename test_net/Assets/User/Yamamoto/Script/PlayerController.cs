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

    private bool movelock = false;//移動処理を停止させる

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //各プレイヤーの座標
    private Vector2 p1pos;
    private Vector2 p2pos;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    private bool bjump;//連続でジャンプさせないフラグ

    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    [System.NonSerialized]public bool islift = false;//持ち上げフラグ

    [System.NonSerialized] public bool isliftfirst = true;//持ち上げフラグの状態を送信するとき一回しか送信しないため

    //物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
    private bool distanceFirst = true;
    private Vector3 dis = Vector3.zero;

    void Start()
    {
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
    void Update()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            //1Pの画面の2Pの情報更新
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                if (ManagerAccessor.Instance.dataManager.player2 != null)
                    ManagerAccessor.Instance.dataManager.player2.GetComponent<PlayerController>().islift = islift;

           
            //持ち上げていないときは普通に移動させる
            if (!islift)
            {
                Move();//移動処理をON

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
        p1pos = ManagerAccessor.Instance.dataManager.player1.transform.position;
        //Debug.Log("p1現在地=" + p1pos);
        if (ManagerAccessor.Instance.dataManager.player2 != null)
            p2pos = ManagerAccessor.Instance.dataManager.player2.transform.position;
        //Debug.Log("p2現在地=" + p2pos);

        // Debug.Log(Mathf.Abs(p1pos.x - p2pos.x));


        //箱と鍵の二点間距離を取って一定の値なら箱オープン可能
        if (Mathf.Abs(p1pos.x - p2pos.x) < 1.0f)
        {
            Debug.Log("密着！！隣の晩御飯！！");
            //上ボタンの同時押しで箱オープン
            if (datamanager.isOwnerInputKey_C_D_UP && datamanager.isClientInputKey_C_D_UP)
            {
                Debug.Log("上キー両押し");
                //宝箱のプレイヤーの時、空いている箱のイラストに変更
                if (gameObject.name == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1OpenImage;
                    movelock = true;//箱の移動を制限
                }

            }
            //else
            //{
            //    //同時に上ボタンを押していないときは画像を元に戻す
            //    if (gameObject.name == "Player1")
            //    {
            //        GetComponent<SpriteRenderer>().sprite = p1Image;
            //    }
            //}
        }

    }




    private void Move()//移動処理（計算部分）
    {
        //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
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
        //アンロックボタンが起動中
        if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart && !movelock)
        {
            //Input Systemからジャンプの入力があった時に呼ばれる
            if (!context.performed || bjump)
            {
                return;
            }

            //操作が競合しないための設定
            if (photonView.IsMine)
            {
                rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                bjump = true;//一度ジャンプしたら着地するまでジャンプできなくする
            }
        }
    }

    //箱の蓋を閉める
    public void OnBoxClose(InputAction.CallbackContext context)
    {
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            if(movelock)
            {
                Debug.Log("アクション");
                //同時に上ボタンを押していないときは画像を元に戻す
                if (gameObject.name == "Player1")
                {
                    GetComponent<SpriteRenderer>().sprite = p1Image;
                    movelock = false;//移動可能にする
                }      
            }
            
        }
    }

    //箱オープン
    public void OnOpenAction(InputAction.CallbackContext context)
    {
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            Debug.Log("箱開ける");
        }
    }

}
