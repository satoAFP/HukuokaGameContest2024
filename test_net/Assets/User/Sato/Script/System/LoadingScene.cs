using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadingScene : MonoBehaviourPunCallbacks
{
    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        if(first)
        {
            if (PhotonNetwork.IsMasterClient)
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName(GlobalSceneName.SceneName);
            first = false;
        }
    }
}
