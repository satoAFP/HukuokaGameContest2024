using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickFallStoneManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("落石")] private GameObject stone;

    [SerializeField, Header("生成間隔")] private int CreateFrame;

    //フレームカウント用
    private int frameCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ManagerAccessor.Instance.dataManager.isFlyStart)
        {
            //マスターのみ生成
            if (PhotonNetwork.IsMasterClient)
            {
                //一定間隔で生成
                frameCount++;
                if (frameCount == CreateFrame)
                {
                    //範囲内のランダムな座標
                    Vector2 pos = new Vector2(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), transform.position.y);

                    //落石生成
                    PhotonNetwork.Instantiate("GimmickStone", pos, Quaternion.identity);
                    frameCount = 0;
                }
            }
        }
        
    }
}
