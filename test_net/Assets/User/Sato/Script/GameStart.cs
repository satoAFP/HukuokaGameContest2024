using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameStart : MonoBehaviourPunCallbacks
{
    private const int PLAYER1 = 1;
    private const int PLAYER2 = 2;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        //ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        GameObject clone = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        //PhotonNetwork.Instantiate("LiftBlock", new Vector3(2, -3.5f), Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            //position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            PhotonNetwork.Instantiate("ShareDatas", position, Quaternion.identity);
        }

    }

}
