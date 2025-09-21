using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ItemDex : MonoBehaviour
{
    [Header("UI関係")]
    [SerializeField] private Transform itemParent;     // GridLayoutGroup を設定した親オブジェクト
    [SerializeField] private GameObject itemPrefab;    // アイテム表示用のプレハブ（Image + Text）

    [Header("マスターデータ")]
    [SerializeField] private List<ItemData> itemMaster;  // IDごとのアイテム情報

    private Dictionary<int, ItemData> itemDict;

    private void Start()
    {
        // マスターデータをDictionary化して高速アクセス可能にする
        itemDict = new Dictionary<int, ItemData>();
        foreach (var data in itemMaster)
        {
            itemDict[data.id] = data;
        }

        // コルーチン開始
        StartCoroutine(NetworkManager.Instance.HaveItem(OnItemsReceived));
    }

    // アイテム取得後の処理
    private void OnItemsReceived(ItemReqest[] items)
    {
        if (items == null)
        {
            Debug.LogError("アイテム取得失敗");
            return;
        }

        foreach (var item in items)
        {
            if (itemDict.TryGetValue(item.id, out ItemData data))
            {
                // プレハブ生成
                GameObject obj = Instantiate(itemPrefab, itemParent);

                // プレハブ内のImageとTextに反映
                obj.transform.Find("ItemImage").GetComponent<Image>().sprite = data.itemSprite;
                obj.transform.Find("ItemName").GetComponent<Text>().text = data.itemName;
            }
            else
            {
                Debug.LogWarning($"マスターデータに存在しないID: {item.id}");
            }
        }
    }

    public void Exit()
    {
        //SceneManager.LoadScene("SelectStageScene");
        Initiate.Fade("SelectStageScene", Color.black, 1.0f);
    }

}
