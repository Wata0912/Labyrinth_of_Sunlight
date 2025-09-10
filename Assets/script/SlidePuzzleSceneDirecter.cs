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

    GameObject freePiece;// �󔒃s�[�X�i�ړ��p�j

    // ���ۂɐ������ꂽ�s�[�X�̃��X�g
    List<GameObject> movablePiece = new List<GameObject>();
    public List<Tile> allTilesScript = new List<Tile>();
   
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(stratpieces, new Vector3(-1.5f, 2.5f, 0), Quaternion.identity);
        Instantiate(goalpieces, new Vector3(1.5f, -2.5f, 0), Quaternion.identity);
        playerObj = Instantiate(Player, new Vector3(-1.5f, 2.5f, 0), Quaternion.identity);
        playerScript = playerObj.GetComponent<Player>();
    
        stage_id = PlayerPrefs.GetInt("StageID", 1); // �f�t�H���g��0
        
        Debug.Log(stage_id);
             
        StartCoroutine(NetworkManager.Instance.GetCell(objects =>
        {
            if (objects != null)
            {
                foreach (var Cell in objects)
                {
                    CreatePiece(Cell.object_id, new Vector3(Cell.x, Cell.y, 0), Quaternion.identity);
                }             
                // Player�Ƀ^�C����n���̂����̃^�C�~���O�����S
                playerScript.SetTiles(allTilesScript);

                // �f�[�^�������Ă���V���b�t���J�n
                ShufflePanels();
            }
            
        },stage_id));
        
        ClearPanel.SetActive(false);
        retryButton.SetActive(false);
        PlayerPrefs.DeleteKey("StageID");
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[����~���̏ꍇ�̂݃X���C�h�\
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
        
        // ESC�L�[�Ń|�[�Y�؂�ւ�
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

    // �w��s�[�X�Ƌ󔒃s�[�X���אڂ��Ă���ΕԂ�
    GameObject GetEmptyPiece(GameObject piece)
    {
        float dist = Vector2.Distance(piece.transform.position, freePiece.transform.position);

        if (dist == 1)
        {
            return freePiece;
        }
        return null;
    }

    // 2�̃s�[�X�����ւ���  
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

    // �s�[�X�𐶐����A���X�g�ɒǉ�
    public void CreatePiece(int index, Vector3 position, Quaternion rotation)
    {
        index -= 1;//id��z��ł�����悤��
        Debug.Log($"CreatePiece �Ăяo�� index={index}, pos={position}");
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

    // �V���b�t�������i�w��񐔃����_���ɓ���ւ���j
    void ShufflePanels()
    { 
        for (int i = 0; i < shuffCount; i++)
        {
            int rnd = UnityEngine.Random.Range(0, movablePiece.Count);
            GameObject piece = movablePiece[rnd];
            SwapPiece(piece, movablePiece[0]);
        }
    }

    // �u�X�^�[�g�v�{�^���ňړ��J�n
    public void moveStart()
    {
        playerScript.isMoving = true;
        moveStartButton.SetActive(false);
    }

    // �i���֎~���ʊO�ȂǂŃ~�X�����Ƃ��ɌĂ΂��
    public void miss()
    {          
        retryButton.SetActive(true);
    }

    // �S�[���ɓ��B�����Ƃ��̏���
    public void GoalPunel()
    {
               
        ClearPanel.SetActive(true);
        //StartCoroutine(NetworkManager.Instance.LevelUP());      
        StartCoroutine(NetworkManager.Instance.GetItem(stage_id));
        PlayerPrefs.SetInt("ClearstageID", stage_id +1);
    }

    public void SelectStageScene()
    {
        SceneManager.LoadScene("SelectStageScene");
    }
    public void ReLoadScene()
    {
        StartCoroutine(NetworkManager.Instance.GetStage(stages =>
        {
            if (stages != null)
            {
                if(stages.Length == stage_id)
                {
                    SceneManager.LoadScene("SelectStageScene");
                }
                else
                {
                    PlayerPrefs.SetInt("StageID", stage_id + 1);

                    SceneManager.LoadScene("SlidePuzzleScene");
                }
            }

        }));
       
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // ���Ԃ�ʏ�ɖ߂�
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // �Q�[�������Ԃ��~�߂�
        isPaused = true;
    }
}
