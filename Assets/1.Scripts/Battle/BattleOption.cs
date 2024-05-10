using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOption : MonoBehaviour
{
    
    public Transform endPointTr;

    //[HideInInspector]  public BattleOption targetOption;

    // public  void Point(BattleOption option)
    // {
    //     targetOption = option;
    //     Debug.Log("Point : " + transform.gameObject.name);
    //     Debug.Log("option.transform.position : " + option.transform.position);
    //     RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, option.transform.position - transform.position, float.MaxValue);
    //     Debug.Log($"hits.Length {hits.Length}");
    //     for (int i =0;i< hits.Length; i++)
    //     {
    //         if(hits[i].transform == targetOption.transform)
    //         {
    //             ArrowPointer.Instantiate(transform.position, hits[i].point, Color.red);
    //             break;
    //         }
    //     }
    // }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.T))
    //     {
    //         if (targetOption == null)
    //             return;
    //         Point(targetOption);
    //     }
    // }
}
