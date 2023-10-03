using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class testplayer : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    private float moveSpeed;

    
    void Start()
    {
        //–¼‘O‚ÆID‚ğİ’è
        gameObject.name = "Player" + photonView.OwnerActorNr;
    }


    void Update()
    {
        //‘€ì‚ª‹£‡‚µ‚È‚¢‚½‚ß‚Ìİ’è
        if (photonView.IsMine)
        {
            // “ü—Í‚ğx‚É‘ã“ü
            float x = Input.GetAxis("Horizontal");

            //Rigidbody2D‚ğæ“¾
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            //x²‚É‰Á‚í‚é—Í‚ğŠi”[
            Vector2 force = new Vector2(x * 10, 0);

            //Rigidbody2D‚É—Í‚ğ‰Á‚¦‚é
            rb.AddForce(force);
        }
           
    }
}
