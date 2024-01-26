using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class EndingSystem : MonoBehaviourPunCallbacks
{
    private bool isAniEnd = false;  //アニメーション終了判定

    private bool first = true;

    [SerializeField, Header("決定SE")] AudioClip enterSE;

    AudioSource audioSource;

    private void Start()
    {
        //出すものを変える
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);

        //出すものを変える
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "タイトルへ";
        }
        else
        {
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "1P操作待ち";
        }

        audioSource = GetComponent<AudioSource>();
    }

    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        if (isAniEnd)
        {
            if (first)
            {
                //シーン名取得
                GlobalSceneName.SceneName = ManagerAccessor.Instance.sceneMoveManager.GetSceneName();
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("NewTitle");

                audioSource.PlayOneShot(enterSE);

                first = false;
            }
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

    }

    public void OnAnimationEnd()
    {
        //出すものを変える
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        //アニメーション終了時の処理
        isAniEnd = true;
    }
}
