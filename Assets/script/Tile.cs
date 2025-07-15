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
        public Vector2 incomingDirection; // �v���C���[�������Ă������
        public Vector2 outgoingDirection; // �o�Ă�������
    }

    public List<DirectionMapping> directionMappings;

    /// <summary>
    /// ���������ɑΉ�����o�Ă���������Ԃ�
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

        return Vector2.zero; // �Ή��Ȃ�
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
