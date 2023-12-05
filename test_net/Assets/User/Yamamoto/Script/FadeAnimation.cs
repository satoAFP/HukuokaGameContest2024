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

        if (datamanager.isDeth && firstfadeout)
        {
            Debug.Log("フェードアウト一回");
            anim.SetBool("FadeOut", true);//フェードアウトアニメーション開始
            firstfadeout = false;
        }
    }

    public void EndFadeOutAnimation()
    {
        if(firstendfadeout)
        {
            Debug.Log("EndFadeOutAnimation通ってるぞいいいい");
            anim.SetBool("FadeOut", false);//フェードアウトアニメーション終了
            firstendfadeout = false;
            fadeoutfinish = true;
        }
       
    }
}
