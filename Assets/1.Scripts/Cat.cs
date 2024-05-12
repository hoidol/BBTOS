using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Cat : MonoSingleton<Cat>
{
    //진입 시 바로 대화
    //캐릭터가 거미줄에 걸린 상태
    //첫 화면 다음 버튼 눌러서 다음 대사로 진행
    //상호작용 요소는
    //채셔 주사위
    //상호작용 3 후
    //몬스터 출연 -> 몬스터와 상호작용 시 전투 화면으로 이동
    //


    //주사위에 따라서
    //상호 작용의 정보를 얻을 수 있음 1,2 - 낮음 , 3 4 - 보통 5 6 - 높음

    // 속마음 말풍선 시
    // 연주하기 버튼 누를 수 있음
    // 속마음을 말하게됨
    bool interacting;
    public Transform originPoint;
    public Transform bodyTr;
    public bool draging;
    public LayerMask dragableLayerMask;
    public LayerMask interactMask;
    //Vector2 startPoint;
    Vector2 dragVector;
    bool found;

    InteractObject foundInteractObject;
    public GameObject holy;
    public GameObject worldCanvasObject;
    public TMP_Text foundText;
    void Update()
    {
        if (GameMgr.Instance.curModeType != GameModeType.Normal)
            return;

        

        if (!draging)
        {
            if (foundInteractTimer <= 0)
            {
                if(foundInteractObject == null)
                {
                    Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0);
                    for (int i = 0; i < cols.Length; i++)
                    {
                        if (cols[i].CompareTag("Interact"))
                        {
                            InteractObject iO = cols[i].GetComponent<InteractObject>();
                            if (iO.interacting)
                                continue;
                            Debug.Log("전투 진입");
                            StartRoll(cols[i].GetComponent<InteractObject>());
                        }
                    }
                }

                if (!found)
                {
                    transform.position = Vector2.Lerp(transform.position, originPoint.position, 5 * Time.deltaTime);
                    holy.SetActive(false);
                }
                    
            }
            foundInteractTimer -= Time.deltaTime;
        }
        if (DialogueMgr.Instance.opened)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (interacting)
                return;
            foundInteractObject = null;
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(worldPoint, dragableLayerMask);
            if (col != null && col.transform == bodyTr)
            {
                draging = true;
                dragVector = (Vector2)transform.position - worldPoint;
                found = false;
                holy.SetActive(false);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (!draging)
                return;

            if (interacting)
                return;

            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 point = dragVector + worldPoint;
            Vector2 direction = (point - (Vector2)Player.Instance.transform.position);

            Vector2 originDir = direction;
            direction.y = direction.y / 0.56f;

            float angle = Mathf.Atan2(originDir.x, originDir.y) * Mathf.Rad2Deg;

            float distance = direction.magnitude;
            if (distance >= 5)
            {
                distance = 5;
                float x = Mathf.Sin(angle * Mathf.Deg2Rad) * distance; // 
                float y = Mathf.Cos(angle * Mathf.Deg2Rad) * distance * 0.56f; // 

                transform.position = (Vector2)Player.Instance.transform.position + new Vector2(x, y);
            }
            else
            {
                transform.position = point;
                //distance = direction.magnitude;
            }

            if (!found)
            {
                Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0, interactMask);
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].CompareTag("Interact"))
                    {
                        Debug.Log("상호작용 만남");
                        if (cols[i].GetComponent<InteractObject>().interacted)
                            continue;
                        interacting = true;
                        SoundMgr.Instance?.PlaySound("Interaction");
                        holy.SetActive(true);
                        found = true;
                        worldCanvasObject.SetActive(true);
                        foundText.text = cols[i].GetComponent<InteractObject>().foundText;
                        foundInteractTimer = 2;
                    }
                }
            }
            //else
            //{
            //    Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0, interactMask);
            //    if(cols.Length <= 0)
            //    {
            //        interacting = false;
            //        interacting = false;
            //        holy.SetActive(false);
            //    }
            //}
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!draging)
                return;

            //Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size,0);
            //for(int i = 0; i < cols.Length; i++)
            //{
            //    if (cols[i].CompareTag("Interact"))
            //    {
            //        Debug.Log("상호작용 만남");
            //        if (cols[i].GetComponent<InteractObject>().interacted)
            //            continue;
            //        SoundMgr.Instance.PlaySound("Interaction");
            //        holy.SetActive(true);
            //        found = true;
            //        foundInteractTimer = 2;
            //    }
            //}

            draging = false;
        }
    }

    void StartRoll(InteractObject interactObj)
    {

        foundInteractObject = interactObj;
        
        Dice.Instance.Roll(bodyTr.transform.position, interactObj.diceNumber, (number) =>
        {
            holy.SetActive(false);
            worldCanvasObject.SetActive(false);
            foundInteractObject.Found(number,()=> {
                foundInteractObject = null;
                found = false;
                interacting = false;
                draging = false;
            });
        });
    }



    public Vector2 offset;
    public Vector2 size;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((Vector2)transform.position + offset, size);
    }

    float foundInteractTimer;
}
