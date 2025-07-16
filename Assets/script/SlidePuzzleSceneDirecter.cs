using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SlidePuzzleSceneDirecter : MonoBehaviour
{
    [SerializeField] List<GameObject> pieces;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject moveStartButton;
    [SerializeField] GameObject stratpieces;
    [SerializeField] GameObject goalpieces;
    [SerializeField] int shuffCount;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject ClearPanel;
    GameObject playerObj;
    public bool isStart = false;
    private Player playerScript;
    
    List<Vector2> startPositions;

    //ステージの座標
    Vector3[] stagePos = {new Vector3(-1.5f,1.5f,0), new Vector3(-0.5f, 1.5f, 0), new Vector3(0.5f, 1.5f, 0), new Vector3(1.5f, 1.5f, 0),
    new Vector3(-1.5f,0.5f,0), new Vector3(-0.5f, 0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(1.5f, 0.5f, 0),
    new Vector3(-1.5f,-0.5f,0), new Vector3(-0.5f, -0.5f, 0), new Vector3(0.5f, -0.5f, 0), new Vector3(1.5f, -0.5f, 0),
    new Vector3(-1.5f,-1.5f,0), new Vector3(-0.5f, -1.5f, 0), new Vector3(0.5f, -1.5f, 0), new Vector3(1.5f, -1.5f, 0),};

    // ステージ上の各座標位置（4x4グリッド）
    //ステージ１配置(仮)
    int[] piecesNum = {5,0,2,7,
                       1,3,5,7,
                        7,7,1,2,
                        7,7,6,5 };

    GameObject freePiece;// 空白ピース（移動用）

    // 実際に生成されたピースのリスト
    List<GameObject> movablePiece = new List<GameObject>();
    public List<Tile> allTilesScript = new List<Tile>();
   
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(stratpieces, new Vector3(-1.5f, 2.5f, 0), Quaternion.identity);
        Instantiate(goalpieces, new Vector3(1.5f, -2.5f, 0), Quaternion.identity);
        playerObj = Instantiate(Player, new Vector3(-1.5f, 2.5f, 0), Quaternion.identity);
        playerScript = playerObj.GetComponent<Player>();


        //ステージ生成
        startPositions = new List<Vector2>();
        for (int i = 0; i < stagePos.Length ; i++)
        {
            CreatePiece(piecesNum[i], stagePos[i], Quaternion.identity);
        }

        playerScript.SetTiles(allTilesScript);
        ShufflePanels();

        ClearPanel.SetActive(false);
        retryButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが停止中の場合のみスライド可能
        if (playerScript.isMoving == false)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit2D)
                {
                    GameObject hitPiece = hit2D.collider.gameObject;

                    GameObject emptyPiece = GetEmptyPiece(hitPiece);

                    SwapPiece(hitPiece, emptyPiece);

                }
            }
        }               
    }

    // 指定ピースと空白ピースが隣接していれば返す
    GameObject GetEmptyPiece(GameObject piece)
    {
        float dist = Vector2.Distance(piece.transform.position, freePiece.transform.position);

        if (dist == 1)
        {
            return freePiece;
        }

        return null;
    }

    // 2つのピースを入れ替える
    void SwapPiece(GameObject pieceA, GameObject pieceB)
    {
        if (pieceA == null || pieceB == null )
        {
            return;
        }

        Vector2 position = pieceA.transform.position;
        pieceA.transform.position = pieceB.transform.position;
        pieceB.transform.position = position;
    }

    public void OnClickRetry()
    {
        
        retryButton.SetActive(false);
        playerObj.transform.position = new Vector3(-1.5f, 2.5f, 0);

        moveStartButton.SetActive(true);

    }

    // ピースを生成し、リストに追加
    public void CreatePiece(int index, Vector3 position, Quaternion rotation)
    {
        GameObject piece;
        if (index >= 0 && index < pieces.Count)
        {
            piece = Instantiate(pieces[index], position, rotation);
            movablePiece.Add(piece);
            startPositions.Add(piece.transform.position);
            Tile tileScript = piece.GetComponent<Tile>();
            allTilesScript.Add(tileScript);
            if(index == 6)
            {
                freePiece = piece;
            }
            
        }
        else
        {
            Debug.LogWarning("Index out of range!");
        }
    }

    // シャッフル処理（指定回数ランダムに入れ替える）
    void ShufflePanels()
    {
       for(int i = 0; i < shuffCount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, movablePiece.Count);
            GameObject piece = movablePiece[rnd];
            SwapPiece(piece, movablePiece[0]);
        }
    }

    // 「スタート」ボタンで移動開始
    public void moveStart()
    {
        playerScript.isMoving = true;
        moveStartButton.SetActive(false);
    }

    // 進入禁止や画面外などでミスしたときに呼ばれる
    public void miss()
    {          
        retryButton.SetActive(true);
    }

    // ゴールに到達したときの処理
    public void GoalPunel()
    {
        ClearPanel.SetActive(true);        
    }

    public void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
