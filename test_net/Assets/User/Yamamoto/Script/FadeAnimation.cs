using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FadeAnimation : MonoBehaviourPunCallbacks
{

    private Animator anim;//�A�j���[�^�[

    private bool firstfadeout = true;//��x�����t�F�[�h�A�E�g�̏�����ʂ�
    private bool firstendfadeout = true;//��x�����t�F�[�h�A�E�g�A�j���[�V�����I���̏�����ʂ�

    public bool  fadeoutfinish = false;//�t�F�[�h�A�E�g���I�������Ƃ�

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DataManager datamanager = ManagerAccessor.Instance.dataManager;

        //�t�F�[�h�A�E�g����
        if(firstfadeout)
        {
            //���S���̃t�F�[�h�A�E�g
            if (datamanager.isDeth)
            {
                if (datamanager.player1 != null && datamanager.player2 != null)
                {
                    //���S���̃m�b�N�o�b�N���I��������t�F�[�h�A�E�g�J�n(��ɓ����������j
                    if (datamanager.player1.GetComponent<PlayerController>().knockback_finish
                    || datamanager.player2.GetComponent<PlayerController>().knockback_finish)
                    {
                        anim.SetBool("FadeOut", true);//�t�F�[�h�A�E�g�A�j���[�V�����J�n
                        firstfadeout = false;
                    }
                    //�������̃t�F�[�h����
                    else if (datamanager.player1.GetComponent<PlayerController>().falling_death
                    || datamanager.player2.GetComponent<PlayerController>().falling_death)
                    {
                        anim.SetBool("FadeOut", true);//�t�F�[�h�A�E�g�A�j���[�V�����J�n
                        firstfadeout = false;
                    }
                }
                //���Ύ��̃t�F�[�h����
                if (datamanager.isFlyStart)
                {
                    anim.SetBool("FadeOut", true);//�t�F�[�h�A�E�g�A�j���[�V�����J�n
                    firstfadeout = false;
                    Debug.Log("aaa");
                }
                Debug.Log("bbb");
            }
        }

        
    }

    public void EndFadeOutAnimation()
    {
        if(firstendfadeout)
        {
            Debug.Log("EndFadeOutAnimation");
            anim.SetBool("FadeOut", false);//�t�F�[�h�A�E�g�A�j���[�V�����I��
            firstendfadeout = false;
            fadeoutfinish = true; 
        }
       
    }
}
