using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : MonoBehaviour
{
    public Vector2 startDirection = Vector2.right; // �ŏ��ɐi�ޕ���
    private Vector2 moveColliderSize = new Vector2(0.05f, 0.05f);
    BoxCollider2D boxCollider2D;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        // ������
        boxCollider2D.enabled = false;
    }

    public void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }

}
