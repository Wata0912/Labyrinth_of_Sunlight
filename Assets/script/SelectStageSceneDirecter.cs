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
    Text buttonText;
    int clearStageID;
    int userLevel;
    AudioSource audioSource;
    public AudioClip sound1;
    bool click;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NetworkManager.Instance.ShowUser(user =>
        {
            if (user != null)
            {
                userText.text = user.Name;
                levelText.text = user.Level.ToString();
                userLevel = user.Level;
                Debug.Log($"Name:{user.Name}, Level:{user.Level}");

                StartCoroutine(NetworkManager.Instance.GetStage(stages =>
                {
                    if (stages != null)
                    {
                        foreach (var stage in stages)
                        {
                            if (stage.id <= userLevel)
                            {
                                Debug.Log($"Stage ID: {stage.id}, Name: {stage.name}");
                                stageSelectButton[stage.id - 1].SetActive(true);
                                buttonText = stageSelectButton[stage.id - 1].GetComponentInChildren<Text>();
                                buttonText.text = stage.name;
                            }

                        }

                    }
                }));
            }
        }));

        for (int i = 0; i < stageSelectButton.Count; i++)
        {
            stageSelectButton[i].SetActive(false);
        }

        //StartCoroutine(NetworkManager.Instance.GetStage(stages =>
        //{
        //    if (stages != null)
        //    {
        //        foreach (var stage in stages)
        //        {
        //                if(stage.id <= userLevel)
        //            {
        //                Debug.Log($"Stage ID: {stage.id}, Name: {stage.name}");
        //                stageSelectButton[stage.id - 1].SetActive(true);
        //                buttonText = stageSelectButton[stage.id - 1].GetComponentInChildren<Text>();
        //                buttonText.text = stage.name;
        //            }
                       
        //        }

        //    }
        //}));
             
            clearStageID = PlayerPrefs.GetInt("ClearstageID", 0); // デフォルトは0
              
        PlayerPrefs.DeleteKey("ClearstageID");
        
        if(clearStageID > stageSelectButton.Count || clearStageID < 0)
        {
            clearStageID = 0;
        }

        audioSource = GetComponent<AudioSource>();
        click = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void PlayStart(int stageNum)
    {
       
            PlayerPrefs.SetInt("StageID", stageNum);
            //SceneManager.LoadScene("SlidePuzzleScene");
            Initiate.Fade("SlidePuzzleScene", Color.black, 1.0f);
          
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }


    }
    
    public void Exit()
    {
              
            //SceneManager.LoadScene("TitleScene");
            Initiate.Fade("TitleScene", Color.black, 1.0f);
            
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }

    }

    public void ItemScene()
    {
        
            //SceneManager.LoadScene("ItemScene");
            Initiate.Fade("ItemScene", Color.black, 1.0f);
            
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }


    }
}
