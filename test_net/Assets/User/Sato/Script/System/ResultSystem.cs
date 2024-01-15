using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ResultSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�N���A���ɏo���I�u�W�F�N�g")] private GameObject[] clearObjs;

    [SerializeField, Header("�I�[�i�[���s�񐔗p�e�L�X�g")] private Text OwnerMissText;
    [SerializeField, Header("�N���C�A���g���s�񐔗p�e�L�X�g")] private Text ClientMissText;

    [SerializeField, Header("�I�[�i�[�̕]���摜")] private GameObject[] OwnerEvaluationObjs;
    [SerializeField, Header("�N���C�A���g�̕]���摜")] private GameObject[] ClientEvaluationObjs;

    [SerializeField, Header("�]���`��p�e�L�X�g")] private Text EvaluationText;

    [SerializeField, Header("�N���A���̃{�^��")] private GameObject ClearButton;
    [SerializeField, Header("�Q�[���I�[�o�[���̃{�^��")] private GameObject GameOverButton;

    [SerializeField, Header("P1�̎��S�摜")] private GameObject P1DethImg;
    [SerializeField, Header("P2�̎��S�摜")] private GameObject P2DethImg;

    [SerializeField, Header("�ŏI�X�e�[�W�̏ꍇ�I���ɂ��Ă�������")] private bool isLast;

    [SerializeField, Header("�o���Ԋu")] private int intervalFrame;

    [SerializeField, Header("noTapArea")] private GameObject noTapArea;

    [SerializeField, Header("�N���A����BGM")] private AudioClip ClearBGM;
    [SerializeField, Header("�Q�[���I�[�o�[����BGM")] private AudioClip LoseBGM;

    private int count = 0;      //�t���[���𐔂���
    private int objCount = 0;   //�I�u�W�F�N�g�𐔂���

    private bool isRetry = false;       //���g���C�I�������Ƃ�
    private bool isStageSelect = false; //�X�e�[�W�Z���N�g�I�������Ƃ�

    private int ownerMemCount = 0;
    private int clientMemCount = 0;

    private bool first = true;

    private FadeAnimation fadeanimation;//�t�F�[�h�A�j���[�V�����X�N���v�g��������ϐ�
   
    //�N���A�����Ɉ�񂵂�����Ȃ�����
    private bool clearFirst;

    void Start()
    {
        fadeanimation = GameObject.Find("Fade").GetComponent<FadeAnimation>();//�t�F�[�h�A�j���[�V�����X�N���v�g�擾

        //�o�����̂�ς���
        if (PhotonNetwork.IsMasterClient)
        {
            ClearButton.transform.GetChild(0).gameObject.SetActive(true);
            GameOverButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            ClearButton.transform.GetChild(1).gameObject.SetActive(true);
            GameOverButton.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

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

        //���s�񐔕`��
        OwnerMissText.text = "����~�X�F" + ManagerAccessor.Instance.dataManager.ownerMissCount.ToString() + "��";
        ClientMissText.text = "����~�X�F" + ManagerAccessor.Instance.dataManager.clientMissCount.ToString() + "��";

        //�]���摜�\��
        //���ꂼ��̕]���擾
        int owner = ResultScoreChange(ManagerAccessor.Instance.dataManager.ownerMissCount);
        int client = ResultScoreChange(ManagerAccessor.Instance.dataManager.clientMissCount);


        if (ManagerAccessor.Instance.dataManager.isClear)
        {
            for (int i = 0; i < owner; i++)
                OwnerEvaluationObjs[i].SetActive(true);

            for (int i = 0; i < client; i++)
                ClientEvaluationObjs[i].SetActive(true);
        }

        //�]���`��
        EvaluationText.text = "��l�́u" + ResultScoreCheck() + "�v";


        //�N���A���
        if (ManagerAccessor.Instance.dataManager.isClear)
        {
            if (first)
            {
                //�N���A�p�l���\��
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                //�N���A���Z�[�u
                ManagerAccessor.Instance.saveDataManager.ClearDataSave(ManagerAccessor.Instance.sceneMoveManager.GetSceneName());

                //BGM�ύX
                ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().clip = ClearBGM;

                //�Đ�
                ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().Play();

                first = false;
            }

            //�t���[���J�E���g
            count++;
        }

        //�Q�[���I�[�o�[���
        if (!ManagerAccessor.Instance.dataManager.isClear
            &&ManagerAccessor.Instance.dataManager.isDeth)
        {
            if(fadeanimation.fadeoutfinish)
            {
                if (first)
                {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);

                    //BGM�ύX
                    ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().clip = LoseBGM;

                    //�Đ�
                    ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().Play();

                    //���S���̉摜�ύX
                    if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player1")
                        P1DethImg.SetActive(true);
                    if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player2")
                        P2DethImg.SetActive(true);

                    first = false;
                }
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
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }
    }

    public void StageSelect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
                Debug.Log(isLast);

            if (isLast)
            {
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Ending");
            }
            else
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Ending");
        }
    }

    //���U���g�̕]���`��p
    private string ResultScoreCheck()
    {
        //���ꂼ��̕]���擾
        int owner = ResultScoreChange(ManagerAccessor.Instance.dataManager.ownerMissCount);
        int client = ResultScoreChange(ManagerAccessor.Instance.dataManager.clientMissCount);

        if (owner == 3 && client == 3)
            return "�S�̗F";
        if ((owner == 3 && client == 2) || (owner == 2 && client == 3)) 
            return "�e�F";
        if ((owner == 3 && client == 1) || (owner == 1 && client == 3))
            return "���ܒ�";
        if ((owner == 3 && client == 0) || (owner == 0 && client == 3))
            return "�Ўv��";
        if (owner == 2 && client == 2)
            return "�F�B";
        if ((owner == 2 && client == 1) || (owner == 1 && client == 2))
            return "�F�B�̗F�B";
        if ((owner == 2 && client == 0) || (owner == 0 && client == 2))
            return "�U��̒�";
        if (owner == 1 && client == 1)
            return "�m�荇��";
        if ((owner == 1 && client == 0) || (owner == 0 && client == 1))
            return "���Ζ�";
        if (owner == 0 && client == 0)
            return "���l";

        return "";
    }

    //�~�X�̉񐔂����U���g�\���p�̃X�R�A�ɕύX
    private int ResultScoreChange(int miss)
    {
        if (0 <= miss && miss < 3)
            return 3;
        else if (3 <= miss && miss < 6)
            return 2;
        else if (6 <= miss && miss < 9)
            return 1;
        else if (9 <= miss)
            return 0;

        return 0;
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
