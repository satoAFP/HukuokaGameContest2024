using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    [SerializeField, Header("ジャンプ速度")]
    private float jumpSpeed;

    //入力された方向を入れる変数
    private Vector2 inputDirection;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    private Test_net test_net;//inputsystemをスクリプトで呼び出す

    void Start()
    {
        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        //名前とIDを設定
        gameObject.name = "Player" + photonView.OwnerActorNr;

        test_net = new Test_net();//スクリプトを変数に格納
        test_net.Enable();

        // デバイス一覧を取得
        foreach (var device in InputSystem.devices)
        {
            // デバイス名をログ出力
            Debug.Log(device.name);
        }

    }
    void Update()
    {
        //Move();//移動処理をON
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
          


        }

        //移動に対応したキーorスティックが押されたとき
        if (test_net.Player.Move.triggered)
        {
            //プレイヤーが入力した方向に横方向限定で移動速度分の力を加える
            rigid.velocity = new Vector2(inputDirection.x * moveSpeed, rigid.velocity.y);
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
        if (!context.performed)
        {
            return;
        }

        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
