using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ResultSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("����I�ɏo���I�u�W�F�N�g")] private GameObject[] objs;

    [SerializeField, Header("�o���Ԋu")] private int intervalFrame;

    [SerializeField, Header("noTapArea")] private GameObject noTapArea;

    private int count = 0;      //�t���[���𐔂���
    private int objCount = 0;   //�I�u�W�F�N�g�𐔂���

    private bool isRetry = false;
    private bool isStageSelect = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;

        if (count == intervalFrame && objCount < objs.Length) 
        {
            objs[objCount].SetActive(true);
            objCount++;

            count = 0;
        }

        if(isRetry)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }

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
