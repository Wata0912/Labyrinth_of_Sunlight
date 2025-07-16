using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;//�v���C���[�̈ړ����x
    private Vector2 moveDirection = Vector2.zero;//���݂̈ړ�����
    public bool isMoving = false;//�v���C���[���ړ������ǂ���

    SlidePuzzleSceneDirecter sceneDirecter;//�V�[���ړ��p
    Tile Tile;//�ʏ�^�C��
    StartTile startTile;//�X�^�[�g�^�C��
    GoalTile goalTile;//�S�[���^�C��

    private Rigidbody2D rb;//�v���C���[��RB
    private List<Tile> tiles = new List<Tile>();//�S�Ẵ^�C���̃����X�g
    Collider2D collider2d;//������ʒu�̃R���C�_�[
    Collider2D collider;//������ʒu�̃R���C�_�[

    void Start()
    {
        //�e�v�f�̎擾
        rb = GetComponent<Rigidbody2D>();
        Tile =  FindObjectOfType<Tile>();
        startTile = FindObjectOfType<StartTile>();
        collider2d = Physics2D.OverlapPoint(transform.position);
        collider = GetComponent<BoxCollider2D>();
        goalTile = FindObjectOfType<GoalTile>();
        sceneDirecter = FindObjectOfType<SlidePuzzleSceneDirecter>();
        moveDirection = new Vector2(0, -1);
    }

    void Update()
    {
        if (isMoving)
        {
            //�ړ��������R���C�_�[�L����(�p�l���������Ȃ��悤�ɂ���)
            startTile.EnableCollider();
            goalTile.EnableCollider();
            collider.enabled = true;
            rb.isKinematic = true;
            if (collider2d != null)
            {
                //�����T�C�Y�ɕύX
                foreach (var tile in tiles)
                {
                    tile.ChangeMoveCollider();
                }
            }

            CheckOutOfScreen();//��ʊO�`�F�b�N

        }
        else
        {
            startTile.FalseCollider();
            goalTile.FalseCollider();
            collider.enabled = false;
            rb.isKinematic = false;
            foreach (var tile in tiles)
            {   //�R���C�_�[�����Ƃɖ߂�
                tile.ResetCollider();
            }
        }
        
    }

    void FixedUpdate()
    {
        if (isMoving)
        {   //�ړ�����
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   

        //�G�ꂽ�^�C�����������擾(�G�ꂽ�^�C����script���Ăяo������)
        Tile turnTile = other.GetComponent<Tile>();

        //�ʒu���X�^�[�g�^�C���̎�
        if (other.CompareTag("StartTile"))
        {   
            if (startTile != null && other.GetComponent<StartTile>() == startTile)
            {
                moveDirection = startTile.startDirection.normalized;

            }
            startTile = other.GetComponent<StartTile>();
        }

        // �Ȃ���p�^�C���ɐG�ꂽ�Ƃ��̏���
        if (other.CompareTag("TurnTile") && isMoving)
        {           
            if (turnTile != null)
            {
                // �֎~����Ă���������痈���ꍇ�A�ړ���~
                if (turnTile.IsBlockedFromDirection(-moveDirection))
                {
                    isMoving = false;
                    rb.velocity = Vector2.zero; // �O�̂���
                    Debug.Log("�i���֎~��������i���������ߒ�~");
                    moveDirection = new Vector2(0, -1);
                    sceneDirecter.miss();
                    return;
                }

                // �ʉ߉\�ȕ����Ȃ�A�����]������
                Vector2 newDir = turnTile.GetNewDirectionFrom(moveDirection);
                if (newDir != Vector2.zero)
                {
                    moveDirection = newDir;
                }
            }
        }

        // �S�[���^�C���ɐG�ꂽ�Ƃ��̏���
        if (other.CompareTag("GoalTile"))
        {          
            isMoving = false;
            rb.velocity = Vector2.zero; // �O�̂���
            Debug.Log("�S�[��");
            sceneDirecter.GoalPunel();
        }

    }

   //�^�C���̏��𐶐����Ɏ擾����ׂ̃Z�b�^�[
    public void SetTiles(List<Tile> tileList)
    {
        tiles = tileList;
    }

    void CheckOutOfScreen()
    {   
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        //��ʊO����
        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1 || this.transform.position.y > 3 || this.transform.position.y < -3)
        {
            Debug.Log("�v���C���[����ʊO�ɏo�܂����I");
            isMoving = false;
            rb.velocity = Vector2.zero;
            moveDirection = new Vector2(0, -1);
            sceneDirecter.miss();
        }
    }
}
