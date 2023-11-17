using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CopyKey : MonoBehaviourPunCallbacks
{
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


    //ブロック持ち上げに使う変数
    [System.NonSerialized] public bool islift = false;//持ち上げフラグ
    [System.NonSerialized] public bool isliftfirst = true;//持ち上げフラグの状態を送信するとき一回しか送信しないため                                                      
    private bool distanceFirst = true;//物を持ち上げて移動するとき、最初にプレイヤー同士の差を求める
    private Vector3 dis = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //名前を設定
        gameObject.name = "CopyKey";

        //全体からコピー鍵取得
        ManagerAccessor.Instance.dataManager.copyKey = gameObject;

        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        

        test_net = new Test_net();//スクリプトを変数に格納

        holdtime = collecttime;//長押しカウント時間を初期化
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //カーソルが鍵を選んでいるとき操作可能
        if (!ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().keymovelock
            && ManagerAccessor.Instance.dataManager.player1.GetComponent<PlayerController>().choicecursor == "CopyKey")
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
                            dis = datamanager.player1.transform.position - datamanager.player2.transform.position;
                            distanceFirst = false;
                        }

                        //2Pが1Pに追従するようにする
                        transform.position = datamanager.player1.transform.position - dis;
                    }
                }
            }

            //ゲームパッド下ボタンで置きなおし
            if (datamanager.isOwnerInputKey_CA)
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
        //1P（箱側）での操作しか受け付けない
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
        //    Debug.Log("コピーキー移動");
        //}


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
}


