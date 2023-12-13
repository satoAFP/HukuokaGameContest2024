using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerate : MonoBehaviour
{
    [SerializeField] private GameObject GenelateErea_L;//生成場所左
    [SerializeField] private GameObject GenelateErea_R;//生成場所右


    [SerializeField] private GameObject[] Stones = new GameObject[3];//大きい岩

    [SerializeField] private GameObject StoneParentObject;//生成した岩の親オブジェクト

    private int stonetype = 0;//生成する岩の種類をランダムに決める

    private float height;

    [SerializeField, Header("何秒間ずつに生成するか設定できる")] private float spawnInterval; // 生成間隔

    [SerializeField, Header("spawnIntervalで設定した秒ごとに何個生成するか設定できる")] private int spawnstone;      // 一回に生成する岩の数

    [SerializeField, Header("移動速度")] private float moveSpeed;

    private bool movestop = false;//DeathEreaの移動を止める

    private float timer = 0f;//時間をカウント

    private Vector2 spawnPosition;//岩生成場所

    private GameObject ParentObj;//岩の親オブジェクトになるもの

    private GameObject ChildObj;//生成した岩オブジェクトを子オブジェクトにする

    // Start is called before the first frame update
    void Start()
    {
        height = gameObject.GetComponent<Renderer>().bounds.size.y;

        ParentObj = Instantiate(StoneParentObject, Vector2.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // オブジェクトを右に移動させる
        if(!movestop)
        {
            Debug.Log("移動中");
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        

        timer += Time.deltaTime;

        //ここで岩を生成する時間を計測
        if (timer >= spawnInterval)
        {
            for(int i=0;i < spawnstone; i++)
            {
                SpawnObject();
            }
           
            timer = 0f;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ストップエリアに入ると移動を止める
        if (collision.gameObject.tag == "StopErea")
        {
            //Debug.Log("エリアストップ");
            movestop = true;

        }
    }


    public void SpawnObject()
    {

        stonetype = Random.Range(1, 3);//岩の種類をランダム選出

        float randomX = Random.Range(GenelateErea_L.transform.localPosition.x, GenelateErea_R.transform.localPosition.x);//ランダムなX座標

        spawnPosition = new Vector2(randomX, height);//生成位置を設定

        var worldPoint = transform.TransformPoint(spawnPosition);//ワールド座標に変換

        worldPoint.y = height;//高さをワールド座標にするとずれるのでローカルに戻す



        ChildObj = Instantiate(Stones[stonetype], worldPoint, Quaternion.identity);

        ChildObj.transform.parent = ParentObj.transform; // 生成した岩をParentObjの子オブジェクトにする
    }




}
