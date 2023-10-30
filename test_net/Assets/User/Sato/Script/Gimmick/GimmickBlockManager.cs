using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickBlockManager : MonoBehaviourPunCallbacks
{
    [SerializeField,Header("liftblockê∂ê¨ópç¿ïW")]private Vector2[] blockInitPos;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for (int i = 0; i < blockInitPos.Length; i++)
            {
                PhotonNetwork.Instantiate("LiftBlock", blockInitPos[i], Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
