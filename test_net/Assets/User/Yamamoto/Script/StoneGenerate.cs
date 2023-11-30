using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGenerate : MonoBehaviour
{
    [SerializeField] private GameObject GenelateErea_L;//�����ꏊ��
    [SerializeField] private GameObject GenelateErea_R;//�����ꏊ�E


    [SerializeField] private GameObject Stone_L;//�傫����

    private float height;

    [SerializeField] private float spawnInterval = 1f; // �����Ԋu

    private float timer = 0f;//���Ԃ��J�E���g

    private Vector2 spawnPosition;//�␶���ꏊ

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
