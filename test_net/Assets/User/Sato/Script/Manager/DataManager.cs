using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviourPunCallbacks
{
    [Header("�X�e�[�W��")] public int StageNum;

    [Header("�~�X�܂ł̃t���[��")] public int MissFrame;

    [Header("BGM")] public GameObject BGM;

    [Header("�������G�t�F�N�g")] public GameObject StarEffect;

    //���ꂼ��̃N���A��
    [System.NonSerialized] public bool isOwnerClear = false;
    [System.NonSerialized] public bool isClientClear = false;

    public bool GetSetIsOwnerClear
    {
        get { return isOwnerClear; }
        set { isOwnerClear = value; }
    }

    public bool GetSetIsClientClear
    {
        get { return isClientClear; }
        set { isClientClear = value; }
    }


    //�v���C���[�I�u�W�F�N�g�擾
    [System.NonSerialized] public GameObject player1 = null;
    [System.NonSerialized] public GameObject player2 = null;
    [System.NonSerialized] public GameObject copyKey = null;
    [System.NonSerialized] public GameObject board = null;

    //�A�����b�N�{�^�����쒆���ǂ���
    [System.NonSerialized] public bool isUnlockButtonStart = false;

    //Locket���쒆���ǂ���
    [System.NonSerialized] public bool isFlyStart = false;
    [System.NonSerialized] public Vector3 flyPos = Vector3.zero;

    //�R�s�[���o�������ǂ���
    [System.NonSerialized] public bool isAppearCopyKey = false;

    //�󔠂��J����Ȃ����
    [System.NonSerialized] public bool isNotOpenBox = false;
    [System.NonSerialized] public bool isOwnerNotOpenBox = false;
    [System.NonSerialized] public bool isClientNotOpenBox = false;

    //�X�e�[�W�ړ����t���O
    [System.NonSerialized] public bool isStageMove = false;

    //�N���A�t���O
    [System.NonSerialized] public bool isClear = false;

    //�N���A�t���O
    [System.NonSerialized] public bool isEnterGoal = false;

    //���S�t���O
    [System.NonSerialized] public bool isDeth = false;

    //�|�[�Y�t���O
    [System.NonSerialized] public bool isPause = false;

    //���S�����v���C���[�̖��O���擾
    [System.NonSerialized] public string DeathPlayerName = null;

    //�~�X�̉�
    [System.NonSerialized] public int ownerMissCount = 0;
    [System.NonSerialized] public int clientMissCount = 0;

    //���E������
    [System.NonSerialized] public bool isOwnerHitRight = false;
    [System.NonSerialized] public bool isClientHitRight = false;
    [System.NonSerialized] public bool isOwnerHitLeft = false;
    [System.NonSerialized] public bool isClientHitLeft = false;
    [System.NonSerialized] public bool isOwnerHitDown = false;
    [System.NonSerialized] public bool isClientHitDown = false;


    //�L�[���͏��(�}�X�^�[)
    //�L�[�{�[�h����
    [System.NonSerialized] public bool isOwnerInputKey_A = false;
    [System.NonSerialized] public bool isOwnerInputKey_D = false;
    [System.NonSerialized] public bool isOwnerInputKey_W = false;
    [System.NonSerialized] public bool isOwnerInputKey_S = false;
    [System.NonSerialized] public bool isOwnerInputKey_B = false;
    //�}�E�X����
    [System.NonSerialized] public bool isOwnerInputKey_LM = false;
    //�R���g���[���[�{�^��
    [System.NonSerialized] public bool isOwnerInputKey_CA = false;
    [System.NonSerialized] public bool isOwnerInputKey_CB = false;
    [System.NonSerialized] public bool isOwnerInputKey_CX = false;
    [System.NonSerialized] public bool isOwnerInputKey_CY = false;
    //�R���g���[���[���X�e�B�b�N
    [System.NonSerialized] public bool isOwnerInputKey_C_L_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_LEFT  = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_UP    = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L_DOWN  = false;
    //�R���g���[���[�E�X�e�B�b�N
    [System.NonSerialized] public bool isOwnerInputKey_C_R_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R_LEFT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R_UP = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R_DOWN = false;
    //�R���g���[���[�\���L�[
    [System.NonSerialized] public bool isOwnerInputKey_C_D_RIGHT = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_LEFT  = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_UP    = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_D_DOWN  = false;
    //�R���g���[���[��{�^�����g���K�[
    [System.NonSerialized] public bool isOwnerInputKey_C_R1 = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_R2 = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L1 = false;
    [System.NonSerialized] public bool isOwnerInputKey_C_L2 = false;
    //�R���g���[���[�|�[�Y
    [System.NonSerialized] public bool isOwnerInputKeyPause = false;

    //�L�[���͏��(�N���C�A���g)
    //�L�[�{�[�h����
    [System.NonSerialized] public bool isClientInputKey_A = false;
    [System.NonSerialized] public bool isClientInputKey_D = false;
    [System.NonSerialized] public bool isClientInputKey_W = false;
    [System.NonSerialized] public bool isClientInputKey_S = false;
    [System.NonSerialized] public bool isClientInputKey_B = false;
    //�}�E�X����
    [System.NonSerialized] public bool isClientInputKey_LM = false;
    //�R���g���[���[�{�^��
    [System.NonSerialized] public bool isClientInputKey_CA = false;
    [System.NonSerialized] public bool isClientInputKey_CB = false;
    [System.NonSerialized] public bool isClientInputKey_CX = false;
    [System.NonSerialized] public bool isClientInputKey_CY = false;
    //�R���g���[���[���X�e�B�b�N
    [System.NonSerialized] public bool isClientInputKey_C_L_RIGHT = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_LEFT  = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_UP    = false;
    [System.NonSerialized] public bool isClientInputKey_C_L_DOWN  = false;
    //�R���g���[���[�E�X�e�B�b�N
    [System.NonSerialized] public bool isClientInputKey_C_R_RIGHT = false;
    [System.NonSerialized] public bool isClientInputKey_C_R_LEFT = false;
    [System.NonSerialized] public bool isClientInputKey_C_R_UP = false;
    [System.NonSerialized] public bool isClientInputKey_C_R_DOWN = false;
    //�R���g���[���[�\���L�[
    [System.NonSerialized] public bool isClientInputKey_C_D_RIGHT = false;//�p�b�h�̏\���L�[
    [System.NonSerialized] public bool isClientInputKey_C_D_LEFT  = false;
    [System.NonSerialized] public bool isClientInputKey_C_D_UP    = false;
    [System.NonSerialized] public bool isClientInputKey_C_D_DOWN  = false;
    //�R���g���[���[��{�^�����g���K�[
    [System.NonSerialized] public bool isClientInputKey_C_R1 = false;
    [System.NonSerialized] public bool isClientInputKey_C_R2 = false;
    [System.NonSerialized] public bool isClientInputKey_C_L1 = false;
    [System.NonSerialized] public bool isClientInputKey_C_L2 = false;
    //�R���g���[���[�|�[�Y
    [System.NonSerialized] public bool isClientInputKeyPause = false;


    //���ꂼ��̃{�^�����͏�
    [System.NonSerialized] public string ownerName = "";
    [System.NonSerialized] public string clientName = "";


    public Text text;

    public Text chat;

    public Text clear;




    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.dataManager = this;
    }

    private void Update()
    {
        if (ManagerAccessor.Instance.dataManager.player1 != null &&
            ManagerAccessor.Instance.dataManager.player2 != null)
        {
            //�X�^�[�g�A�S�[���ɂ���Ƃ��󔠂������Ȃ��悤�ɂ���
            if (ManagerAccessor.Instance.dataManager.isOwnerNotOpenBox ||
                ManagerAccessor.Instance.dataManager.isClientNotOpenBox)
                ManagerAccessor.Instance.dataManager.isNotOpenBox = true;
            else
                ManagerAccessor.Instance.dataManager.isNotOpenBox = false;
        }

        //if(Input.GetMouseButton(0))
        //{
        //    Instantiate(ManagerAccessor.Instance.dataManager.StarEffect);
        //}
    }


    //�v���C���[�擾�p�֐�
    public GameObject GetPlyerObj(string name)
    {
        //�v���C���[�擾
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");

        //���ꂼ�ꖼ�O����v������Ԃ�
        for (int i = 0; i < p.Length; i++) 
        {
            if (p[i].name == name)
                return p[i];
        }

        return null;
    }

    
}
