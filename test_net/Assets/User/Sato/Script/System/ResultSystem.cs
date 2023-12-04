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


    private int ownerMemCount = 0;
    private int clientMemCount = 0;

    //クリア処理に一回しか入らない処理
    private bool clearFirst;

    // Update is called once per frame
    void FixedUpdate()
    {
        //2Pにミスデータ共有
        if (PhotonNetwork.IsMasterClient)
        {
            if (ownerMemCount != ManagerAccessor.Instance.dataManager.ownerMissCount)
            {
                photonView.RPC(nameof(RpcShareOwnerMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.ownerMissCount);
                ownerMemCount = ManagerAccessor.Instance.dataManager.ownerMissCount;
            }
            if (ownerMemCount != ManagerAccessor.Instance.dataManager.clientMissCount)
            {
                photonView.RPC(nameof(RpcShareClientMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.clientMissCount);
                clientMemCount = ManagerAccessor.Instance.dataManager.clientMissCount;
            }
        }


        if (ManagerAccessor.Instance.dataManager.isClear)
        {
            //クリアパネル表示
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //クリア情報セーブ
            ManagerAccessor.Instance.saveDataManager.ClearDataSave(ManagerAccessor.Instance.sceneMoveManager.GetSceneName());

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
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }
    }

    [PunRPC]
    private void RcpShareIsStageSelect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    [PunRPC]
    private void RpcShareOwnerMissCount(int miss)
    {
        ManagerAccessor.Instance.dataManager.ownerMissCount = miss;
    }

    [PunRPC]
    private void RpcShareClientMissCount(int miss)
    {
        ManagerAccessor.Instance.dataManager.clientMissCount = miss;
    }
}
