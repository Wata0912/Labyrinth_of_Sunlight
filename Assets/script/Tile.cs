using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 moveColliderSize = new Vector2(0.05f, 0.05f);//�ړ����̃^�C���̓����蔻��T�C�Y�𓹂̕��ɂ��邽�߂̂���
    BoxCollider2D boxCollider2D;
    public List<Vector2> forbiddenDirections;//�ʍs�s�̕���
    public List<DirectionMapping> directionMappings;//�N�������Əo�͕����̃��X�g

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    [System.Serializable]//unity���ŕ����ݒ�
    public class DirectionMapping
    {
        public Vector2 incomingDirection; // �v���C���[�������Ă������
        public Vector2 outgoingDirection; // �o�Ă�������
    }

    /// <summary>
    /// ���������ɑΉ�����o�Ă���������Ԃ�
    /// </summary>
    public Vector2 GetNewDirectionFrom(Vector2 incoming)
    {
       
        foreach (var mapping in directionMappings)
        {   
            //�v���C���[�̓����Ă����������ݒ肳�ꂽ���̂Ƌt�Ȃ�(�E�ɐi�s���Ă���ꍇ����������ė���)
            if (Vector2.Equals(mapping.incomingDirection.normalized, -incoming.normalized))
            {   //�o�͕����ɐi�s������ύX����
                return mapping.outgoingDirection.normalized;
            }
        }

        //
        return Vector2.zero; 
    }

    public void ChangeMoveCollider()
    {   //�ړ����ɓ����ɕύX
        boxCollider2D.size = moveColliderSize;
    }

    public void ResetCollider()
    {   //�O�ׂ̈������擾(unity���ŃG���[���o��)
        boxCollider2D = GetComponent<BoxCollider2D>();
        //�^�C������p�ɖ߂�
        boxCollider2D.size = new Vector2(1,1);
    }

    //�N���֎~����
    public bool IsBlockedFromDirection(Vector2 incomingDirection)
    {   
        foreach (var dir in forbiddenDirections)
        {   //�֎~�������X�g���N�������ƈ�v�������~
            if (Vector2.Equals(dir.normalized, incomingDirection.normalized))
            {
                return true;
            }
        }
        return false;
    }

}
