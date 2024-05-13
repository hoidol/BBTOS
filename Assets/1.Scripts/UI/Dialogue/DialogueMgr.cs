using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Boomlagoon.JSON;
using TMPro;

public class DialogueMgr : MonoSingleton<DialogueMgr>
{
    public DialogueCanvas canvas;

    bool _opened;
    public bool opened
    {
        get
        {
            return _opened;
        }
    }
    public Transform parentTr;
    public DialoguePanel dialoguePanelPrefab;
    public MindDialgouePanel mindDialgouePanelPrefab;
    public InteractResultDialoguePanel interactDialoguePanelPrefab;

    public RectTransform dialoguePanelRecTr;

    public GameObject nextBtnPanel;
    public GameObject playBtnPanel;

    public GameObject optionPanel;
    public DialogueOption[] dialogueOptions;
    Vector2 initPosition;
    Vector2 otherPosition;

    public Image thumImg;
    public CharacterData[] characterDatas;
    public TMP_Text nextText;

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();

    public override void Awake()
    {
        Load();

    }

    void Load()
    {
        string jsonStr = Resources.Load<TextAsset>("JSON/DialogueData").text;

        JSONObject obj = JSONObject.Parse(jsonStr);

        JSONArray array = obj.GetArray("CSV");

        for(int i =0;i< array.Length; i++)
        {
            Dialogue dialogue = new Dialogue();
            //idx	type	name	script	option1	option2	option3	option1Active	option2Active	option3Active	info	end
            dialogue.idx = int.Parse(array[i].Obj.GetString("idx"));

            dialogueDic.Add(dialogue.idx,dialogue);
            dialogue.characterType = System.Enum.Parse<CharacterType>(array[i].Obj.GetString("name"));
            dialogue.type = System.Enum.Parse<DialogueType>(array[i].Obj.GetString("type"));
            dialogue.script = array[i].Obj.GetString("script").Replace("\\n","\n");
            dialogue.script = dialogue.script.Replace("\\\"", "\"");
            if (!string.IsNullOrEmpty(array[i].Obj.GetString("end")))
                dialogue.end = true;
            else
                dialogue.end = false;

            switch (dialogue.type)
            {
                case DialogueType.Option:
                    DialogueOptionData oData = new DialogueOptionData();
                    dialogue.options.Add(oData);
                    oData.script = array[i].Obj.GetString("option1").Replace("\\n", "\n");
                    oData.script = oData.script.Replace("\\\"", "\"");
                    oData.disable = false;

                    oData = new DialogueOptionData();
                    dialogue.options.Add(oData);
                    oData.script = array[i].Obj.GetString("option2").Replace("\\n", "\n");
                    oData.script = oData.script.Replace("\\\"", "\"");
                    oData.disable = true;
                    break;
                case DialogueType.Check:
                    string[] infos = array[i].Obj.GetString("info").Split('/');
                    //TRUE/후각 굴림 성공
                    dialogue.check = bool.Parse(infos[0]);
                    dialogue.mindScript = infos[1];
                    break;
                case DialogueType.Mind:
                    dialogue.script = array[i].Obj.GetString("option1").Replace("\\n", "\n");

                    dialogue.script = dialogue.script.Replace("\\\"", "\"");
                    dialogue.mindScript = array[i].Obj.GetString("option2").Replace("\\n", "\n");
                    dialogue.mindScript = dialogue.mindScript.Replace("\\\"", "\"");
                    //array[i].Obj.GetString("info");
                    break;
                case DialogueType.InteractResult:
                    dialogue.script = array[i].Obj.GetString("info");

                    dialogue.interactResultDatas[0] = new InteractResultDialogueData();
                    dialogue.interactResultDatas[0].locked = false;
                    dialogue.interactResultDatas[0].script = array[i].Obj.GetString("option1").Replace("\\n", "\n");
                    dialogue.interactResultDatas[0].script = dialogue.interactResultDatas[0].script.Replace("\\\"", "\"");

                    dialogue.interactResultDatas[1] = new InteractResultDialogueData();
                    dialogue.interactResultDatas[1].locked = false;
                    dialogue.interactResultDatas[1].script = array[i].Obj.GetString("option2").Replace("\\n", "\n");
                    dialogue.interactResultDatas[1].script = dialogue.interactResultDatas[1].script.Replace("\\\"", "\"");

                    dialogue.interactResultDatas[2] = new InteractResultDialogueData();
                    dialogue.interactResultDatas[2].locked = true;
                    if (!string.IsNullOrEmpty(array[i].Obj.GetString("option3")))
                    {
                        dialogue.interactResultDatas[2].locked = false;
                        dialogue.interactResultDatas[2].script = array[i].Obj.GetString("option3").Replace("\\n", "\n");
                        dialogue.interactResultDatas[2].script = dialogue.interactResultDatas[2].script.Replace("\\\"", "\"");
                    }
                    
                    

                    break;

            }


        }

    }
    private void Start()
    {
        initPosition = dialoguePanelRecTr.anchoredPosition;
        otherPosition = initPosition * -1f;
        thumImg.gameObject.SetActive(false);
    }


