using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnemy : MonoBehaviour
{
    public InteractObject[] interactObjects;
    public InteractEvent[] interactEvents;
    public GameObject bodyObj;
    public GameObject candyObj;

    public bool interacting;
    public bool interacted;
    void Active()
    {
        if (interacting)
            return;
        if (interacted)
            return;

        bodyObj.SetActive(true);

        interacting = true;

        StartCoroutine(CoInteract(()=> {
            candyObj.SetActive(false);
        }));
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

            Debug.Log($"InteractEnemy {i} 완료");
            yield return new WaitForSeconds(0.5f);
        }
        endCallback.Invoke();
        interacted = true;
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < interactObjects.Length; i++)
        {
            if (!interactObjects[i].interacted)
                return;
        }
        Active();

    }
}
