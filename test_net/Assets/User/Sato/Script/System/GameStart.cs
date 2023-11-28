using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameStart : MonoBehaviourPunCallbacks
{
    private const int PLAYER1 = 1;
    private const int PLAYER2 = 2;

    [SerializeField, Header("プレイヤー1の出現座標")] private Vector2 p1pos;
    [SerializeField, Header("プレイヤー2の出現座標")] private Vector2 p2pos;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        //自身のアバター（ネットワークオブジェクト）を生成する
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            GameObject clone = PhotonNetwork.Instantiate("Avatar", p1pos, Quaternion.identity);
        }
        else
        {
            GameObject clone = PhotonNetwork.Instantiate("Avatar", p2pos, Quaternion.identity);
        }
    }

}
