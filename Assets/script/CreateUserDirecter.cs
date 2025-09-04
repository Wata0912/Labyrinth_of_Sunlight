using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateUserDirecter : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        if (inputField == null)
            inputField = GetComponent<InputField>();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUser()
    {
        string inputText = inputField.text;

        //ユーザーデータが保存されてない場合は登録
        StartCoroutine(NetworkManager.Instance.RegistUser(
            inputText, 1,        //名前
       result =>
       {                          //登録終了後の処理
           if (result == true)
           {
               SceneManager.LoadScene("SelectStageScene");
           }
           else
           {
               Debug.Log("ユーザー登録が正常に終了しませんでした。");
        
           }
       }));
    }
}
