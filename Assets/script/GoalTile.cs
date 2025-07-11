using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D boxCollider2D;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        // –³Œø‰»
        boxCollider2D.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCollider()
    {
        boxCollider2D.enabled = true;
    }
}
