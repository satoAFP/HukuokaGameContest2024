using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StoneGenerate : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject[] Stones = new GameObject[3];//大きい岩

    [SerializeField] private GameObject StoneParentObject;//生成した岩の親オブジェクト

    [SerializeField, Header("危険表示")] private GameObject DangerMark;

    [SerializeField, Header("カメラ")] private Transform cameraGenelate;

    [SerializeField, Header("落石の前後どの座標に生成するか")] private float genelatePosX;
    [SerializeField, Header("プレイヤーの上下どの座標に生成するか")] private float genelatePosY;

    [SerializeField, Header("表示する距離")] private float DisplayDir;//表示するときの距離

    private int stonetype = 0;//生成する岩の種類をランダムに決める

    private float height;

    [SerializeField, Header("何秒間ずつに生成するか設定できる")] private float spawnInterval; // 生成間隔

    [SerializeField, Header("spawnIntervalで設定した秒ごとに何個生成するか設定できる")] private int spawnstone;      // 一回に生成する岩の数

    [SerializeField, Header("移動速度")] private float moveSpeed;

    private float firstspeed = 0;//最初に設定したmoveSpeedを保存する

    private bool movestop = false;//DeathEreaの移動を止める

    private float timer = 0f;//時間をカウント

    private Vector2 spawnPosition;//岩生成場所

    private GameObject ParentObj;//岩の親オブジェクトになるもの

    private GameObject ChildObj;//生成した岩オブジェクトを子オブジェクトにする

    private Rigidbody2D rb; // Rigidbody2Dを保持する変数

    // Start is called before the first frame update
    void Start()
    {
        height = transform.position.y + transform.localScale.y / 2;

        ParentObj = Instantiate(StoneParentObject, Vector2.zero, Quaternion.identity);

        firstspeed = moveSpeed;

        // ゲームオブジェクトにアタッチされたRigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //危険表示非表示
        if (PhotonNetwork.IsMasterClient)
        {
           
            if (ManagerAccessor.Instance.dataManager.player1.transform.position.x - transform.position.x < DisplayDir)
                DangerMark.SetActive(true);
            else
                DangerMark.SetActive(false);
        }
        else
        {
            if (ManagerAccessor.Instance.dataManager.player2.transform.position.x - transform.position.x < DisplayDir)
                DangerMark.SetActive(true);
            else
                DangerMark.SetActive(false);
        }

        //危険表示の座標変更
        DangerMark.transform.position = new Vector3(transform.position.x + genelatePosX, cameraGenelate.position.y + genelatePosY);

        // オブジェクトを右に移動させる
        if (!movestop && !ManagerAccessor.Instance.dataManager.isPause)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        if (ManagerAccessor.Instance.dataManager.isPause ||
            ManagerAccessor.Instance.dataManager.isClear ||
            ManagerAccessor.Instance.dataManager.isDeth)
        {
            moveSpeed = 0;//デスエリアに掛かっている速度を0にする
            rb.constraints = RigidbodyConstraints2D.FreezeAll;//横方向の移動を止める
        }
        else//ポーズ中でなければ岩を生成し続ける
        {
            //ポーズした時に止めた移動処理を解除する
            moveSpeed = firstspeed;

            rb.constraints =
RigidbodyConstraints2D.FreezeRotation |
RigidbodyConstraints2D.FreezePositionY;

            timer += Time.deltaTime;

            //ここで岩を生成する時間を計測
            if (timer >= spawnInterval)
            {
                for (int i = 0; i < spawnstone; i++)
                {
                    SpawnObject();
                }

                timer = 0f;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ストップエリアに入ると移動を止める
        if (collision.gameObject.tag == "StopErea")
        {
            movestop = true;
        }
    }


    public void SpawnObject()
    {

        stonetype = Random.Range(1, 3);//岩の種類をランダム選出

        float randomX = Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2);//ランダムなX座標


        spawnPosition = new Vector2(randomX, height);//生成位置を設定

        ChildObj = Instantiate(Stones[stonetype]);

        ChildObj.transform.parent = ParentObj.transform; // 生成した岩をParentObjの子オブジェクトにする
        ChildObj.transform.position = spawnPosition;
    }




}
