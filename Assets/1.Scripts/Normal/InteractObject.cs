using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractObject : MonoBehaviour
{
    //public Transform bodyTr;
    public InteractEvent[] interactEvents;
    public bool interacting;

    public bool interacted;
    public int diceNumber;

    public GameObject hoObject;
    public string foundText;

    private void Awake()
    {
      //  bodyTr.gameObject.SetActive(false);
        //interactEvents = GetComponentsInChildren<InteractEvent>();
    }

    public void Found(int number, Action endCallback)
    {
        if (interacted)
            return;
        interacting = true;
        StartCoroutine(CoInteract(endCallback));
    }

    IEnumerator CoInteract(Action endCallback)
    {
        for (int i = 0; i < interactEvents.Length; i++)
        {
            bool next = false;
            interactEvents[i].Interact(() => {
                next = true;
            });
            yield return new WaitUntil(() => next);
            yield return new WaitForSeconds(0.5f);
        }
        hoObject.SetActive(false);
        endCallback.Invoke();
        interacted = true;
    }
        

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "ExplorerArea")
        {
            //if(!bodyTr.gameObject.activeSelf)
            //    bodyTr.gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ExplorerArea")
        {
            //if (bodyTr.gameObject.activeSelf)
            //    bodyTr.gameObject.SetActive(false);
        }
    }
}
