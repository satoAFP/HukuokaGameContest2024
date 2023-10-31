using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ResultSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("クリア時に出すオブジェクト")] private GameObject[] clearObjs;

    [SerializeField, Header("出す間隔")] private int intervalFrame;

    [SerializeField, Header("noTapArea")] private GameObject noTapArea;

    private int count = 0;      //フレームを数える
    private int objCount = 0;   //オブジェクトを数える

    private bool isRetry = false;       //リトライ選択したとき
    private bool isStageSelect = false; //ステージセレクト選択したとき

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ManagerAccessor.Instance.dataManager.isClear)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            //フレームカウント
            count++;
        }

        if (ManagerAccessor.Instance.dataManager.isDeth)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        //一定間隔で画像を出す
        if (count == intervalFrame && objCount < clearObjs.Length) 
        {
            clearObjs[objCount].SetActive(true);
            objCount++;

            count = 0;
        }

        //リトライ選択したとき
        if (isRetry)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }
        //ステージセレクト選択したとき
        if (isStageSelect)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    public void Retry()
    {
        photonView.RPC(nameof(RcpShareIsRetry), RpcTarget.All);
    }

    public void StageSelect()
    {
        photonView.RPC(nameof(RcpShareIsStageSelect), RpcTarget.All);
    }


    [PunRPC]
    private void RcpShareIsRetry()
    {
        isRetry = true;
        noTapArea.SetActive(true);
    }

    [PunRPC]
    private void RcpShareIsStageSelect()
    {
        isStageSelect = true;
        noTapArea.SetActive(true);
    }

}
