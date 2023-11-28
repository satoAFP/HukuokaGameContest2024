using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprashSystem : MonoBehaviour
{
    [SerializeField, Header("�X�v���b�V�����")]
    private GameObject splashMenu;

    private bool isInputB = false;


    // Update is called once per frame
    void Update()
    {
        if (isInputB)
            splashMenu.SetActive(false);
    }

    //�R���g���[���[B����
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

        isInputB = true;
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

        isInputB = false;
    }
}
