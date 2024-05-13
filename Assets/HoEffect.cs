using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoEffect : MonoBehaviour
{
    InteractObject iObj;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        iObj = GetComponentInParent<InteractObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (iObj.interacted)
            return;

        if (collision.CompareTag("CatArea"))
        {
            spriteRenderer.enabled = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {

        if (iObj.interacted)
            return;

        if (collision.CompareTag("CatArea"))
        {
            spriteRenderer.enabled = false;
        }
    }
}
