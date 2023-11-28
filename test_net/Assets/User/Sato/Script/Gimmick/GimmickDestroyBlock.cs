using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickDestroyBlock : MonoBehaviour
{
    [SerializeField, Header("フェードの速度")] private int FeedSpeed;

    [System.NonSerialized] public bool DestroyStart = false;//消滅中

    // Update is called once per frame
    void FixedUpdate()
    {
        //フェード＆当たり判定消す
        if(DestroyStart)
        {
            GetComponent<BoxCollider2D>().enabled = false;

            GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
        }
        //透明になった段階で削除
        if (GetComponent<SpriteRenderer>().color.a <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
