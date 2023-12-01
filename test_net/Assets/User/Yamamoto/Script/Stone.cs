using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2Dを保持する変数

    private float gravity = 0;//重力をランダムに設定

    // Start is called before the first frame update
    void Start()
    {
        gravity = Random.Range(0.1f, 1.0f);

        // ゲームオブジェクトにアタッチされたRigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();

        //重力スケールを変更する
        rb.gravityScale = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        //岩が画面外に行くと削除
        if (collision.gameObject.tag == "DeathField")
        {
            //Debug.Log("いわーい");
            Destroy(this.gameObject);
        }
    }
}
