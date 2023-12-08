using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("死亡時のコピーキー")]
    private Sprite DeathImage;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    private bool bjump;//連続でジャンプさせないフラグ

    [SerializeField, Header("鍵回収時間（大体60で１秒）")]
    private int collecttime;

    [SerializeField]
    private int holdtime;//設定したアイテム回収時間を代入する

    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    private bool copykey_death = false;//コピーキーが死亡した時のフラグ
    private bool firstdeathjump = true;//死亡時のノックバックジャンプを一回だけさせる
    private float knockbacktime = 1.0f;//ノックバックするときのＸ座標も移動
    private float timer = 0f;//時間をカウント

    //ブロック持ち上げに使う変数
    [System.NonSerialized] public bool islift = false;//持ち上げフラグ
    [System.NonSerialized] public bool isliftfirst = true;//持ち上げフラグの状態を送信するとき一回しか送信しないため                                                      
    private bool distanceFirst = true;//物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
    private Vector3 dis = Vector3.zero;

    private bool firstDeathEreaHit = true;//一度だけゲームオーバーエリアに当たった処理をする

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //プレイヤーがゲームオーバーになっていなければコピーキーの基本操作許可
        if (!ManagerAccessor.Instance.dataManager.isDeth || !copykey_death)
        {
            //カーソルが鍵を選んでいるとき操作可能
            if ( !ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
                &&ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")
            {
                //操作が競合しないための設定
                if (photonView.IsMine)
                {
                    //持ち上げていないときは普通に移動させる
                    if (!islift)
                    {
                        Move();//移動処理をON
                        distanceFirst = true;
                    }
                    else
                    {
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
            GetComponent<SpriteRenderer>().sprite = DeathImage;

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
            else if (timer >= 2.5f)
            {
                Destroy(gameObject);//念のためにコピーキーを削除

                //コピー鍵出現中フラグ
                ManagerAccessor.Instance.dataManager.isAppearCopyKey = false;
                ManagerAccessor.Instance.dataManager.copyKey = null;
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //落石エリアに入るとコピーキー死亡の処理
        if (collision.gameObject.tag == "DeathErea")
        {
            if(firstDeathEreaHit)
            {
                Debug.Log("コピーキー当たる");
                photonView.RPC(nameof(RpcCopyKeyDeath), RpcTarget.All, true);
                firstDeathEreaHit = false;
            }
           
        }
    }

    //playerinputで起動させる関数
    //移動処理
    public void OnMove(InputAction.CallbackContext context)
    {
        //移動方向の入力情報がInputdirectionの中に入るようになる
        inputDirection = context.ReadValue<Vector2>();
    }

    //ジャンプ
    public void Onjump(InputAction.CallbackContext context)
    {

        if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
            && ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")//カーソルが鍵を選択している時
        {
            //1P（箱側）での操作しか受け付けない
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //アンロックボタンが起動中
                if (!ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
                {
                    Debug.Log("コピーキージャンプ");
                    //Input Systemからジャンプの入力があった時に呼ばれる
                    if (!context.performed || bjump)
                    {
                        return;
                    }
                    else
                    {
                        rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                        bjump = true;//一度ジャンプしたら着地するまでジャンプできなくする
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
}


