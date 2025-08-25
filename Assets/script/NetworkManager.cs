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
    // WebAPIの接続先を設定
#if DEBUG
    // 開発環境で使用する値をセット
    const string API_BASE_URL = "http://localhost:8000/api/";
#else
  // 本番環境で使用する値をセット
  const string API_BASE_URL = "https://…azure.com/api/";
#endif

    //private int userID; // 自分のユーザーID
    private string apiToken; // APUトークン
    private string userName; // 入力される想定の自分のユーザー名

    // プロパティ

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

    // 通信用の関数

    //ユーザー登録処理
    public IEnumerator RegistUser(string name, int level, Action<bool> result)
    {
        //サーバーに送信するオブジェクトを作成
        RegistUserRequest requestData = new RegistUserRequest();
        requestData.Name = name;
        requestData.Level = level;
        //サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(requestData);
        //送信
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/store", json, "application/json");
        //結果を受け取るまで待機
        yield return request.SendWebRequest();
        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success
&& request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response =
                         JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);
            //ファイルにユーザーIDを保存
            this.userName = name;
            this.apiToken = response.APIToken;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //ここで呼び出し元のresult処理を呼び出す


    }

    // ユーザー情報を保存する
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

    // ユーザー情報を読み込む
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
    //ユーザー情報更新
    public IEnumerator UpdateUser(string name, int level, Action<bool> result)
    {
        //サーバーに送信するオブジェクトを作成
        UpdateUserRequest requestData = new UpdateUserRequest();
        requestData.Name = name;
        requestData.Level = 1;
        //サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(requestData);
        //送信
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + this.apiToken);

        yield return request.SendWebRequest();

        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success
         && request.responseCode == 200)
        {
            // 通信が成功した場合、ファイルに更新したユーザー名を保存
            this.userName = name;
            SaveUserData();
            isSuccess = true;
        }

        result?.Invoke(isSuccess); //ここで呼び出し元のresult処理を呼び出す

    }

    public IEnumerator GetCell(Action<CellResponse[]> result, int id)
    {
        //ステージ一覧取得APIを実行
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "stages/get/" + id);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success
        && request.responseCode == 200)
        {

            //結果を通知
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            Debug.Log("レスポンス: " + resultJson);
            CellResponse[] cells = JsonConvert.DeserializeObject<CellResponse[]>(resultJson);
            result?.Invoke(cells);
        }
        else
        {
            result?.Invoke(null);
        }

    }

    //ステージ一覧取得処理
    public IEnumerator GetStage(Action<StageResponse[]> result)
    {
        //ステージ一覧取得APIを実行
        UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "stages/index");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success
        && request.responseCode == 200)
        {

            //結果を通知
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
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

            //ステージ一覧取得APIを実行
            UnityWebRequest request = UnityWebRequest.Get(API_BASE_URL + "users/show");
            // ヘッダー設定
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + saveData.APIToken);

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success
            && request.responseCode == 200)
            {

                //結果を通知
                //通信が成功した場合、返ってきたJSONをオブジェクトに変換
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

        // WWWFormを使用（空のフォーム）
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

