using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FadeAnimation : MonoBehaviourPunCallbacks
{

    private Animator anim;//アニメーター

    private bool firstfadeout = true;//一度だけフェードアウトの処理を通す
    private bool firstendfadeout = true;//一度だけフェードアウトアニメーション終了の処理を通す

    public bool  fadeoutfinish = false;//フェードアウトが終了したとき

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //フェードアウト処理
        if(firstfadeout)
        {
            //死亡時のフェードアウト
            if (datamanager.isDeth)
            {
                //死亡時のノックバックが終了したらフェードアウト開始
                if(datamanager.player1.GetComponent<PlayerController>().knockback_finish
                || datamanager.player2.GetComponent<PlayerController>().knockback_finish)
                {
                    //Debug.Log("フェードアウト一回");
                    anim.SetBool("FadeOut", true);//フェードアウトアニメーション開始
                    firstfadeout = false;
                }
            }
        }

        
    }

    public void EndFadeOutAnimation()
    {
        if(firstendfadeout)
        {
            Debug.Log("EndFadeOutAnimation");
            anim.SetBool("FadeOut", false);//フェードアウトアニメーション終了
            firstendfadeout = false;
            fadeoutfinish = true;
        }
       
    }
}
