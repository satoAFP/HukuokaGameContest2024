using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LogInSystem : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject NoTapArea;

    
    private int roomNumber = 0;         //���[���ԍ�
    private int roomMember = 2;         //���[�����̍ő�l��
    private bool logInSuccess = false;  //���O�C��������

    private void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);

        //�T�[�o�[����ؒf
        if (GlobalSceneName.SceneName == "Ending")
            PhotonNetwork.Disconnect();
    }

    private void Update()
    {
        if(logInSuccess)
        {
            //���[�����̐l���J�E���g
            int i = 0;
            foreach (var p in PhotonNetwork.PlayerList)
                i++;

            //�����o�[�����������V�[���ړ�
            //if (i == 2)
            //    ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    public void LogIn(int room)
    {
        //��ʂ�G��Ȃ�����
        NoTapArea.SetActive(true);

        //���[���ԍ��ݒ�
        roomNumber = room;

        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // ���r�[�֎Q������
        PhotonNetwork.JoinLobby();
    }

    // ���r�[�֎Q���������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedLobby()
    {
        //���[���̃��O�C���ő吔�ݒ�
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = roomMember;

        //���[���̍쐬
        PhotonNetwork.CreateRoom("Room" + roomNumber, roomOptions, TypedLobby.Default);
    }


    public override void OnCreatedRoom()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinRoom("Room" + roomNumber);
    }

    //���[���̎Q�������s�������ɌĂ΂��R�[���o�b�N
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //��ʂ�G���悤�ɂ���
        NoTapArea.SetActive(false);
        //Photon�̃T�[�o�[����ؒf����
        PhotonNetwork.Disconnect();
        ManagerAccessor.Instance.dataManager.text.text = "���[���Q�����s" + roomNumber;
    }

    // ���[���̍쐬�����s�������ɌĂ΂��R�[���o�b�N
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //���s���͂��̃��[���ɎQ������
        PhotonNetwork.JoinRoom("Room" + roomNumber);
        ManagerAccessor.Instance.dataManager.text.text = "���[���쐬���s" + roomNumber;
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        

        //�ڑ�����
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == roomMember)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            
            if (roomNumber < 5)
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Opening");
            else if (roomNumber == 5)
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Sato");
            else if (roomNumber == 6)
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Yamamoto");
        }
    }
}
