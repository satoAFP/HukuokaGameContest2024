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


    private int ownerMemCount = 0;
    private int clientMemCount = 0;
    private bool firstdeath = true;//��x�����Q�[���I�[�o�[�̏�����ʂ�

    private float resultspawnInterval = 1.0f;//�t�F�[�h�C���E�A�E�g�̌�Ƀ��U���g��ʂ��o�����߂̃J�E���g
    private float timer = 0f;//���Ԃ��J�E���g

    //�N���A�����Ɉ�񂵂�����Ȃ�����
    private bool clearFirst;

    // Update is called once per frame
    void FixedUpdate()
    {
        //2P�Ƀ~�X�f�[�^���L
        if (PhotonNetwork.IsMasterClient)
        {
            if (ownerMemCount != ManagerAccessor.Instance.dataManager.ownerMissCount)
            {
                photonView.RPC(nameof(RpcShareOwnerMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.ownerMissCount);
                ownerMemCount = ManagerAccessor.Instance.dataManager.ownerMissCount;
            }
            if (clientMemCount != ManagerAccessor.Instance.dataManager.clientMissCount)
            {
                photonView.RPC(nameof(RpcShareClientMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.clientMissCount);
                clientMemCount = ManagerAccessor.Instance.dataManager.clientMissCount;
            }
        }


        if (ManagerAccessor.Instance.dataManager.isClear)
        {
            //�N���A�p�l���\��
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            //�N���A���Z�[�u
            ManagerAccessor.Instance.saveDataManager.ClearDataSave(ManagerAccessor.Instance.sceneMoveManager.GetSceneName());

            //�t���[���J�E���g
            count++;
        }

        //�Q�[���I�[�o�[���
        if (ManagerAccessor.Instance.dataManager.isDeth
            && firstdeath)
        {
            timer += Time.deltaTime;

            //�����Ŋ�𐶐����鎞�Ԃ��v��
            if (timer >= resultspawnInterval)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                timer = 0f;
                firstdeath = false;
            }
        }

        //���Ԋu�ŉ摜���o��
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
