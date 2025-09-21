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
            //SceneManager.LoadScene("SelectStageScene");
            Initiate.Fade("SelectStageScene", Color.black, 1.0f);

        }
        else
        {
            //SceneManager.LoadScene("CreateUserScene");
            Initiate.Fade("CreateUserScene", Color.black, 1.0f);

        }
    }
}
