using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoSingleton<BattleMgr>
{
    public BattleEnemy[] enemies;

    public GameObject endTurnBtn;
    public GameObject applyingSkillPanel;
    public BattleMode mode;
    public override void Awake()
    {
        // battleUnits = FindObjectsOfType<BattleUnit>(true);
        endTurnBtn.SetActive(false);
        applyingSkillPanel.SetActive(false);

        //Time.timeScale = 3;
    }
    //--------- 전투

    // 1. 
    // 2. 캔, 코튼 앨리스 순서로 공격도입 대사 시작
    // 3. 선택지 선택 -> 턴 종료
    // 4. 주사위를 굴림 , 주사위가 없는 경우도 있음
    // 5. 굴려진 주사위가 연결되어있는 말풍선으로 이동하고 주사위 사라지면서 효과적용 + 난이도 이상되야지 효과 적용됌
    // 7. 방금 턴의 결과에 대한 대사
    // 반복

    public void ReadyBattle()
    {
        Debug.Log("BattleMgr ReadyBattle");

        StartCoroutine(CoBattle());
    }
    public int curRound;
    bool selectedOption;
    IEnumerator CoBattle()
    {

        yield return new WaitForSeconds(0.5f);
        enemies[0].StartBattle(); //캔디
        yield return new WaitForSeconds(1);
        enemies[1].StartBattle(); //코튼
        yield return new WaitForSeconds(1);
        BattlePlayer.Instance.StartBattle(); //플레이어
        yield return new WaitForSeconds(2);

        enemies[0].battleOption.dialogueText.text = ""; 
        enemies[1].battleOption.dialogueText.text = "";
        BattlePlayer.Instance.bubble.gameObject.SetActive(false);

        for(int i = 0; i < 4; i++)
        {
            curRound = i;
               yield return new WaitForSeconds(1);
            Debug.Log("캔, 코튼 앨리스 순서로 공격도입 대사 시작");
            //2.캔, 코튼 앨리스 순서로 공격도입 대사 시작
            enemies[0].StartRound(i); //캔디
            enemies[1].StartRound(i); //코튼
            BattlePlayer.Instance.StartRound(i); //플레이어
            //yield return new WaitForSeconds(1);

            enemies[0].battleOption.dialogueText.text = null;
            enemies[1].battleOption.dialogueText.text = null;
            BattlePlayer.Instance.bubble.gameObject.SetActive(false);

            // 3. 선택지 선택 -> 턴 종료

            Debug.Log("선택지 선택");
            enemies[0].StartSelect(i); //캔디
            yield return new WaitForSeconds(1f);
            enemies[1].StartSelect(i); //코튼
            yield return new WaitForSeconds(1f);

            endTurnBtn.SetActive(true);
            applyingSkillPanel.SetActive(false);
            BattlePlayer.Instance.StartSelect(i); //플레이어
            BattleCat.Instance.StartSelect(i);

            enemies[0].UpdatePointer(i);
            enemies[1].UpdatePointer(i);
            BattlePlayer.Instance.UpdatePointer(i); //플레이어

            //고양이 보너스 체크
            bool doneBonus = false;
            BattleCat.Instance.RollBonus(i, () =>{ doneBonus = true;});
            yield return new WaitUntil(() => doneBonus);

            
            selectedOption = false;
            yield return new WaitUntil(() =>
            {
                return selectedOption;
            });
            selectedOption = false;
            Debug.Log("선택지 완료" + selectedOption);
            BattlePlayer.Instance.EndSelect(i); //플레이어
            BattleCat.Instance.EndSelect(i);
            enemies[0].EndSelect(i); //캔디
            enemies[1].EndSelect(i); //코튼

            //고양이 보너스가 있으면
            //선택한 옵션에 보너스 주사위 생기기
            endTurnBtn.SetActive(false);
            applyingSkillPanel.SetActive(true);

            Debug.Log("주사위를 굴리기!" );
            // 4. 주사위를 굴림 , 주사위가 없는 경우도 있음
            enemies[0].Roll(i); //캔디
            yield return new WaitForSeconds(1);//3으로 바꾸기
            // 5. 굴려진 주사위가 연결되어있는 말풍선으로 이동하고 주사위 사라지면서 효과적용 + 난이도 이상되야지 효과 적용됌
            bool doenEffect = false;
            enemies[0].ApplySkill(i, () => { doenEffect = true; }); //캔디
            yield return new WaitUntil(() => doenEffect);


            enemies[1].Roll(i); //코튼
            yield return new WaitForSeconds(1);//3으로 바꾸기
            doenEffect = false;
            enemies[1].ApplySkill(i, () => { doenEffect = true; }); //코튼
            yield return new WaitUntil(() => doenEffect);


            BattlePlayer.Instance.Roll(i); //플레이어
            yield return new WaitForSeconds(1);//3으로 바꾸기
            doenEffect = false;
            BattlePlayer.Instance.ApplySkill(i,()=> { doenEffect = true;}); //플레이어
            yield return new WaitUntil(() => doenEffect);
            yield return new WaitForSeconds(1);



            Debug.Log("라운드 끝내기!");
            applyingSkillPanel.SetActive(false);
            enemies[0].EndRound(i); //캔디
            yield return new WaitForSeconds(1);
            enemies[1].EndRound(i); //코튼
            yield return new WaitForSeconds(1);
            BattlePlayer.Instance.EndRound(i); //플레이어
            yield return new WaitForSeconds(2);
            enemies[0].battleOption.dialogueText.text = null;
            enemies[1].battleOption.dialogueText.text = null;
            BattlePlayer.Instance.bubble.gameObject.SetActive(false);
        }

        enemies[0].EndBattle(); //캔디
        enemies[1].EndBattle(); //코튼
        BattlePlayer.Instance.EndBattle(); //플레이어
        //yield return new WaitForSeconds(2);


        enemies[0].battleOption.gameObject.SetActive(false);
        enemies[1].battleOption.gameObject.SetActive(false);
        BattlePlayer.Instance.bubble.gameObject.SetActive(false);
        mode.EndMode();
    }


    public void EndTurn()
    {
        //플레이어가 뭔가를 선택해야지 완료 누를 수 있음
        //player
        if (PlayerBattleOption.selectedOption == null)
            return;

        selectedOption = true;
    }
}
