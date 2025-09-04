using UnityEngine;

// プレイヤーが所持しているアイテムのデータ管理
[System.Serializable]
public class PlayerItemData
{
    public int[] ownedItemIds; // データベースから送られてくるアイテムID配列
}