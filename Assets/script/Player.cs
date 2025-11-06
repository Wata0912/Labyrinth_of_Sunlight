using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;//プレイヤーの移動速度
    private Vector2 moveDirection = Vector2.zero;//現在の移動方向
    public bool isMoving = false;//プレイヤーが移動中かどうか

    SlidePuzzleSceneDirecter sceneDirecter;//シーン移動用
    Tile Tile;//通常タイル
    StartTile startTile;//スタートタイル
    GoalTile goalTile;//ゴールタイル

    private Rigidbody2D rb;//プレイヤーのRB
    private List<Tile> tiles = new List<Tile>();//全てのタイルのリリスト
    Collider2D collider2d;//今いる位置のコライダー
    

    void Start()
    {
        //各要素の取得
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
            //移動中だけタイルコライダー有効化(パネルが動かないようにする)
            startTile.EnableCollider();
            goalTile.EnableCollider();
            collider2d.enabled = true;
            rb.isKinematic = true;
            if (collider2d != null)
            {
                //道幅サイズに変更
                foreach (var tile in tiles)
                {
                    tile.ChangeMoveCollider();
                }
            }

            CheckOutOfScreen();//画面外チェック

        }
        else
        {

            startTile.FalseCollider();
            goalTile.FalseCollider();
            collider2d.enabled = false;
            rb.isKinematic = false;
            foreach (var tile in tiles)
            {   //コライダーをもとに戻す
                tile.ResetCollider();
            }
        }
        
    }

    void FixedUpdate()
    {
        if (isMoving)
        {   //移動処理
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   

        //触れたタイルが持つ情報を取得(触れたタイルのscriptを呼び出すため)
        Tile turnTile = other.GetComponent<Tile>();

        //位置がスタートタイルの時
        if (other.CompareTag("StartTile"))
        {   
            if (startTile != null && other.GetComponent<StartTile>() == startTile)
            {
                moveDirection = startTile.startDirection.normalized;

            }
            startTile = other.GetComponent<StartTile>();
        }

        // 曲がり角タイルに触れたときの処理
        if (other.CompareTag("TurnTile") && isMoving)
        {           
            if (turnTile != null)
            {
                // 禁止されている方向から来た場合、移動停止
                if (turnTile.IsBlockedFromDirection(-moveDirection))
                {
                    isMoving = false;
                    rb.velocity = Vector2.zero; // 念のため
                    Debug.Log("進入禁止方向から進入したため停止");
                    moveDirection = new Vector2(0, -1);
                    sceneDirecter.miss();
                    return;
                }

                // 通過可能な方向なら、方向転換する
                Vector2 newDir = turnTile.GetNewDirectionFrom(moveDirection);
                if (newDir != Vector2.zero)
                {
                    moveDirection = newDir;
                }
            }
        }

        // ゴールタイルに触れたときの処理
        if (other.CompareTag("GoalTile"))
        {          
            isMoving = false;
            rb.velocity = Vector2.zero; // 念のため
            Debug.Log("ゴール");
            sceneDirecter.GoalPunel();
        }

    }

   //タイルの情報を生成時に取得する為のセッター
    public void SetTiles(List<Tile> tileList)
    {
        tiles = tileList;
    }

    void CheckOutOfScreen()
    {   
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        //画面外判定
        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1 || this.transform.position.y > 3 || this.transform.position.y < -3)
        {
            Debug.Log("プレイヤーが画面外に出ました！");
            isMoving = false;
            rb.velocity = Vector2.zero;
            moveDirection = new Vector2(0, -1);
            sceneDirecter.miss();
        }
    }
}
