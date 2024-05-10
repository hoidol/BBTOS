using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathIndicator : MonoBehaviour
{
    public LineRenderer lineRdr;
    List<Transform> pathTrs = new List<Transform>();
    private void Awake()
    {
        lineRdr = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Transform[] trs =GetComponentsInChildren<Transform>();
        for(int i=0;i< trs.Length; i++)
        {
            if (trs[i] == transform)
                continue;
            pathTrs.Add(trs[i]);

        }

        lineRdr.positionCount = pathTrs.Count;
        for (int i =0;i< pathTrs.Count; i++)
        {
            lineRdr.SetPosition(i, pathTrs[i].position);
        }

    }

}
