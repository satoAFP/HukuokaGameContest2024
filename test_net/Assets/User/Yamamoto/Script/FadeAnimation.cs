using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FadeAnimation : MonoBehaviourPunCallbacks
{

    private Animator anim;//�A�j���[�^�[

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
            anim.SetBool("FadeOut", true);//�t�F�[�h�A�E�g�A�j���[�V�����J�n
        }
    }

    public void EndFadeOutAnimation()
    {
        Debug.Log("EndFadeOutAnimation�ʂ��Ă邼��������");
        anim.SetBool("FadeOut", false);//�t�F�[�h�A�E�g�A�j���[�V�����I��
    }
}
