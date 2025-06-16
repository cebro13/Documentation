using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAllChildWhenEnter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
