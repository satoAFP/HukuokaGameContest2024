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
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        
    }

    public void LogIn(int room)
    {
        //画面を触れなくする
        NoTapArea.SetActive(true);

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();

        //ルーム番号設定
        roomNumber = room;
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
        roomOptions.MaxPlayers = 2;

        for (int i = 0; i < 5; i++)
            PhotonNetwork.CreateRoom("Room" + i, roomOptions, TypedLobby.Default);

    }


    public override void OnCreatedRoom()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinRoom("Room" + roomNumber);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("参加に失敗");
    }

    // ルームの作成が失敗した時に呼ばれるコールバック
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("ルームの作成に失敗しました");
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("参加に成功");

        var players = PhotonNetwork.PlayerList;
        Debug.Log(players.Length);

        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Block", position, Quaternion.identity);
    }
}
