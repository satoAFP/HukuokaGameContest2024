using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        ManagerAccessor.Instance.sceneMoveManager = this;
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
        GlobalSceneName.SceneName = GetSceneName();
        ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("LoadScene");
    }


    //現在のシー名取得用関数
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


    
}

public static class GlobalSceneName
{
    public static string SceneName = "";
}