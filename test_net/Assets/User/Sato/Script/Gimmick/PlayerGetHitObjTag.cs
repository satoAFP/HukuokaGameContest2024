using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGetHitObjTag : MonoBehaviourPunCallbacks
{
    [System.NonSerialized] public List<string> HitTags = new List<string>();

    [SerializeField, Header("ローカル座標")] private Vector3 localPos;

    private void Update()
    {
        //データマネージャー取得
        DataManager dataManager = ManagerAccessor.Instance.dataManager;

        //座標を親オブジェクトに合わせる
        transform.position = transform.parent.transform.position + localPos;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor" || collision.tag == "Gimmick")
        {
            //触れたオブジェクトのタグ記憶
            HitTags.Add(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //出たオブジェクトのタグを消去
        for (int i = 0; i < HitTags.Count; i++)
        {
            if (HitTags[i] == collision.tag)
            {
                HitTags.RemoveAt(i);
            }
        }
    }
}
