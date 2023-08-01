using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LogInSystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject NoTapArea;

    private int roomNumber = 0;

    private bool logInFirst = true;
    private bool logOutFirst = false;
    private bool first = true;

    private void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";

        
    }

    public void LogIn(int room)
    {
        //��ʂ�G��Ȃ�����
        NoTapArea.SetActive(true);

        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

        //���[���ԍ��ݒ�
        roomNumber = room;
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
        roomOptions.MaxPlayers = 2;

        for (int i = 0; i < 5; i++)
            PhotonNetwork.CreateRoom("Room" + i, roomOptions, TypedLobby.Default);

    }


    public override void OnCreatedRoom()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinRoom("Room" + roomNumber);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("�Q���Ɏ��s");
    }

    // ���[���̍쐬�����s�������ɌĂ΂��R�[���o�b�N
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("���[���̍쐬�Ɏ��s���܂���");
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        Debug.Log("�Q���ɐ���");

        var players = PhotonNetwork.PlayerList;
        Debug.Log(players.Length);

        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Block", position, Quaternion.identity);
    }
}
