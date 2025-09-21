using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleDirecter : MonoBehaviour
{
  
    AudioSource audioSource;
    public AudioClip sound1;
    bool click;
    
    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource = GetComponent<AudioSource>();
        click = false;
    }

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
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }
        
    }
}
