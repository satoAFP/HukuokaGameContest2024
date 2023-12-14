using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopErea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //落石エリアに入るとゲームオーバーのシーン
        if (collision.gameObject.tag == "DeathErea")
        {
            Debug.Log("エリアストップ");

        }
    }


}
