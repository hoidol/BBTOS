using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoSingleton<CameraMgr>
{
    public RectTransform dialogueEmptyRect;
    public Vector2 dialoguePosition;
    Camera mainCamera;
    public override  void Awake()
    {
        mainCamera = GetComponent<Camera>();
        
    }

    public Vector2 cameraTargetPosition;
    public bool dialogueOpened;
    const float initOrthographicSize = 6;
    public void ChangeDialogueState(bool b)
    {
        if(dialoguePosition == Vector2.zero)
            dialoguePosition = Camera.main.ScreenToWorldPoint(dialogueEmptyRect.position) * -1;
        dialogueOpened = b;
        if (b)
        {
            cameraTargetPosition = dialoguePosition;
        }
        else
        {
            cameraTargetPosition = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (zoomTimer > 0)
        {
            zoomTimer -= Time.deltaTime;

            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomSize, Time.deltaTime * 5);
        }
        else
        {
            if (zooming)
            {
                cameraTargetPosition = Vector2.zero;
            }
                
            zooming = false;
            if (!Mathf.Approximately( mainCamera.orthographicSize, initOrthographicSize))
            {
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, initOrthographicSize, Time.deltaTime * 10);
            }

            
        }

        transform.position = Vector2.Lerp(transform.position, cameraTargetPosition, Time.deltaTime * 10);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ZoomIn(zoomTr.position);
        }
    }
    public Transform zoomTr;
    [SerializeField] float zoomTimer;
    [SerializeField] float zoomSize;


    bool zooming;
    public void ZoomIn(Vector2 position, float size = 2, float timer =2f)
    {
        zooming = true;
           cameraTargetPosition = position;
        zoomTimer = timer;
        zoomSize = size;
    }
    
    //public void
}
