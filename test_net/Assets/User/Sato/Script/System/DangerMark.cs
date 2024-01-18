using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DangerMark : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("落石")] private Transform stoneGenelate;

    [SerializeField, Header("落石の前後どの座標に生成するか")] private float genelatePosX;
    [SerializeField, Header("プレイヤーの上下どの座標に生成するか")] private float genelatePosY;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.zero;

        //表示する座標の設定
        if (PhotonNetwork.IsMasterClient)
            pos = new Vector3(stoneGenelate.position.x + genelatePosX, ManagerAccessor.Instance.dataManager.player1.transform.position.y + genelatePosY);
        else
            pos = new Vector3(stoneGenelate.position.x + genelatePosX, ManagerAccessor.Instance.dataManager.player2.transform.position.y + genelatePosY);

        //座標変更
        transform.position = pos;


    }
}
