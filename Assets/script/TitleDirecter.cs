using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleDirecter : MonoBehaviour
{
 

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
