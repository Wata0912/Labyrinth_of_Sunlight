using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 moveColliderSize = new Vector2(0.05f, 0.05f);//移動時のタイルの当たり判定サイズを道の幅にするためのもの
    BoxCollider2D boxCollider2D;
    public List<Vector2> forbiddenDirections;//通行不可の方向
    public List<DirectionMapping> directionMappings;//侵入方向と出力方向のリスト

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    [System.Serializable]//unity側で複数設定
    public class DirectionMapping
    {
        public Vector2 incomingDirection; // プレイヤーが入ってくる方向
        public Vector2 outgoingDirection; // 出ていく方向
    }

    /// <summary>
    /// 来た方向に対応する出ていく方向を返す
    /// </summary>
    public Vector2 GetNewDirectionFrom(Vector2 incoming)
    {
       
        foreach (var mapping in directionMappings)
        {   
            //プレイヤーの入ってきた方向が設定されたものと逆なら(右に進行している場合左から入って来る)
            if (Vector2.Equals(mapping.incomingDirection.normalized, -incoming.normalized))
            {   //出力方向に進行方向を変更する
                return mapping.outgoingDirection.normalized;
            }
        }

        //
        return Vector2.zero; 
    }

    public void ChangeMoveCollider()
    {   //移動時に道幅に変更
        boxCollider2D.size = moveColliderSize;
    }

    public void ResetCollider()
    {   //念の為もう一回取得(unity側でエラーが出る)
        boxCollider2D = GetComponent<BoxCollider2D>();
        //タイル操作用に戻す
        boxCollider2D.size = new Vector2(1,1);
    }

    //侵入禁止判定
    public bool IsBlockedFromDirection(Vector2 incomingDirection)
    {   
        foreach (var dir in forbiddenDirections)
        {   //禁止方向リストが侵入方向と一致したら停止
            if (Vector2.Equals(dir.normalized, incomingDirection.normalized))
            {
                return true;
            }
        }
        return false;
    }

}
