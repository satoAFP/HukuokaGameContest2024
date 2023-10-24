using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageSelect : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�ړ�����V�[����")] private string sceneName;

    private bool isPlayerEnter = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerEnter)
        {
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName(sceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name=="Player1"|| collision.name == "Player2")
        {
            photonView.RPC(nameof(RpcShareIsPlayerEnter), RpcTarget.All, true);
        }
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareIsPlayerEnter(bool data)
    {
        isPlayerEnter = data;
    }

}
