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

    [SerializeField, Header("StageSelectでの出現位置設定用")] private GameObject[] stage;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;


        if (ManagerAccessor.Instance.sceneMoveManager.GetSceneName() == "StageSelect") 
        {
            for (int i = 0; i < ManagerAccessor.Instance.dataManager.StageNum; i++) 
            {
                if ("Stage" + (i + 1) == GlobalSceneName.SceneName) 
                {
                    p1pos = new Vector2(stage[i].transform.position.x + 0.5f, stage[i].transform.position.y);
                    p2pos = new Vector2(stage[i].transform.position.x - 0.5f, stage[i].transform.position.y);
                }
            }
        }

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
