using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTileScript : MonoBehaviour
{
    [SerializeField]
    private Character character;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            character.MyCurrentTile = collision.transform;
        }
    }
}
