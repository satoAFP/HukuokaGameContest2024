using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackGroundScroll : MonoBehaviourPunCallbacks
{
    private GameObject player = null;

    [SerializeField] private GameObject[] imgObj = new GameObject[9];

    private Vector3 memPos = Vector3.zero;

    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (first)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                player = ManagerAccessor.Instance.dataManager.player1;
                memPos = player.transform.position;
                first = false;
            }
            else
            {
                player = ManagerAccessor.Instance.dataManager.player2;
                memPos = player.transform.position;
                first = false;
            }
        }

        




    }
}