    public List<GameObject> dialoguePanels = new List<GameObject>();
    //public List<Dialogue> dialogueStack = new List<Dialogue>();
    public int curIdx;
    public Action endCallback;
    public void StartDialogue(int idx, Action endCallback)
    {
        this.endCallback = endCallback;
        curIdx = idx;
        nextBtnPanel.SetActive(false);
        optionPanel.SetActive(false);

        for(int i=0;i< dialoguePanels.Count; i++)
        {
            Destroy(dialoguePanels[i].gameObject);
        }
        dialoguePanels.Clear();

        Open(() => {
            Debug.Log("대사 나와라!");
            ShowDialogue();
        });
        //dialogueStack.AddRange(d);

    }

    //public void AddDialogue(Action endCallback, params Dialogue[] d)
    //{
    //    this.endCallback = endCallback;
    //    Open(()=> {
    //        NextDialogue();
    //    });
    //    dialogueStack.AddRange(d);

    //}

    Dialogue curDialogue;
    MindDialgouePanel curMindDialoguePanel;
    public void ShowDialogue()
    {
        if (curMindDialoguePanel != null)
            return;

        
        curDialogue = dialogueDic[curIdx];

        nextBtnPanel.SetActive(true);
        optionPanel.SetActive(false);
        playBtnPanel.SetActive(false);
        CharacterData cData = GetCharacterData(curDialogue.characterType);

        nextText.color = new Color(1, 1, 1, 1f);


        if (curDialogue.type == DialogueType.Normal ||
            curDialogue.type == DialogueType.Check ||
            curDialogue.type == DialogueType.Option)
        {

            DialoguePanel dialoguePanel = Instantiate(dialoguePanelPrefab, parentTr);
            dialoguePanel.SetData(curDialogue,cData);
            dialoguePanels.Add(dialoguePanel.gameObject);
            if (curDialogue.type == DialogueType.Option)
            {
                optionPanel.SetActive(true);
                nextBtnPanel.SetActive(false);
                for (int i=0;i< dialogueOptions.Length; i++)
                {
                    if(i < curDialogue.options.Count)
                    {
                        dialogueOptions[i].gameObject.SetActive(true);
                        dialogueOptions[i].SetDialogue(curDialogue.options[i]);
                    }
                    else
                    {
                        dialogueOptions[i].gameObject.SetActive(false);
                    }
                } 
            }
        }
        else if(curDialogue.type == DialogueType.InteractResult)
        {
            InteractResultDialoguePanel resultPanel = Instantiate(interactDialoguePanelPrefab, parentTr);
            resultPanel.SetData(curDialogue, cData);
            dialoguePanels.Add(resultPanel.gameObject);
        }
        else if (curDialogue.type == DialogueType.Mind)
        {
            nextText.color = new Color(1, 1, 1, 0.5f);
            playBtnPanel.SetActive(true);
            curMindDialoguePanel = Instantiate(mindDialgouePanelPrefab, parentTr);
            curMindDialoguePanel.SetData(curDialogue,cData);
            dialoguePanels.Add(curMindDialoguePanel.gameObject);
        }

        thumImg.gameObject.SetActive(true);
        thumImg.sprite = cData.thum;

    }

