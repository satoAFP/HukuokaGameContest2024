using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Header("�R���g���[���[���͉摜")]
    //�{�^��
    public Sprite ArrowRight;
    public Sprite ArrowLeft;
    public Sprite ArrowUp;
    public Sprite ArrowDown;
    //�\���L�[
    public Sprite CrossRight;
    public Sprite CrossLeft;
    public Sprite CrossUp;
    public Sprite CrossDown;
    //��{�^��
    public Sprite R1;
    public Sprite R2;
    public Sprite L1;
    public Sprite L2;
    //R�X�e�B�b�N
    public Sprite RStick;
    public Sprite RStickRight;
    public Sprite RStickLeft;
    public Sprite RStickUp;
    public Sprite RStickDown;
    //L�X�e�B�b�N
    public Sprite LStick;
    public Sprite LStickPlus;
    public Sprite LStickRight;
    public Sprite LStickLeft;
    public Sprite LStickUp;
    public Sprite LStickDown;

    //�A�j���[�V����
    public RuntimeAnimatorController RStickRotateR;
    public RuntimeAnimatorController RStickRotateL;



    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        ManagerAccessor.Instance.spriteManager = this;
    }
}
