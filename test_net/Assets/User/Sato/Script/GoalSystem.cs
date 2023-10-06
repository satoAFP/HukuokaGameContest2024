using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class GoalSystem : CGimmick
{
    [SerializeField] private Text text;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER1);

        }
        if (collision.gameObject.name == "Player2")
        {
            photonView.RPC(nameof(RpcClearCheck), RpcTarget.All, PLAYER2);
        }
    }

    

}
