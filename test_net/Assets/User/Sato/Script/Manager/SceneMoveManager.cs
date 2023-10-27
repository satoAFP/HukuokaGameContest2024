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
        StartCoroutine(DelaySceneMoveName());
    }

    private IEnumerator DelaySceneMoveName()
    {
        // 3秒間待つ
        yield return new WaitForSeconds(0.5f);

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
        yield return new WaitForSeconds(0.5f);

        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
