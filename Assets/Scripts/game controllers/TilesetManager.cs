using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesetManager : MonoBehaviour
{

    public bool DisableHitbox;
    void Start()
    {
        try
        {
            this.GetComponent<BoxCollider2D>().enabled = !DisableHitbox;
        }
        catch
        {
            try
            {
                this.GetComponent<PolygonCollider2D>().enabled = !DisableHitbox;
            }
            catch
            {
                Debug.Log("Hitbox not found");
            }
        }

        this.transform.position = new Vector2(
            Mathf.Round( this.transform.position.x * 100) / 100,
            Mathf.Round( this.transform.position.y * 100) / 100
            );
    }
}
