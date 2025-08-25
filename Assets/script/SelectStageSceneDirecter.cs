using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectStageSceneDirecter : MonoBehaviour
{
    SlidePuzzleSceneDirecter slide;
    [SerializeField]List<GameObject> stageSelectButton;
    [SerializeField] Text userText;
    [SerializeField] Text levelText;
    int clearStageID;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NetworkManager.Instance.ShowUser(user =>
        {
            if (user != null)
            {
                userText.text = user.Name;
                levelText.text = user.Level.ToString();
                Debug.Log($"Name:{user.Name}, Level:{user.Level}");
            }
        }));



        for (int i = 0; i < 4 + 1; i++)
        {
            stageSelectButton[i].SetActive(false);
        }

        StartCoroutine(NetworkManager.Instance.GetStage(stages =>
        {
            if (stages != null)
            {
                foreach (var stage in stages)
                {
                    Debug.Log($"Stage ID: {stage.id}, Name: {stage.name}");
                    stageSelectButton[stage.id].SetActive(true);
                }

            }
        }));
      

        
            clearStageID = PlayerPrefs.GetInt("ClearstageID", 0); // デフォルトは0
        

        
        PlayerPrefs.DeleteKey("ClearstageID");
        
        if(clearStageID > stageSelectButton.Count || clearStageID < 0)
        {
            clearStageID = 0;
        }
        

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public  void PlayStart(int stageNum)
    {

        PlayerPrefs.SetInt("StageID", stageNum);
        SceneManager.LoadScene("SlidePuzzleScene");
        
    }
    
    public void Exit()
    {
        SceneManager.LoadScene("TitleScene");
    }

}
