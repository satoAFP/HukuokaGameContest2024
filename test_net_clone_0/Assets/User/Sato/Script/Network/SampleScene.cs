using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// MonoBehaviourPunCallbacks���p�����āAPUN�̃R�[���o�b�N���󂯎���悤�ɂ���
public class SampleScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField roomName;

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
        if (logInFirst)
        {
            if (roomName.text.ToString() != "")
            {
                // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
                PhotonNetwork.ConnectUsingSettings();
                logInFirst = false;
                logOutFirst = true;
            }
        }
    }

    public void LogOut()
    {
        if (logOutFirst)
        {
            //���[�����甲���鏈��
            PhotonNetwork.LeaveRoom();
            logOutFirst = false;
            logInFirst = true;
        }
    }

    // ���[������ޏo�������ɌĂ΂��R�[���o�b�N
    public override void OnLeftRoom()
    {
        Debug.Log("���[������ޏo���܂���");
        PhotonNetwork.LeaveLobby();
    }

    // ���r�[�֎Q���������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedLobby()
    {
        Debug.Log("���r�[�֎Q�����܂���");
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        if (first)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName.text.ToString(), roomOptions, TypedLobby.Default);
            first = false;
        }

        Debug.Log("�Q����");
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        var players = PhotonNetwork.PlayerList;
        Debug.Log(players.Length);

        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Block", position, Quaternion.identity);
    }
}