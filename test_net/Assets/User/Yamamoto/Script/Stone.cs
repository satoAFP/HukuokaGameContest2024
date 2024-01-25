using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private Rigidbody2D rb; // Rigidbody2Dを保持する変数

    private float speed = 0;//落石の速度

    private float acc = 0;//加速度

    [SerializeField, Header("ランダムに決める速度の最低値")]
    private float randspeed_low = 0;
    [SerializeField, Header("ランダムに決める速度の最大値")]
    private float randspeed_max = 0;
    [SerializeField, Header("ランダムに決める加速度の最低値")]
    private float randacc_low = 0;
    [SerializeField, Header("ランダムに決める加速度の最大値")]
    private float randacc_max = 0;

    // Start is called before the first frame update
    void Start()
    {
        //ここで落石の速度と加速度を一定の値からランダムで選ぶ
        acc 　= Random.Range(randacc_low, randacc_max);
        speed = Random.Range(randspeed_low, randspeed_max);

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
            acc += 0.05f;//加速度+
            rb.constraints = RigidbodyConstraints2D.None;//FreezePositionを解除する

            transform.Translate(Vector3.down * (speed + acc) * Time.deltaTime);//落石の移動処理
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
