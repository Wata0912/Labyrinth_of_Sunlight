using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] GameObject pauseMenuUI; 
    GameObject playerObj;
    public bool isStart = false;
    private Player playerScript;
    public int stage_id;
    public static bool isPaused = false;

    List<Vector2> startPositions;

    GameObject freePiece;// 空白ピース（移動用）

    // 実際に生成されたピースのリスト
    List<GameObject> movablePiece = new List<GameObject>();
    public List<Tile> allTilesScript = new List<Tile>();

    AudioSource audioSource;
    [SerializeField]  public AudioClip sound1;
    [SerializeField]  public AudioClip sound2;
    [SerializeField] public AudioClip sound3;
    bool click;
    bool Shuffled;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(stratpieces, new Vector3(-1.5f, 2.5f, 0), Quaternion.identity);
        Instantiate(goalpieces, new Vector3(1.5f, -2.5f, 0), Quaternion.identity);
        playerObj = Instantiate(Player, new Vector3(-1.5f, 2.5f, 0), Quaternion.identity);
        playerScript = playerObj.GetComponent<Player>();
    
        stage_id = PlayerPrefs.GetInt("StageID", 1); // デフォルトは0
        
        Debug.Log(stage_id);
             
        StartCoroutine(NetworkManager.Instance.GetCell(objects =>
        {
            if (objects != null)
            {
                Shuffled = false;
                foreach (var Cell in objects)
                {
                    CreatePiece(Cell.object_id, new Vector3(Cell.x, Cell.y, 0), Quaternion.identity);
                }             
                // Playerにタイルを渡すのもこのタイミングが安全
                playerScript.SetTiles(allTilesScript);

                // データが入ってからシャッフル開始
                ShufflePanels();
            }
            
        },stage_id));
        
        ClearPanel.SetActive(false);
        retryButton.SetActive(false);
        PlayerPrefs.DeleteKey("StageID");

        audioSource = GetComponent<AudioSource>();
        click = false;
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
        
        // ESCキーでポーズ切り替え
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
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
        if(Shuffled == true)
        {
            audioSource.PlayOneShot(sound2);
        }
        
    }

    public void OnClickRetry()
    {     
        retryButton.SetActive(false);
        playerObj.transform.position = new Vector3(-1.5f, 2.5f, 0);

        moveStartButton.SetActive(true);
        audioSource.PlayOneShot(sound3);
    }

    // ピースを生成し、リストに追加
    public void CreatePiece(int index, Vector3 position, Quaternion rotation)
    {
        index -= 1;//idを配列でつかえるように
        Debug.Log($"CreatePiece 呼び出し index={index}, pos={position}");
        GameObject piece;
        if (index >= 0 && index < pieces.Count)
        {
            piece = Instantiate(pieces[index], position, rotation);
            movablePiece.Add(piece);
            //startPositions.Add(piece.transform.position);
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
        for (int i = 0; i < shuffCount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, movablePiece.Count);
            GameObject piece = movablePiece[rnd];
            SwapPiece(piece, movablePiece[0]);
        }

        Shuffled = true;
    }

    // 「スタート」ボタンで移動開始
    public void moveStart()
    {
        playerScript.isMoving = true;
        moveStartButton.SetActive(false);
        audioSource.PlayOneShot(sound3);
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
        //StartCoroutine(NetworkManager.Instance.LevelUP());      
        StartCoroutine(NetworkManager.Instance.GetItem(stage_id));
        PlayerPrefs.SetInt("ClearstageID", stage_id +1);
    }

    public void SelectStageScene()
    {
        //SceneManager.LoadScene("SelectStageScene");
        Initiate.Fade("SelectStageScene", Color.black, 1.0f);
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }
    }
    public void ReLoadScene()
    {
        StartCoroutine(NetworkManager.Instance.GetStage(stages =>
        {
            if (stages != null)
            {
                if(stages.Length == stage_id)
                {
                    //SceneManager.LoadScene("SelectStageScene");
                    Initiate.Fade("SelectStageScene", Color.black, 1.0f);
                }
                else
                {
                    PlayerPrefs.SetInt("StageID", stage_id + 1);

                    //SceneManager.LoadScene("SlidePuzzleScene");
                    Initiate.Fade("SlidePuzzleScene", Color.black, 1.0f);
                }
                if (click == false)
                {
                    audioSource.PlayOneShot(sound1);
                    click = true;
                }
            }

        }));
       
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // 時間を通常に戻す
        isPaused = false;
        audioSource.PlayOneShot(sound1);
    }

    public void Pause()
    {
        audioSource.PlayOneShot(sound1);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // ゲーム内時間を止める
        isPaused = true;
        
    }

    public void Retire()
    {
        // まず時間を戻す
        Time.timeScale = 1f;
        isPaused = false;

        // それからUIを閉じる
        pauseMenuUI.SetActive(false);

        Debug.Log("リタイア");
        SceneManager.LoadScene("SelectStageScene", LoadSceneMode.Single);
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }
    }

   
}
