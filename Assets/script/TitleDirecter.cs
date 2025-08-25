using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleDirecter : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameScene()
    {
       
        bool isSuccess = NetworkManager.Instance.LoadUserData();
        if (isSuccess)
        {
            SceneManager.LoadScene("SelectStageScene");
        }
        else
        {
            SceneManager.LoadScene("CreateUserScene");

        }
    }
}
