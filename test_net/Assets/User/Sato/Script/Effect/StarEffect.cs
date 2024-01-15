using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    [SerializeField, Header("出すエフェクト")] private GameObject star;

    [SerializeField, Header("出す星の数")] private int initNum;

    [SerializeField, Header("消えるまでの時間")] private int killTime;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initNum; i++) 
        {
            GameObject clone = Instantiate(star);
            clone.transform.parent = gameObject.transform;
            clone.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        if (killTime == count) 
        {
            Destroy(gameObject);
        }
    }
}
