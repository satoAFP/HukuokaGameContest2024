using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleteTime : MonoBehaviour
{
    [SerializeField, Header("エフェクトが消えるまでの時間")] private int DeleteTime;

    [SerializeField, Header("爆発SE")] AudioClip bombSE;

    private AudioSource audioSource;

    private int count = 0;//フレームカウント用

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //SE再生
        audioSource.PlayOneShot(bombSE);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //設定時間で削除
        count++;
        if (count == DeleteTime) 
        {
            Destroy(gameObject);
        }
    }
}
