using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerate : MonoBehaviour
{
    [SerializeField] private GameObject GenelateErea_L;//生成場所左
    [SerializeField] private GameObject GenelateErea_R;//生成場所右


    [SerializeField] private GameObject[] Stones = new GameObject[3];//大きい岩

    private int stonetype = 0;//生成する岩の種類をランダムに決める

    private float height;

    [SerializeField, Header("何秒間ずつに生成するか設定できる")] private float spawnInterval; // 生成間隔

    [SerializeField, Header("spawnIntervalで設定した秒ごとに何個生成するか設定できる")] private int spawnstone;      // 一回に生成する岩の数

    private float timer = 0f;//時間をカウント

    private Vector2 spawnPosition;//岩生成場所

    // Start is called before the first frame update
    void Start()
    {
        height = gameObject.GetComponent<Renderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

    public void SpawnObject()
    {

        stonetype = Random.Range(1, 3);//岩の種類をランダム選出

        float randomX = Random.Range(GenelateErea_L.transform.localPosition.x, GenelateErea_R.transform.localPosition.x);//ランダムなX座標

        spawnPosition = new Vector2(randomX, height);//生成位置を設定

        var worldPoint = transform.TransformPoint(spawnPosition);//ワールド座標に変換

        worldPoint.y = height;//高さをワールド座標にするとずれるのでローカルに戻す

       // Debug.Log(spawnPosition);

        // Debug.Log("ワールド返還"+worldPoint);

        Instantiate(Stones[stonetype], worldPoint, Quaternion.identity);
    }
}
