using Photon.Pun;
using System.Collections;
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

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    /// <summary>
    /// �����̖��O�̃V�[���ֈړ�
    /// </summary>
    /// <param name="name">�ړ�����V�[����</param>
    public void SceneMoveName(string name)
    {
        // �R���[�`���̋N��
        StartCoroutine(DelaySceneMoveName());
    }

    private IEnumerator DelaySceneMoveName()
    {
        // 3�b�ԑ҂�
        yield return new WaitForSeconds(0.5f);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(name);
        }
    }

    /// <summary>
    /// ���g���C�p
    /// </summary>
    public void SceneMoveRetry()
    {
        // �R���[�`���̋N��
        StartCoroutine(DelaySceneMoveRetry());
    }

    private IEnumerator DelaySceneMoveRetry()
    {
        // 3�b�ԑ҂�
        yield return new WaitForSeconds(0.5f);

        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
