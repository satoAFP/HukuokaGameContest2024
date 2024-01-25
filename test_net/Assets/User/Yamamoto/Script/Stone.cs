using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2Dを保持する変数

    private float speed = 0;//落石の速度

    private float acc = 0;//加速度

    // Start is called before the first frame update
    void Start()
    {
        //ここで落石の速度と加速度を一定の値からランダムで選ぶ
        acc = Random.Range(0.2f, 1.0f);
        speed = Random.Range(2.0f, 5.0f);

        // ゲームオブジェクトにアタッチされたRigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //ポーズ中の時岩の落下を止める
        if (ManagerAccessor.Instance.dataManager.isPause)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;//FreezePositionYをオンにする
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;//FreezePositionを解除する

            transform.Translate(Vector3.down * (speed + acc * Time.time) * Time.deltaTime);//落石の移動処理
        }
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
