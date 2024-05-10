using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    
    public static ArrowPointer Instantiate()
    {
        ArrowPointer arrowPrefab = Resources.Load<ArrowPointer>("Prefabs/ArrowPointer");
        ArrowPointer arrow = Instantiate(arrowPrefab);
        //arrow.SetArrow(start, end, color);
        return arrow;
    }
    public LayerMask mask;

    public void Point(Transform owner, Vector2 start, Vector2 end, Color color)
    {
        //ArrowPointer arrowPointer = null;
        SetArrow(start, end, color);
        //RaycastHit2D[] hits = Physics2D.RaycastAll(start, end - start, float.MaxValue, mask);
        //for (int i = 0; i < hits.Length; i++)
        //{
        //    if (hits[i].transform != owner)
        //    {
        //        SetArrow(start, hits[i].point, color);
        //        //arrowPointer = Instantiate(start, hits[i].point, Color.red);
        //        break;
        //    }
        //}
    }
    public void Point(Vector2 start, Vector2 end, Color color)
    {
        arrowTr.position = end;
        arrowTr.up = end - start;

        lineRenderer.positionCount = 2;
        
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        spriteRdr.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }


    public LineRenderer lineRenderer;

    public Transform arrowTr;
    public SpriteRenderer spriteRdr;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRdr = arrowTr.GetComponent<SpriteRenderer>();

        lineRenderer.positionCount = 0;

    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        arrowTr.gameObject.SetActive(true);
    //        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        lineRenderer.SetPosition(1, pos);
    //        arrowTr.position = lineRenderer.GetPosition(1);
    //        arrowTr.up = arrowTr.position - transform.position;
    //    }
    //    else if (Input.GetMouseButton(0))
    //    {
    //        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        lineRenderer.SetPosition(1, pos);
    //        arrowTr.position = lineRenderer.GetPosition(1);
    //        arrowTr.up = arrowTr.position - transform.position;
    //    }
    //    else if (Input.GetMouseButtonUp(0))
    //    {
    //        arrowTr.gameObject.SetActive(false);
    //        lineRenderer.SetPosition(0, transform.position);
    //        lineRenderer.SetPosition(1, transform.position);
    //    }
    //}

    
    public void SetArrow(Vector2 start, Vector2 end, Color color)
    {
        lineRenderer.positionCount = 2;

        Vector2 dir = end - start;
        float distance = dir.magnitude - 0.36f;

        end = start + dir.normalized * distance;

        arrowTr.position = end;
        arrowTr.up = end - start;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        spriteRdr.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

}
