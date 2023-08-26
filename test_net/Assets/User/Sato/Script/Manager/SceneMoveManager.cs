using Photon.Pun;
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

    //コルーチン呼び出し用
    public void SceneMoveName(string name)
    {
        //通信を一時止める
        PhotonNetwork.IsMessageQueueRunning = false;

        SceneManager.LoadScene(name);
    }
}
