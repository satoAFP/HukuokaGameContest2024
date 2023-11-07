using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGetHitObjTag : MonoBehaviourPunCallbacks
{
    [System.NonSerialized] public List<string> HitTags;

    [SerializeField, Header("ローカル座標")] private Vector3 localPos;

    private void Update()
    {
        //データマネージャー取得
        DataManager dataManager = ManagerAccessor.Instance.dataManager;

        //プレイヤーが生成されているとき
        if (dataManager.player1 != null && dataManager.player2 != null)
        {
            //それぞれの座標に移動させる
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                transform.position = dataManager.player1.transform.position + localPos;
            }
            else
            {
                transform.position = dataManager.player2.transform.position + localPos;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //触れたオブジェクトのタグ記憶
        if (photonView.IsMine)
        {
            HitTags.Add(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //出たオブジェクトのタグを消去
        if (photonView.IsMine)
        {
            for (int i = 0; i < HitTags.Count; i++)
            {
                if (HitTags[i] == collision.tag)
                {
                    HitTags.RemoveAt(i);
                }
            }
        }
    }
}
