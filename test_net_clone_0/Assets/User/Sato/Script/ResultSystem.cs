using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ResultSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�N���A���ɏo���I�u�W�F�N�g")] private GameObject[] clearObjs;

    [SerializeField, Header("�o���Ԋu")] private int intervalFrame;

    [SerializeField, Header("noTapArea")] private GameObject noTapArea;

    private int count = 0;      //�t���[���𐔂���
    private int objCount = 0;   //�I�u�W�F�N�g�𐔂���

    private bool isRetry = false;       //���g���C�I�������Ƃ�
    private bool isStageSelect = false; //�X�e�[�W�Z���N�g�I�������Ƃ�

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ManagerAccessor.Instance.dataManager.isClear)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            //�t���[���J�E���g
            count++;
        }

        if (ManagerAccessor.Instance.dataManager.isDeth)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        //���Ԋu�ŉ摜���o��
        if (count == intervalFrame && objCount < clearObjs.Length) 
        {
            clearObjs[objCount].SetActive(true);
            objCount++;

            count = 0;
        }

        //���g���C�I�������Ƃ�
        if (isRetry)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }
        //�X�e�[�W�Z���N�g�I�������Ƃ�
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
