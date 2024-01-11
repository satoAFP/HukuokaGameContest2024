using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprashSystem : MonoBehaviour
{
    [SerializeField, Header("スプラッシュ画面")]
    private GameObject splashMenu;

    [SerializeField, Header("BGM再生用オブジェクト")]
    private GameObject BGMObj;

    [SerializeField, Header("宝箱が開く音")]
    private AudioClip OpeneSE;

    [SerializeField, Header("新しい場合")]
    private bool NewSystem = true;

    private AudioSource audioSource;

    [System.NonSerialized]
    public bool isInputB = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NewSystem)
            if (isInputB)
                splashMenu.SetActive(false);
    }

    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        isInputB = true;
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        isInputB = false;
    }

    public void PlaySE()
    {
        //SE再生
        audioSource.PlayOneShot(OpeneSE);
    }

    public void EndAnimation()
    {
        //BGM再生
        BGMObj.SetActive(true);
    }
}
