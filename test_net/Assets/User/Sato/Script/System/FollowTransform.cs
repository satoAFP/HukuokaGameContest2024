using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FollowTransform : MonoBehaviourPunCallbacks
{
    private Transform target; // �Ǐ]����Ώ�
    [SerializeField] private Vector3 offset; // �I�t�Z�b�g�iWorld Space�̃I�t�Z�b�g�j
    private RectTransform rectTransform;

    public void SetTarget(Transform target, Vector3 offset)
    {
        this.target = target;
        this.offset = offset;
        rectTransform = GetComponent<RectTransform>();
        RefreshPosition();
    }
    public void SetTarget(Transform target)
    {
        SetTarget(target, Vector3.zero);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (ManagerAccessor.Instance.dataManager.player1)
            {
                target = ManagerAccessor.Instance.dataManager.player1.transform;
                RefreshPosition();
            }
        }
    }

    private void RefreshPosition()
    {
        if (target)
        {
            // World Position��Screen Position�ɕϊ�
            Vector2 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
            rectTransform.position = screenPos;
        }
    }
}
