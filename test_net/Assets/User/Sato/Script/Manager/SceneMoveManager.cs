using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        ManagerAccessor.Instance.sceneMoveManager = this;
    }

    //�R���[�`���Ăяo���p
    public void SceneMoveName(string name)
    {
        //�ʐM���ꎞ�~�߂�
        PhotonNetwork.IsMessageQueueRunning = false;

        SceneManager.LoadScene(name);
    }
}
