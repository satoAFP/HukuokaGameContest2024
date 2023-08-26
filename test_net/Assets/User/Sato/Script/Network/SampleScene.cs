using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class SampleScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField roomName;

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
        if (logInFirst)
        {
            if (roomName.text.ToString() != "")
            {
                // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
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
            //ルームから抜ける処理
            PhotonNetwork.LeaveRoom();
            logOutFirst = false;
            logInFirst = true;
        }
    }

    // ルームから退出した時に呼ばれるコールバック
    public override void OnLeftRoom()
    {
        Debug.Log("ルームから退出しました");
        PhotonNetwork.LeaveLobby();
    }

    // ロビーへ参加した時に呼ばれるコールバック
    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーへ参加しました");
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        if (first)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName.text.ToString(), roomOptions, TypedLobby.Default);
            first = false;
        }

        Debug.Log("参加中");
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        var players = PhotonNetwork.PlayerList;
        Debug.Log(players.Length);

        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("Block", position, Quaternion.identity);
    }
}