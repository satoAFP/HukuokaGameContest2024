using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackGroundScroll : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�w�i�I�u�W�F�N�g")] private GameObject BackGround;

    [SerializeField, Header("�摜�T�C�Y")] private Vector2 Size;

    private GameObject[] imgObj = new GameObject[9];    //�w�i�摜�i�[�p
    private Vector3 player = Vector3.zero;              //���̉�ʂł̑���L�����擾�p

    [SerializeField] private Vector2 mapPos = Vector2.zero;//�w�i�摜�P�ʂł̎�l���̍��W

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //�v���C���[�����݂��Ă���Ƃ�
        if (ManagerAccessor.Instance.dataManager.player1 != null &&
            ManagerAccessor.Instance.dataManager.player2 != null)
        {
            //Locket�ɏ���Ă��Ȃ���
            if (!ManagerAccessor.Instance.dataManager.isFlyStart)
            {
                //���̉�ʂł̑���L�����擾
                if (PhotonNetwork.IsMasterClient)
                {
                    player = ManagerAccessor.Instance.dataManager.player1.transform.position;
                }
                else
                {
                    player = ManagerAccessor.Instance.dataManager.player2.transform.position;
                }
            }
            else
            {
                player = ManagerAccessor.Instance.dataManager.flyPos;
            }

            if (first)
            {
                //�w�i����
                for (int i = 0; i < 9; i++)
                    imgObj[i] = Instantiate(BackGround);

                //�w�i�̏������W�ݒ�
                for (int i = 0; i < 9; i += 3)
                {
                    imgObj[i].transform.position = new Vector3(-9, Size.y / 2 - (Size.y * i / 3), 0);
                    imgObj[i + 1].transform.position = new Vector3(9, Size.y / 2 - (Size.y * i / 3), 0);
                    imgObj[i + 2].transform.position = new Vector3(27, Size.y / 2 - (Size.y * i / 3), 0);
                }

                first = false;
            }

            //�w�i�摜�P�ʂł̎�l���̍��W�ݒ�
            if (player.x >= 0)
            {
                mapPos.x = Mathf.Floor(player.x / Size.x);
            }
            else
            {
                if (mapPos.x == 0)
                    mapPos.x = -1;

                mapPos.x = Mathf.Ceil(player.x / Size.x) - 1;
            }

            if (player.y >= 0)
            {
                mapPos.y = Mathf.Floor(player.y / Size.y);
            }
            else
            {
                if (mapPos.y == 0)
                    mapPos.y = -1;

                mapPos.y = Mathf.Ceil(player.y / Size.y) - 1;
            }

            //����L�����̍��W�ɂ���Ĕw�i�摜�̍��W�X�V
            for (int i = 0; i < 9; i += 3)
            {
                imgObj[i].transform.position = new Vector3(-(Size.x / 2) + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
                imgObj[i + 1].transform.position = new Vector3((Size.x / 2) + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
                imgObj[i + 2].transform.position = new Vector3((Size.x / 2) + Size.x + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
            }
        }
    }
}
