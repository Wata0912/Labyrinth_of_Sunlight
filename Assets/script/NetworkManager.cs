using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;
using System.IO;


public class NetworkManager : MonoBehaviour
{
    // WebAPI�̐ڑ����ݒ�
#if DEBUG
    // �J�����Ŏg�p����l���Z�b�g
    const string API_BASE_URL = "http://localhost:8000/api/";
#else
  // �{�Ԋ��Ŏg�p����l���Z�b�g
  const string API_BASE_URL = "https://�cazure.com/api/";
#endif

    //private int userID; // �����̃��[�U�[ID
    private string apiToken; // APU�g�[�N��
    private string userName; // ���͂����z��̎����̃��[�U�[��

    // �v���p�e�B

    public string UserName
    {
        get
        {
            return this.userName;
        }
    }

    private static NetworkManager instance;
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObj = new GameObject("NetworkManager");
                instance = gameObj.AddComponent<NetworkManager>();
                DontDestroyOnLoad(gameObj);
            }
            return instance;
        }
    }

    // �ʐM�p�̊֐�

    //���[�U�[�o�^����
    public IEnumerator RegistUser(string name, int level, Action<bool> result)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        RegistUserRequest requestData = new RegistUserRequest();
        requestData.Name = name;
        requestData.Level = level;
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);
        //���M
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/store", json, "application/json");
        //���ʂ��󂯎��܂őҋ@
        yield return request.SendWebRequest();
        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success
&& request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response =
                         JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);
            //�t�@�C���Ƀ��[�U�[ID��ۑ�
            this.userName = name;
            this.apiToken = response.APIToken;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //�����ŌĂяo������result�������Ăяo��


    }

    // ���[�U�[����ۑ�����
    private void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.UserName = this.userName;
        saveData.APIToken = this.apiToken;
        string json = JsonConvert.SerializeObject(saveData);
        var writer =
                new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();



    }

    // ���[�U�[����ǂݍ���
    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            return false;
        }
        var reader =
                   new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        this.apiToken = saveData.APIToken;
        this.userName = saveData.UserName;
        return true;
    }
    //���[�U�[���X�V
    public IEnumerator UpdateUser(string name, int level, Action<bool> result)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        UpdateUserRequest requestData = new UpdateUserRequest();
        requestData.Name = name;
        requestData.Level = 1;
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);
        //���M
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + this.apiToken);

        yield return request.SendWebRequest();

        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success
         && request.responseCode == 200)
        {
            // �ʐM�����������ꍇ�A�t�@�C���ɍX�V�������[�U�[����ۑ�
            this.userName = name;
            SaveUserData();
            isSuccess = true;
        }

        result?.Invoke(isSuccess); //�����ŌĂяo������result�������Ăяo��

    }

    public IEnumerator GetCell(Action<CellResponse[]> result, int id)
    {
        //�X�e�[�W�ꗗ�擾API�����s
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "stages/get/" + id);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success
        && request.responseCode == 200)
        {

            //���ʂ�ʒm
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;
            Debug.Log("���X�|���X: " + resultJson);
            CellResponse[] cells = JsonConvert.DeserializeObject<CellResponse[]>(resultJson);
            result?.Invoke(cells);
        }
        else
        {
            result?.Invoke(null);
        }

    }

    //�X�e�[�W�ꗗ�擾����
    public IEnumerator GetStage(Action<StageResponse[]> result)
    {
        //�X�e�[�W�ꗗ�擾API�����s
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "stages/index");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success
        && request.responseCode == 200)
        {

            //���ʂ�ʒm
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;
            StageResponse[] stages = JsonConvert.DeserializeObject<StageResponse[]>(resultJson);
            result?.Invoke(stages);
        }
        else
        {
            result?.Invoke(null);
        }

    }

    public IEnumerator ShowUser(Action<UpdateUserRequest> result)
    {

        if (File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            var reader =
                   new StreamReader(Application.persistentDataPath + "/saveData.json");
            string json = reader.ReadToEnd();
            reader.Close();
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            //�X�e�[�W�ꗗ�擾API�����s
            UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/show");
            // �w�b�_�[�ݒ�
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + saveData.APIToken);

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
            {

                //���ʂ�ʒm
                //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
                string resultJson = request.downloadHandler.text;
                UpdateUserRequest user = JsonConvert.DeserializeObject<UpdateUserRequest>(resultJson);
                result?.Invoke(user);
            }
            else
            {
                result?.Invoke(null);
            }
        }

    }

    public IEnumerator LevelUP()
    {

        // WWWForm���g�p�i��̃t�H�[���j
        WWWForm form = new WWWForm();

        if (File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            var reader =
                   new StreamReader(Application.persistentDataPath + "/saveData.json");
            string json = reader.ReadToEnd();
            reader.Close();
            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

            UnityWebRequest request = UnityWebRequest.Post(API_BASE_URL + "users/levelUP", form);
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + saveData.APIToken);
            yield return request.SendWebRequest();
            
        }
    }
}

