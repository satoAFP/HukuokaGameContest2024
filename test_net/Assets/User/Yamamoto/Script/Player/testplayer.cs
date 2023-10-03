using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class testplayer : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("移動速度")]
    private float moveSpeed;

    
    void Start()
    {
        //名前とIDを設定
        gameObject.name = "Player" + photonView.OwnerActorNr;
    }


    void Update()
    {
        //操作が競合しないための設定
        if (photonView.IsMine)
        {
            // 入力をxに代入
            float x = Input.GetAxis("Horizontal");

            //Rigidbody2Dを取得
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            //x軸に加わる力を格納
            Vector2 force = new Vector2(x * 10, 0);

            //Rigidbody2Dに力を加える
            rb.AddForce(force);
        }
           
    }
}
