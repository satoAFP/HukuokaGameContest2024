using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickFallStoneManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("óéêŒ")] private GameObject stone;

    [SerializeField, Header("ê∂ê¨ä‘äu")] private int CreateFrame;

    private int frameCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            frameCount++;
            if (frameCount == CreateFrame)
            {
                Vector2 pos = new Vector2(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), transform.position.y);

                PhotonNetwork.Instantiate("GimmickStone", pos, Quaternion.identity);
            }
        }
    }
}
