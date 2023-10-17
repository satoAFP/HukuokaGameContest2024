using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("宝箱")]
    private Sprite p1Image;

    [SerializeField, Header("鍵")]
    private Sprite p2Image;

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    private bool bjump;//連続でジャンプさせないフラグ

    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    [System.NonSerialized]public bool islift = false;//持ち上げフラグ

    void Start()
    {
        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        //名前とIDを設定
        gameObject.name = "Player" + photonView.OwnerActorNr;

        if (gameObject.name == "Player1")
            GetComponent<SpriteRenderer>().sprite = p1Image;
        if (gameObject.name == "Player2")
            GetComponent<SpriteRenderer>().sprite = p2Image;

        test_net = new Test_net();//スクリプトを変数に格納
        //test_net.Enable();

        // デバイス一覧を取得
        foreach (var device in InputSystem.devices)
        {
            // デバイス名をログ出力
            Debug.Log(device.name);
        }

    }
    void Update()
    {

        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            //持ち上げていないときは普通に移動させる
            if(!islift)
            {
                Move();//移動処理をON
                Debug.Log("デフォルト");
            }
            else
            {
                //持ち上げている時は2プレイヤーが同じ移動方向を入力時移動
                if ((datamanager.isOwnerInputKey_C_L_RIGHT&& datamanager.isClientInputKey_C_L_RIGHT)||
                   (datamanager.isOwnerInputKey_C_L_LEFT && datamanager.isClientInputKey_C_L_LEFT))
                {
                    Move();
                    Debug.Log("特殊");
                }
            }
        }
    }

    private void Move()//移動処理（計算部分）
    {
        //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
        rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameObject.name == "Player2")
        {
            Debug.Log("プレイヤー2認識");
        }


        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            Debug.Log("スティック動かして移動している");
            //移動方向の入力情報がInputdirectionの中に入るようになる
            inputDirection = context.ReadValue<Vector2>();
        }
        else
        {
            Debug.Log("識別できてない");
        }
    }

    public void Onjump(InputAction.CallbackContext context)
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

    public void OnAction(InputAction.CallbackContext context)
    {
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            Debug.Log("アクション");
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
}
