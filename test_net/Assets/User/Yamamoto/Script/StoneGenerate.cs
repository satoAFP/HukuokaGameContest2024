using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerate : MonoBehaviour
{
    [SerializeField] private GameObject GenelateErea_L;//生成場所左
    [SerializeField] private GameObject GenelateErea_R;//生成場所右


    [SerializeField] private GameObject Stone_L;//大きい岩

    private float height;

    [SerializeField] private float spawnInterval = 1f; // 生成間隔

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

        if (timer >= spawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }

        //print("width: " + width);
    }

    public void SpawnObject()
    {
       

        float randomX = Random.Range(GenelateErea_L.transform.localPosition.x, GenelateErea_R.transform.localPosition.x);

        Debug.Log(randomX);

        spawnPosition = new Vector2(randomX, height);

        Debug.Log(spawnPosition);

        Instantiate(Stone_L, spawnPosition, Quaternion.identity);
    }
}
