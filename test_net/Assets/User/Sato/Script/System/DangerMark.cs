using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DangerMark : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("落石")] private Transform stoneGenelate;
    [SerializeField, Header("カメラ")] private Transform cameraGenelate;

    [SerializeField, Header("落石の前後どの座標に生成するか")] private float genelatePosX;
    [SerializeField, Header("プレイヤーの上下どの座標に生成するか")] private float genelatePosY;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.zero;

        //表示する座標の設定
        pos = new Vector3(stoneGenelate.position.x + genelatePosX, cameraGenelate.position.y + genelatePosY);

        //座標変更
        transform.position = pos;


    }
}
