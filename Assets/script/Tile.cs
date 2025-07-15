using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 moveColliderSize = new Vector2(0.05f, 0.05f);
    BoxCollider2D boxCollider2D;
    private Player player;
    public List<Vector2> forbiddenDirections;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        
    }

    [System.Serializable]
    public class DirectionMapping
    {
        public Vector2 incomingDirection; // プレイヤーが入ってくる方向
        public Vector2 outgoingDirection; // 出ていく方向
    }

    public List<DirectionMapping> directionMappings;

    /// <summary>
    /// 来た方向に対応する出ていく方向を返す
    /// </summary>
    public Vector2 GetNewDirectionFrom(Vector2 incoming)
    {
       

        foreach (var mapping in directionMappings)
        {
            if (Vector2.Equals(mapping.incomingDirection.normalized, -incoming.normalized))
            {
                return mapping.outgoingDirection.normalized;
            }
        }

        return Vector2.zero; // 対応なし
    }

    public void ChangeMoveCollider()
    {
        boxCollider2D.size = moveColliderSize;
    }

    public void ResetCollider()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(0.8f, 0.8f);
    }

    public bool IsBlockedFromDirection(Vector2 incomingDirection)
    {
        foreach (var dir in forbiddenDirections)
        {
            if (Vector2.Equals(dir.normalized, incomingDirection.normalized))
            {
                return true;
            }
        }
        return false;
    }

}
