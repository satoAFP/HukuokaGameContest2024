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
        // 3秒間待つ
        yield return new WaitForSeconds(1.0f);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(name);
        }
    }

    /// <summary>
    /// リトライ用
    /// </summary>
    public void SceneMoveRetry()
    {
        // コルーチンの起動
        StartCoroutine(DelaySceneMoveRetry());
    }

    private IEnumerator DelaySceneMoveRetry()
    {
        // 3秒間待つ
        yield return new WaitForSeconds(1.0f);

        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
