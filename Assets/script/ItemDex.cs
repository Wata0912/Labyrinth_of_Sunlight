using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ItemDex : MonoBehaviour
{
    [Header("UI�֌W")]
    [SerializeField] private Transform itemParent;     // GridLayoutGroup ��ݒ肵���e�I�u�W�F�N�g
    [SerializeField] private GameObject itemPrefab;    // �A�C�e���\���p�̃v���n�u�iImage + Text�j

    [Header("�}�X�^�[�f�[�^")]
    [SerializeField] private List<ItemData> itemMaster;  // ID���Ƃ̃A�C�e�����

    private Dictionary<int, ItemData> itemDict;

    AudioSource audioSource;
    public AudioClip sound1;
    bool click;

    private void Start()
    {
        // �}�X�^�[�f�[�^��Dictionary�����č����A�N�Z�X�\�ɂ���
        itemDict = new Dictionary<int, ItemData>();
        foreach (var data in itemMaster)
        {
            itemDict[data.id] = data;
        }

        // �R���[�`���J�n
        StartCoroutine(NetworkManager.Instance.HaveItem(OnItemsReceived));

        audioSource = GetComponent<AudioSource>();
        click = false;
    }

    // �A�C�e���擾��̏���
    private void OnItemsReceived(ItemReqest[] items)
    {
        if (items == null)
        {
            Debug.LogError("�A�C�e���擾���s");
            return;
        }

        foreach (var item in items)
        {
            if (itemDict.TryGetValue(item.id, out ItemData data))
            {
                // �v���n�u����
                GameObject obj = Instantiate(itemPrefab, itemParent);

                // �v���n�u����Image��Text�ɔ��f
                obj.transform.Find("ItemImage").GetComponent<Image>().sprite = data.itemSprite;
                obj.transform.Find("ItemName").GetComponent<Text>().text = data.itemName;
            }
            else
            {
                Debug.LogWarning($"�}�X�^�[�f�[�^�ɑ��݂��Ȃ�ID: {item.id}");
            }
        }
    }

    public void Exit()
    {
        //SceneManager.LoadScene("SelectStageScene");
        Initiate.Fade("SelectStageScene", Color.black, 1.0f);
        if (click == false)
        {
            audioSource.PlayOneShot(sound1);
            click = true;
        }
    }

}
