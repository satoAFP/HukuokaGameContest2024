using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprashSystem : MonoBehaviour
{
    [SerializeField, Header("スプラッシュ画面")]
    private GameObject splashMenu;

    private bool isInputB = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInputB)
            splashMenu.SetActive(false);
    }

    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        isInputB = true;
        Debug.Log("aaa");
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        isInputB = false;
        Debug.Log("bbb");
    }
}
