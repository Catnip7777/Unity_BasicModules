using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;


public class InputModule : MonoBehaviour
{
    public static InputModule Instance { get; private set; }
    void Awake()
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
    public enum KeyMode
    {
        None,
        DOWN,
        ING,
        UP
    }


    public static int STATE_NUM= 10;
    public static int ACTION_NUM= 50;
    public static int COMBO_CACHE = 5;
    public static int COMBO_KEYS_NUM = 5;
    public static int COMBINE_CACHE = 5;
    public static int COMBINE_KEYS_NUM = 5;
    public int actionEventNowItem = 0;
    public int comboActionEventNowItem = 0;
    public int combineActionEventNowItem = 0;


    bool isAnyKeyDown=false;
    bool isCombineCacheUsing=false;
    bool isCombineKeyTriggered=false;
    float comboTimerTime=0.0f;
    public float LOST_COMBO_TIME = 1f;
    float combineTimerTime = 0.0f;
    public float COMBINE_RECOVER_TIME = 0.5f;


    KeyCode[] tmpKeySet;
    public int changeKeyFrom = 0;
    public int changeKeyTo = 0; 
 

    public KeyCode[,] keyBind = new KeyCode[STATE_NUM, ACTION_NUM];
    public int[,] keyBindMode = new int[STATE_NUM, ACTION_NUM];
    public int[,] keyBindEventIndex = new int[STATE_NUM, ACTION_NUM];

    public KeyCode[] comboCache= new KeyCode[COMBO_CACHE];
    public KeyCode[,] comboKeys= new KeyCode[STATE_NUM,COMBO_CACHE*COMBO_KEYS_NUM];
    public int[,] comboKeyEventIndex = new int[STATE_NUM, COMBO_KEYS_NUM];

    public KeyCode[] combineCache = new KeyCode[COMBINE_CACHE];
    public KeyCode[,] combineKeys = new KeyCode[STATE_NUM,COMBINE_CACHE*COMBINE_KEYS_NUM];
    public int[,] combineKeyEventIndex = new int[STATE_NUM, COMBINE_KEYS_NUM];

    //actionEvent[keyBindEventIndex[STATE_NUM,配置时的Index(序号)]]，其他模块使用这个字符串即可绑定对应监听事件
    //comboKeyActionEvent[comboKeyEventIndex[STATE_NUM,配置时的Index(序号)]]，其他模块使用这个字符串即可绑定对应监听事件
    //combineKeyActionEvent[combineKeyEventIndex[STATE_NUM,配置时的Index(序号)]]，其他模块使用这个字符串即可绑定对应监听事件
    public String[] actionEvent = new String[STATE_NUM * ACTION_NUM];
    public String[] comboKeyActionEvent = new String[STATE_NUM * COMBO_KEYS_NUM];
    public String[] combineKeyActionEvent = new String[STATE_NUM * COMBINE_KEYS_NUM];
    
