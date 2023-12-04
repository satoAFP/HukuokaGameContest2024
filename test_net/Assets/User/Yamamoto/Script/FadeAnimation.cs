using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FadeAnimation : MonoBehaviourPunCallbacks
{

    private Animator anim;//アニメーター

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        if (datamanager.isDeth)
        {
            anim.SetBool("FadeOut", true);//フェードアウトアニメーション開始
        }
    }

    public void EndFadeOutAnimation()
    {
        Debug.Log("EndFadeOutAnimation通ってるぞいいいい");
        anim.SetBool("FadeOut", false);//フェードアウトアニメーション終了
    }
}
