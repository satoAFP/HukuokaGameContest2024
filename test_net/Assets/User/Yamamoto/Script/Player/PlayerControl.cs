using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControl : MonoBehaviour
{
    private InputAction inputaction;//InputSystemを使うために必要なスクリプト

    [SerializeField,Header("プレイヤーのスピード")]
    private float speed = 0;

    private Rigidbody2D rb;//プレイヤーのリジットボディ

    //入力の上下移動の判別をする
    private float movementX;
    private float movementY;


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーにアタッチされているRigidbodyを取得
        rb = gameObject.GetComponent<Rigidbody2D>();

        inputaction = new InputAction();
        inputaction.Enable();//インスタンス化したInputSystemが利用可能に
    }

    /// <summary>
    /// 移動操作（上下左右キーなど）を取得
    /// </summary>
    /// <param name="movementValue"></param>
    private void OnMove(InputValue movementValue)
    {
        // Moveアクションの入力値を取得
        Vector2 movementVector = movementValue.Get<Vector2>();

        // x,y軸方向の入力値を変数に代入
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // 入力値を元に3軸ベクトルを作成
        Vector3 movement = new Vector3(movementX / 5, 0.0f, movementY / 5);

        // rigidbodyのAddForceを使用してプレイヤーを動かす。
       // rb.AddForce(movement * speed);

        transform.position += movement;

    }
}
