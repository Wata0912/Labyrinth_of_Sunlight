using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveDirection = Vector2.zero;
    public bool isMoving = false;
    SlidePuzzleSceneDirecter sceneDirecter;
    Tile Tile;
    StartTile startTile;
    GoalTile goalTile;

    private Rigidbody2D rb;
    private StartTile currentStartTile = null;    
    private List<Tile> tiles = new List<Tile>();
    Collider2D collider2d;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Tile =  FindObjectOfType<Tile>();
        startTile = FindObjectOfType<StartTile>();
        collider2d = Physics2D.OverlapPoint(transform.position);
        goalTile = FindObjectOfType<GoalTile>();
        sceneDirecter = FindObjectOfType<SlidePuzzleSceneDirecter>();
        moveDirection = new Vector2(0, -1);
    }

    void Update()
    {
        if (isMoving)
        {
            startTile.EnableCollider();
            goalTile.EnableCollider();
            if(collider2d != null)
            {
                
                foreach (var tile in tiles)
                {
                    tile.ChangeMoveCollider();
                }
            }

            CheckOutOfScreen();


        }
        else
        {
            foreach (var tile in tiles)
            {
                tile.ResetCollider();
            }
        }


        if (collider2d != null && collider2d.CompareTag("StartTile"))
        {
            currentStartTile = collider2d.GetComponent<StartTile>();
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Tile turnTile = other.GetComponent<Tile>();

        if (other.CompareTag("StartTile"))
        {
            currentStartTile = other.GetComponent<StartTile>();
        }

        if (other.CompareTag("TurnTile") && isMoving)
        {           
            if (turnTile != null)
            {
                if (turnTile.IsBlockedFromDirection(-moveDirection))
                {
                    isMoving = false;
                    rb.velocity = Vector2.zero; // îOÇÃÇΩÇﬂ
                    Debug.Log("êiì¸ã÷é~ï˚å¸Ç©ÇÁêiì¸ÇµÇΩÇΩÇﬂí‚é~");
                    moveDirection = new Vector2(0, -1);
                    sceneDirecter.miss();
                    return;
                }

                Vector2 newDir = turnTile.GetNewDirectionFrom(moveDirection);
                if (newDir != Vector2.zero)
                {
                    moveDirection = newDir;
                }
            }
        }

        if (other.CompareTag("GoalTile"))
        {          
            isMoving = false;
            rb.velocity = Vector2.zero; // îOÇÃÇΩÇﬂ
            Debug.Log("ÉSÅ[Éã");
            sceneDirecter.GoalPunel();
        }



    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StartTile"))
        {
            if (currentStartTile != null && other.GetComponent<StartTile>() == currentStartTile)
            {
                moveDirection = currentStartTile.startDirection.normalized;
               
                
            }
        }
    }

   
    public void SetTiles(List<Tile> tileList)
    {
        tiles = tileList;
    }

    void CheckOutOfScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1 || this.transform.position.y > 3 || this.transform.position.y < -3)
        {
            Debug.Log("ÉvÉåÉCÉÑÅ[Ç™âÊñ äOÇ…èoÇ‹ÇµÇΩÅI");
            isMoving = false;
            rb.velocity = Vector2.zero;
            moveDirection = new Vector2(0, -1);
            sceneDirecter.miss();
        }
    }
}
