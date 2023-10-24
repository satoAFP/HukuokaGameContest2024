using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{

    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    //移動方向入れる変数
    private Rigidbody2D rigid;

    private BoardInput boardinput;//inputsystemをスクリプトで呼び出す

    // Start is called before the first frame update
    void Start()
    {
        //PlayerのRigidbody2Dコンポーネントを取得する
        rigid = GetComponent<Rigidbody2D>();

        boardinput = new BoardInput();//スクリプトを変数に格納

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
