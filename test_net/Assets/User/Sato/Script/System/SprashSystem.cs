using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SprashSystem : MonoBehaviour
{
    [SerializeField, Header("�X�v���b�V�����")]
    private GameObject splashMenu;

    [SerializeField, Header("BGM�Đ��p�I�u�W�F�N�g")]
    private GameObject BGMObj;

    [SerializeField, Header("�󔠂��J����")]
    private AudioClip OpeneSE;

    [SerializeField, Header("�V�����ꍇ")]
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

    public void PlaySE()
    {
        //SE�Đ�
        audioSource.PlayOneShot(OpeneSE);
    }

    public void EndAnimation()
    {
        //BGM�Đ�
        BGMObj.SetActive(true);
    }
}
