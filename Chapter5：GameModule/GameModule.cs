using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameModule : MonoBehaviour
{
    public static GameModule Instance {  get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public enum GameState
    {
        UI,
        SCENE,
        CG,
        CHANGE_KEY
    }
    public GameState lastState;
    GameState NowState;
    public GameState gameState
    {
        get
        {
            return NowState;
        }
        set
        {
            lastState = NowState;
            NowState = value;

        }
    }


    public string NowGameObject;
    public string NextGameObject;
    static int GAMEMODULE_OBJECTS_NUM = 10;
    public GameObject[] GameModule_Objects = new GameObject[GAMEMODULE_OBJECTS_NUM];


    void Start()
    {
        gameState = GameState.UI;
        GameObject_Init();

        EventModule.AddListener("PM_TransAnimationFinished", SwitchGameObject);

    }
    public void SetGameStateTo(string stateName)
    {
        foreach(int statevalue in Enum.GetValues(typeof(GameState))){
            if(Enum.GetName(typeof(GameState), statevalue) == stateName)
            {
                gameState = (GameState)statevalue;
                LogColorText.LogGreen($"[GameModule]LOG>>Set GameState To {gameState}");return;
            }
        }
        LogColorText.LogYellow($"[GameModule]WARNING>>No GameState {stateName},Now {gameState}");
    }    

    void GameObject_Init()
    {
        SetAll(false);
        NowGameObject = "null";
        NextGameObject = "TitleUI";
        SwitchGameObject();
    }
    public void SetNowGameObject(GameObject now)
    {
        NowGameObject = now.name; LogColorText.LogGreen($"[GameModule]LOG>>Set NowGameObject Value {NowGameObject}!");
    }
    public void SetNextGameObject(GameObject next)
    {
        NextGameObject = next.name; LogColorText.LogGreen($"[GameModule]LOG>>Set NextGameObject Value {NextGameObject}!");
    }
    void SetAll(bool state)
    {
        for (int i = 0; i < GameModule_Objects.Length; i++)
        {
            if (GameModule_Objects[i] == null)
            {
                continue;
            }
            else
            {
                GameModule_Objects[i].SetActive(state); LogColorText.LogGreen($"[GameModule]LOG>>Make {GameModule_Objects[i].name} {state}!");
            }
        }
    }
    public void SwitchGameObject()
    {
        bool ischanged = false;
        for (int i = 0; i < GameModule_Objects.Length; i++)
        {
            if (GameModule_Objects[i] == null)
            {
                continue;
            }
            else
            {
                if (GameModule_Objects[i].name == NowGameObject)
                {
                    ischanged = true;
                    GameModule_Objects[i].SetActive(false); LogColorText.LogGreen($"[GameModule]LOG>>Make {NowGameObject} Inactive!");
                }
                if (GameModule_Objects[i].name == NextGameObject)
                {
                    ischanged = true;
                    GameModule_Objects[i].SetActive(true); LogColorText.LogGreen($"[GameModule]LOG>>Make {NextGameObject} Active!");
                }
            }
        }
        if (!ischanged)
        {
            LogColorText.LogYellow($"[GameModule]WARNING>>No GameModule_Objects Changed!");
        }
    }

}