    public bool isInitialFinished=false;
    void Start()
    {
        InitialAllActionEvents();

        //按键配置，很简单吧:P
        SetAKey(GameModule.GameState.SCENE, KeyCode.W, KeyMode.DOWN, 0);
        SetAKey(GameModule.GameState.SCENE, KeyCode.A, KeyMode.DOWN, 1);
        SetAKey(GameModule.GameState.SCENE, KeyCode.S, KeyMode.DOWN, 2);
        SetAKey(GameModule.GameState.SCENE, KeyCode.D, KeyMode.DOWN, 3);
        SetAKey(GameModule.GameState.SCENE, KeyCode.Space, KeyMode.DOWN, 4);
        SetAKey(GameModule.GameState.SCENE, KeyCode.W, KeyMode.ING, 5);
        SetAKey(GameModule.GameState.SCENE, KeyCode.A, KeyMode.ING, 6);
        SetAKey(GameModule.GameState.SCENE, KeyCode.S, KeyMode.ING, 7);
        SetAKey(GameModule.GameState.SCENE, KeyCode.D, KeyMode.ING, 8);
        tmpKeySet = new KeyCode[] { KeyCode.Space ,KeyCode.Space,KeyCode.Space,KeyCode.Space,KeyCode.Space};
        SetAComboKey(GameModule.GameState.SCENE, tmpKeySet, 0);
        tmpKeySet = new KeyCode[] { KeyCode.W, KeyCode.A,KeyCode.None,KeyCode.None,KeyCode.None };
        SetAComboKey(GameModule.GameState.SCENE, tmpKeySet, 1);
        tmpKeySet = new KeyCode[] { KeyCode.A, KeyCode.D, KeyCode.None, KeyCode.None, KeyCode.None };
        SetACombineKey(GameModule.GameState.SCENE, tmpKeySet, 0);
        tmpKeySet = new KeyCode[] { KeyCode.A, KeyCode.W, KeyCode.None, KeyCode.None, KeyCode.None };
        SetACombineKey(GameModule.GameState.SCENE, tmpKeySet, 1);

        //以下是改键功能，使用时调用下面的SetChangeKeyTo_And_EnterChangeKeyState()即可，有需要可以配置下面的触发过程
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.W, KeyMode.DOWN, 0);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.A, KeyMode.DOWN, 1);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.S, KeyMode.DOWN, 2);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.D, KeyMode.DOWN, 3);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.Space, KeyMode.DOWN, 4);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.W, KeyMode.ING, 5);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.A, KeyMode.ING, 6);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.S, KeyMode.ING, 7);
        SetAKey(GameModule.GameState.CHANGE_KEY, KeyCode.D, KeyMode.ING, 8);
        tmpKeySet = new KeyCode[] { KeyCode.None,  KeyCode.None,KeyCode.None,KeyCode.Joystick1Button0,KeyCode.None };
        SetAComboKey(GameModule.GameState.CHANGE_KEY, tmpKeySet, 0);
        EventModule.AddListener($"COMBO{comboKeyEventIndex[(int)GameModule.GameState.CHANGE_KEY, 0]}",ChangeKey);
        tmpKeySet = new KeyCode[] { KeyCode.None,  KeyCode.None, KeyCode.None,KeyCode.Space, KeyCode.None };
        SetAComboKey(GameModule.GameState.CHANGE_KEY, tmpKeySet, 1);
        EventModule.AddListener($"COMBO{comboKeyEventIndex[(int)GameModule.GameState.CHANGE_KEY, 1]}", ChangeKey);
        tmpKeySet = new KeyCode[] { KeyCode.None,KeyCode.None, KeyCode.None, KeyCode.Mouse0,  KeyCode.None };
        SetAComboKey(GameModule.GameState.CHANGE_KEY, tmpKeySet, 2);
        EventModule.AddListener($"COMBO{comboKeyEventIndex[(int)GameModule.GameState.CHANGE_KEY, 2]}", ChangeKey);

        //按键数据更新事件，不需要更改
        EventModule.AddListener("IM_KeyDataUpdate", () => { LogColorText.LogGreen("[InputModule]LOG>>IM_KeyDataUpdate!"); });
        isInitialFinished = true;
    }
    void InitialAllActionEvents()
    {
        for(int i = 0; i < actionEvent.Length; i++)
        {
            actionEvent[i] = $"AC{i}";
        }
        for (int i = 0; i < comboKeyActionEvent.Length; i++)
        {
            comboKeyActionEvent[i] = $"COMBO{i}";
        }
        for (int i = 0; i < combineKeyActionEvent.Length; i++)
        {
            combineKeyActionEvent[i] = $"COMBINE{i}";
        }
    }
    void Update()
    {
        isAnyKeyDown = false;
        ClearCombineKeyCache();
        for (int i = 0; i < keyBind.GetLength(1); i++)
        {
            if (Input.GetKeyDown(keyBind[(int)GameModule.Instance.gameState, i]))
            {
                if (keyBindMode[(int)GameModule.Instance.gameState, i] == (int)KeyMode.DOWN)
                {
                    changeKeyFrom = i;ComboKeyCacheRefresh(i);isAnyKeyDown = true;
                    EventModule.TriggerListener(actionEvent[keyBindEventIndex[(int)GameModule.Instance.gameState, i]]);
                }
                    

            }
            else if(Input.GetKey(keyBind[(int)GameModule.Instance.gameState, i]))
            {
                
                if (keyBindMode[(int)GameModule.Instance.gameState, i] == (int)KeyMode.ING)
                {
                    CombineKeyCacheRefresh(keyBind[(int)GameModule.Instance.gameState,i]); 
                    EventModule.TriggerListener(actionEvent[keyBindEventIndex[(int)GameModule.Instance.gameState, i]]);
                }
                    

            }
            else if (Input.GetKeyUp(keyBind[(int)GameModule.Instance.gameState, i]))
            {
                if (keyBindMode[(int)GameModule.Instance.gameState, i] == (int)KeyMode.UP)
                    EventModule.TriggerListener(actionEvent[keyBindEventIndex[(int)GameModule.Instance.gameState, i]]);

            }
        }
        ComboTimerRun(isAnyKeyDown);
        CombineTimerRun(isCombineCacheUsing);
        CombineKeysCheck();
        
    }
    int SetAKey(GameModule.GameState gameState,KeyCode keyCode,KeyMode keyMode,int keyIndex)
    {
        if ((int)gameState >= 0 && (int)gameState < STATE_NUM) {
            if (keyIndex >= 0 && keyIndex < ACTION_NUM)
            { 
                if (actionEventNowItem >= 0 && actionEventNowItem < STATE_NUM * ACTION_NUM)
                {
                    int capturedNum = actionEventNowItem;
                    keyBind[(int)gameState, keyIndex] = keyCode;
                    keyBindMode[(int)gameState, keyIndex] = (int)keyMode;
                    EventModule.AddListener(actionEvent[actionEventNowItem], () => { 
                        LogColorText.LogGreen($"[InputModule]LOG>>KEY   ACTION:{actionEvent[capturedNum]}   GAMESTATE:{gameState}   KEY:{keyCode}   KEYMODE:{keyMode}"); });
                    actionEventNowItem++;
                    keyBindEventIndex[(int)gameState, keyIndex]=actionEventNowItem-1;
                    return actionEventNowItem - 1;
                }
                else
                {
                    LogColorText.LogRed("[InputModule]ERROR>>KEY   ActionEvent out of STATE_NUM * ACTION_NUM limit!");return -1;
                }

            }
            else
            {
                LogColorText.LogRed("[InputModule]ERROR>>KEY   keyIndex out of ACTION_NUM limit!"); return -1;
            }
        }
        else
        {
            LogColorText.LogRed("[InputModule]ERROR>>KEY   gameState out of STATE_NUM limit!"); return -1;
        }
    }
    int SetAComboKey(GameModule.GameState gameState, KeyCode[] comboKeySet, int keyIndex)
    {
        if ((int)gameState >= 0 && (int)gameState < STATE_NUM)
        {
            if (keyIndex >= 0 && keyIndex < COMBO_KEYS_NUM)
            {
                if (comboActionEventNowItem >= 0 && comboActionEventNowItem < STATE_NUM*COMBO_KEYS_NUM)
                {
                    if (comboKeySet.Length == COMBO_CACHE)
                    {
                        int capturedNum = comboActionEventNowItem;
                        string capturedKeyString=string.Join("+", comboKeySet);
                        GameModule.GameState capturedState=gameState;

                        for (int i = 0; i < COMBO_CACHE; i++)
                        {
                            comboKeys[(int)gameState, i + keyIndex * COMBO_CACHE] = comboKeySet[i];
                        }

                        EventModule.AddListener(comboKeyActionEvent[comboActionEventNowItem], () =>
                        {
                            LogColorText.LogGreen($"[InputModule]LOG>>ComboKEY   ACTION:{comboKeyActionEvent[capturedNum]}   GAMESTATE:{capturedState}   KEY:{capturedKeyString}");
                        });
                        comboActionEventNowItem++;
                        comboKeyEventIndex[(int)gameState, keyIndex] = comboActionEventNowItem - 1;
                        return comboActionEventNowItem - 1;
                    }
                    else
                    {
                        LogColorText.LogRed($"[InputModule]ERROR>>ComboKEY   tmpKeySet:{comboKeySet.Length} != COMBO_CACHE:{COMBO_CACHE}!"); return -1;
                    }
                }
                else
                {
                    LogColorText.LogRed("[InputModule]ERROR>>ComboKEY   comboActionEventNowItem out of the length of STATE_NUM*COMBO_KEYS_NUM!"); return -1;
                }
            }
            else
            {
                LogColorText.LogRed("[InputModule]ERROR>>ComboKEY   keyIndex out of COMBO_KEYS_NUM limit!"); return -1;
            }
        }
        else
        {
            LogColorText.LogRed("[InputModule]ERROR>>ComboKEY   gameState out of STATE_NUM limit!"); return -1;
        }
    }
    int SetACombineKey(GameModule.GameState gameState, KeyCode[] combineKeySet, int keyIndex)
    {
        if ((int)gameState >= 0 && (int)gameState < STATE_NUM)
        {
            if (keyIndex >= 0 && keyIndex < COMBINE_KEYS_NUM)
            {
                if (combineActionEventNowItem >= 0 && keyIndex < STATE_NUM*COMBINE_KEYS_NUM)
                {
                    if (combineKeySet.Length == COMBINE_CACHE)
                    {
                        int capturedNum = combineActionEventNowItem;
                        string capturedKeyString = string.Join("+", combineKeySet);

                        for (int i = 0; i < COMBINE_CACHE; i++)
                        {
                            combineKeys[(int)gameState, i + keyIndex * COMBINE_CACHE] = combineKeySet[i];
                        }

                        EventModule.AddListener(combineKeyActionEvent[combineActionEventNowItem], () =>
                        {
                            LogColorText.LogGreen($"[InputModule]LOG>>CombineKEY   ACTION:{combineKeyActionEvent[capturedNum]}   GAMESTATE:{gameState}   KEY:{capturedKeyString}");
                        });
                        combineActionEventNowItem++;
                        combineKeyEventIndex[(int)gameState, keyIndex] = combineActionEventNowItem - 1;
                        return combineActionEventNowItem - 1;
                    }
                    else
                    {
                        LogColorText.LogRed($"[InputModule]ERROR>>CombineKEY   tmpKeySet:{combineKeySet.Length} != COMBINE_CACHE:{COMBINE_CACHE}!"); return -1;
                    }
                }
                else
                {
                    LogColorText.LogRed("[InputModule]ERROR>>CombineKEY   combineActionEventNowItem out of the length of STATE_NUM*COMBINE_KEYS_NUM!"); return -1;
                }
            }
            else
            {
                LogColorText.LogRed("[InputModule]ERROR>>CombineKEY   keyIndex out of COMBINE_KEYS_NUM limit!"); return -1;
            }
        }
        else
        {
            LogColorText.LogRed("[InputModule]ERROR>>CombineKEY   gameState out of STATE_NUM limit!"); return -1;
        }
    }



    void ComboTimerRun(bool isAnyKeyDown)
    {
        if (isAnyKeyDown)
        {
            comboTimerTime = 0; //LogColorText.LogGreen("[InputModule]LOG>>ComboKey Timer Reset!");
        }
        else
        {
            comboTimerTime += Time.deltaTime;
            if (comboTimerTime >= LOST_COMBO_TIME)
            {
                ClearComboKeyCache();
                //LogColorText.LogGreen($"[InputModule]LOG>>Combo Lost,Interval {comboTimerTime}s!");
                comboTimerTime = 0;

            }
        }
    }
    void ComboKeyCacheRefresh(int i)
    {
        for(int j = 0; j < COMBO_CACHE - 1; j++)
        {
            comboCache[COMBO_CACHE - 1-j]=comboCache[COMBO_CACHE - 1-j-1];    
        }
        comboCache[0]=keyBind[(int)GameModule.Instance.gameState,i];

        ComboKeysCheck();
        
    }
    void ComboKeysCheck()
    {
        for (int j = 0; j < COMBO_KEYS_NUM; j++)
        {
            bool isComboKey = true;
            int isAllEmpty=0;
            for (int k = 0; k < COMBO_CACHE; k++)
            {
                if(comboKeys[(int)GameModule.Instance.gameState, k + j * COMBO_CACHE] == KeyCode.None)
                {
                    isAllEmpty += 1;
                    if(isAllEmpty==COMBO_CACHE)isComboKey = false;
                    continue;
                }
                if (comboKeys[(int)GameModule.Instance.gameState, k + j * COMBO_CACHE] == comboCache[COMBO_CACHE - 1 - k])
                {

                }
                else
                {
                    isComboKey = false;
                }

            }
            if (isComboKey)
            {
                EventModule.TriggerListener(comboKeyActionEvent[comboKeyEventIndex[(int)GameModule.Instance.gameState, j]]);
                ClearComboKeyCache();
            }
        }

    }
    void ClearComboKeyCache()
    {
        for (int i = 0; i < COMBO_CACHE; i++)
        {
            comboCache[i] = KeyCode.None;
        }
    }


    bool HaveKeyInCombineCache(KeyCode keyCode)
    {
        for (int j = 0; j < COMBINE_CACHE; j++)
        {
            if (keyCode == combineCache[j])
            {

                return true;

            }

        }
        return false;
    }
    void CombineKeyCacheRefresh(KeyCode keyCode)
    {
        for(int j = 0;j < COMBINE_CACHE; j++)
        {
            if(combineCache[j] == KeyCode.None&&!HaveKeyInCombineCache(keyCode))
            {
                combineCache[j] = keyCode;
                break;
            }

        }
    }
    void ClearCombineKeyCache()
    {
        for (int i = 0;i<COMBINE_CACHE ; i++)
        {
            combineCache[i] = KeyCode.None;
        }
    }
    void CombineTimerRun(bool isCacheUsing)
    {
        if (isCacheUsing&&isCombineKeyTriggered)
        {
            combineTimerTime = 0; //LogColorText.LogGreen("[InputModule]LOG>>CombineKey Timer Reset!");
        }
        else
        {
            combineTimerTime += Time.deltaTime;
            if (combineTimerTime >= COMBINE_RECOVER_TIME)
            {
                isCombineKeyTriggered = false;
                //LogColorText.LogGreen($"[InputModule]LOG>>Combine Recover,Interval {combineTimerTime}s!");
                combineTimerTime = 0;

            }
        }
    }
    void CombineKeysCheck()
    {
        bool isCombineCacheAllEmpty=false;
        for(int k = 0; k < COMBINE_CACHE; k++)
        {
            if (combineCache[k] != KeyCode.None)
            {
                isCombineCacheUsing = true;
                isCombineCacheAllEmpty = true;
                break;
            }

        }
        if (!isCombineCacheAllEmpty)
        {
            isCombineCacheUsing=false;
        }

        if (isCombineKeyTriggered)
        {
            return;
        }
        for(int i = 0; i < COMBINE_KEYS_NUM; i++)
        {
            bool isCombineKey = true;
            int isAllEmpty = 0;
            for (int j = 0; j < COMBINE_CACHE; j++)
            {
                if (combineKeys[(int)GameModule.Instance.gameState, j + i * COMBINE_CACHE] == KeyCode.None)
                {
                    isAllEmpty += 1;
                    if (isAllEmpty == COMBINE_CACHE) isCombineKey = false;
                    continue;
                }
                if (HaveKeyInCombineCache(combineKeys[(int)GameModule.Instance.gameState, j + i * COMBINE_CACHE]))
                {

                }else
                {
                    isCombineKey = false ;
                }
            }
            if (isCombineKey)
            {
                EventModule.TriggerListener(combineKeyActionEvent[combineKeyEventIndex[(int)GameModule.Instance.gameState, i]]);
                isCombineKeyTriggered = true;
                break;
            }
        }
        
    }


    public void SetChangeKeyTo_And_EnterChangeKeyState(string actionEventString)
    {
        bool findActionEvent=false;
        for (int i = 0;i<actionEvent.Length;i++)
        {
            if(actionEvent[i] == actionEventString)
            {
                changeKeyTo = i; findActionEvent = true; LogColorText.LogGreen($"[InputModule]LOG>>Set changeKeyTo {changeKeyTo}!"); break;
            }
        }
        if(!findActionEvent)
        {
            LogColorText.LogYellow($"[InputModule]WARNING>>No actionEvent,changeKeyTo will be 0!");return;
        }
        GameModule.Instance.SetGameStateTo("CHANGE_KEY");
    }
    void ChangeKey()
    {
        KeyCode tmpKeyCode = keyBind[(int)GameModule.GameState.SCENE, changeKeyTo];
        keyBind[(int)GameModule.GameState.SCENE, changeKeyTo] = keyBind[(int)GameModule.GameState.SCENE, changeKeyFrom];
        keyBind[(int)GameModule.GameState.SCENE, changeKeyFrom] = tmpKeyCode;
        GameModule.Instance.SetGameStateTo(Enum.GetName(typeof(GameModule.GameState), (int)GameModule.Instance.lastState));
        LogColorText.LogGreen($"[InputModule]LOG>>Exchange Key{keyBind[(int)GameModule.GameState.SCENE, changeKeyFrom]} & Key{keyBind[(int)GameModule.GameState.SCENE, changeKeyTo]}!");
        EventModule.TriggerListener("IM_KeyDataUpdate");
        EventModule.TriggerListener("UpdateUI");
    }



}

