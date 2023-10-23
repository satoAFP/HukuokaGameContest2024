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
    //�X�e�B�b�N
    public Sprite StickR;



    // Start is called before the first frame update
    void Start()
    {
        //�}�l�[�W���[�A�N�Z�b�T�ɓo�^
        ManagerAccessor.Instance.spriteManager = this;
    }
}
