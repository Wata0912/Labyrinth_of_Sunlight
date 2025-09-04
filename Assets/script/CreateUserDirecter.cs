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

        //���[�U�[�f�[�^���ۑ�����ĂȂ��ꍇ�͓o�^
        StartCoroutine(NetworkManager.Instance.RegistUser(
            inputText, 1,        //���O
       result =>
       {                          //�o�^�I����̏���
           if (result == true)
           {
               SceneManager.LoadScene("SelectStageScene");
           }
           else
           {
               Debug.Log("���[�U�[�o�^������ɏI�����܂���ł����B");
        
           }
       }));
    }
}
