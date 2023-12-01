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
    /// �����̖��O�̃V�[���ֈړ�
    /// </summary>
    /// <param name="name">�ړ�����V�[����</param>
    public void SceneMoveName(string name)
    {
        // �R���[�`���̋N��
        StartCoroutine(DelaySceneMoveName(name));
    }

    private IEnumerator DelaySceneMoveName(string name)
    {
        // 1�b�ԑ҂�
        yield return new WaitForSeconds(1.0f);

        PhotonNetwork.LoadLevel(name);

    }

    /// <summary>
    /// ���g���C�p
    /// </summary>
    public void SceneMoveRetry()
    {
        ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("LoadScene");
    }


    //���݂̃V�[���擾�p�֐�
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


    
}

public static class GlobalSceneName
{
    public static string SceneName = "";
}