    public void OnClickedNextBtn()
    {
        if (curMindDialoguePanel != null)
            return;
        if (dialogueDic[curIdx].end)
        {
            
            EndDialogue();
            return;
        }
        curIdx++;
        ShowDialogue();
        if (curDialogue.type == DialogueType.Check)
        {
            SoundMgr.Instance?.PlaySound("AutoCheckSuc");
        }else
        {
            SoundMgr.Instance?.PlaySound("Button");
        }
    }

    public void OnClickedPlayBtn()
    {
        curMindDialoguePanel.OpenMind();
        curMindDialoguePanel = null;
        SkillMgr.Instance.skillInfos[7].gameObject.SetActive(false);
        playBtnPanel.SetActive(false);
        nextBtnPanel.SetActive(true);

        SoundMgr.Instance?.PlaySound("Skill8");
    }

    public CharacterData GetCharacterData(CharacterType type)
    {
        for(int i = 0; i < characterDatas.Length; i++)
        {
            if (characterDatas[i].type == type)
                return characterDatas[i];
        }
        return null;
    }
    public void EndDialogue()
    {
        Debug.Log("DialogueMgr EndDialogue");
        thumImg.gameObject.SetActive(false);
        endCallback?.Invoke();
        Close(()=> {
            canvas.gameObject.SetActive(false);
        });
    }

    bool moving;
    Action movingCallback;
    public void Open(Action oCallback = null)
    {
        if (_opened)
            return;

        SoundMgr.Instance?.PlaySound("TalkUI");
        canvas.gameObject.SetActive(true);
        _opened = true;
        moving = true;
        movingCallback = oCallback ;
        
        CameraMgr.Instance.ChangeDialogueState(true);
    }

    public void Close(Action oCallback = null)
    {
        if (!_opened)
            return;
        _opened = false;

        moving = true;
        movingCallback = oCallback;
        CameraMgr.Instance.dialogueOpened = _opened;

        CameraMgr.Instance.ChangeDialogueState(false);
    }

    //확실하게 다 열린다음에 글씨보이게
    void Update()
    {

        if (_opened)
        {
            if (Vector2.SqrMagnitude( dialoguePanelRecTr.anchoredPosition - otherPosition)<0.1f && moving)
            {
                moving = false;
                movingCallback?.Invoke();
                return;
            }

            dialoguePanelRecTr.anchoredPosition = Vector2.Lerp(dialoguePanelRecTr.anchoredPosition, otherPosition, Time.deltaTime * 10);
        }
        else
        {
            if (Vector2.SqrMagnitude(dialoguePanelRecTr.anchoredPosition - initPosition) < 0.1f && moving)
            {
                moving = false;
                movingCallback?.Invoke();
                return;
            }
            dialoguePanelRecTr.anchoredPosition = Vector2.Lerp(dialoguePanelRecTr.anchoredPosition, initPosition, Time.deltaTime * 10);            
        }
    }
}

[System.Serializable]
public  class Dialogue
{
    public int idx;
    public DialogueType type;
    public CharacterType characterType;
    public string script;
    public List<DialogueOptionData> options = new List<DialogueOptionData>();
    public InteractResultDialogueData[] interactResultDatas = new InteractResultDialogueData[3];
    public bool check;

    public bool end;

    public string mindScript;
}

[System.Serializable]
public class CharacterData
{
    public CharacterType type;
    public string name;
    public Sprite thum;
    public Color color;
}
public enum CharacterType
{
    Alice,
    Ego,
    Cheshire,
    Candy,
    Cotton
       
}
public class DialogueOptionData
{
    public string script;
    public bool disable;
}

public class InteractResultDialogueData
{
    public string script;
    public bool locked;
}


public enum DialogueType
{
    Normal,
    Check,
    Option,
    InteractResult,
    Mind //속마음
}