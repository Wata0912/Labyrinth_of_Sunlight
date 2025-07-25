using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : MonoBehaviour
{
    public Vector2 startDirection = Vector2.right; // 最初に進む方向
    private Vector2 moveColliderSize = new Vector2(0.05f, 0.05f);
    BoxCollider2D boxCollider2D;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        // 無効化
        boxCollider2D.enabled = false;
    }

    public void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }

    public void FalseCollider()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }

}
