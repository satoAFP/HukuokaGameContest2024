using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageSelect : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("移動するシーン名")] private string sceneName;

    private bool isPlayerEnter = false;
    private bool first = true;


    // Update is called once per frame
    void Update()
    {
        if (isPlayerEnter)
        {
            if (first)
            {
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName(sceneName);
                first = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "Player2") 
        {
            photonView.RPC(nameof(RpcShareIsPlayerEnter), RpcTarget.All, true);
        }
    }

    //シーン切り替え情報共有
    [PunRPC]
    private void RpcShareIsPlayerEnter(bool data)
    {
        isPlayerEnter = data;
    }

}
