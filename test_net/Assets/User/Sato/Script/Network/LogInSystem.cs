using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LogInSystem : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject NoTapArea;

    
    private int roomNumber = 0;         //ルーム番号
    private int roomMember = 2;         //ルーム内の最大人数
    private bool logInSuccess = false;  //ログイン成功時

    private void Start()
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);

        //サーバーから切断
        if (GlobalSceneName.SceneName == "Ending")
            PhotonNetwork.Disconnect();
    }

    private void Update()
    {
        if(logInSuccess)
        {
            //ルーム内の人数カウント
            int i = 0;
            foreach (var p in PhotonNetwork.PlayerList)
                i++;

            //メンバーがそろったらシーン移動
            //if (i == 2)
            //    ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    public void LogIn(int room)
    {
        //画面を触れなくする
        NoTapArea.SetActive(true);

        //ルーム番号設定
        roomNumber = room;

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ロビーへ参加する
        PhotonNetwork.JoinLobby();
    }

    // ロビーへ参加した時に呼ばれるコールバック
    public override void OnJoinedLobby()
    {
        //ルームのログイン最大数設定
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = roomMember;

        //ルームの作成
        PhotonNetwork.CreateRoom("Room" + roomNumber, roomOptions, TypedLobby.Default);
    }


    public override void OnCreatedRoom()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinRoom("Room" + roomNumber);
    }

    //ルームの参加が失敗した時に呼ばれるコールバック
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //画面を触れるようにする
        NoTapArea.SetActive(false);
        //Photonのサーバーから切断する
        PhotonNetwork.Disconnect();
        ManagerAccessor.Instance.dataManager.text.text = "ルーム参加失敗" + roomNumber;
    }

    // ルームの作成が失敗した時に呼ばれるコールバック
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //失敗時はそのルームに参加する
        PhotonNetwork.JoinRoom("Room" + roomNumber);
        ManagerAccessor.Instance.dataManager.text.text = "ルーム作成失敗" + roomNumber;
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        

        //接続成功
        
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
