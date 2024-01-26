using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        ManagerAccessor.Instance.sceneMoveManager = this;

        if(GetSceneName()!="Title"&& GetSceneName() != "StageSelect" && GetSceneName() != "LoadScene")
        {
            GlobalSceneName.SceneName = GetSceneName();
        }
    }

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    /// <summary>
    /// 引数の名前のシーンへ移動
    /// </summary>
    /// <param name="name">移動するシーン名</param>
    public void SceneMoveName(string name)
    {
        // コルーチンの起動
        StartCoroutine(DelaySceneMoveName(name));
    }

    private IEnumerator DelaySceneMoveName(string name)
    {
        // 1秒間待つ
        yield return new WaitForSeconds(1.0f);

        PhotonNetwork.LoadLevel(name);

    }

    /// <summary>
    /// リトライ用
    /// </summary>
    public void SceneMoveRetry()
    {
        ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("LoadScene");
    }


    //現在のシー名取得用関数
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // 他のプレイヤーが切断されたときに呼ばれるコールバック
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("NewTitle");

        // ロビーから退出する
        PhotonNetwork.Disconnect();
    }

}

public static class GlobalSceneName
{
    public static string SceneName = "";
}