using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputModule : MonoBehaviour
{

    public static int STATE_NUM= 10;
    public static int ACTION_NUM= 50;
    public static int KEY_CACHE = 5;
    public static int MULTIKEYS_NUM = 5;
    public static int COMBOKEYS_NUM = 5;
    public static int COMBO_CACHE = 5;

    int changekeyto = 0; public static int keyindexnow = 0;

    public static KeyCode[,] keybind = new KeyCode[STATE_NUM, ACTION_NUM];
    private int[,] keybindmode = new int[STATE_NUM, ACTION_NUM];
    public static KeyCode[] keycache= new KeyCode[KEY_CACHE];
    public KeyCode[,] multikeys= new KeyCode[MULTIKEYS_NUM,KEY_CACHE];
    public bool[,] multikeysenable = new bool[MULTIKEYS_NUM, KEY_CACHE];
    public int[] multikeyslength= new int[MULTIKEYS_NUM];
    public KeyCode[] combokeys = new KeyCode[COMBOKEYS_NUM];
    public int[] combokeyslength = new int[COMBOKEYS_NUM];
    public KeyCode[] combocache = new KeyCode[COMBO_CACHE];
    public static String[] actionevent = new String[10] 
    { "AC1", "AC2", "AC3", "AC4", "AC5","AC6", "AC7", "AC8", "AC9", "AC10" };
    public static String[] multikeyactionevent = new String[5]
    { "MAC1", "MAC2", "MAC3", "MAC4", "MAC5" };
    public static String[] combokeyactionevent = new String[5]
    { "CAC1", "CAC2", "CAC3", "CAC4", "CAC5" };

    void Start()
    {

        keybind[0, 0] = KeyCode.A;
        keybind[0, 1] = KeyCode.D; 
        keybind[0, 2] = KeyCode.W;
        keybind[0, 3] = KeyCode.S;
        keybind[0, 4] = KeyCode.Q;
        keybind[0, 5] = KeyCode.W;
        keybind[0, 6] = KeyCode.E;
        keybind[0, 7] = KeyCode.D;
        keybind[0, 8] = KeyCode.F;
        keybind[0, 9] = KeyCode.Space;
        keybindmode[0, 0] = 1;
        keybindmode[0, 1] = 1;
        keybindmode[0, 2] = 1;
        keybindmode[0, 3] = 1;
        keybindmode[0, 4] = 1;
        keybindmode[0, 5] = 3;
        keybindmode[0, 6] = 3;
        keybindmode[0, 7] = 2;
        keybindmode[0, 8] = 1;
        keybindmode[0, 9] = 1;
        EventModule.AddListener(actionevent[0], Action1);
        EventModule.AddListener(actionevent[1], Action2);
        EventModule.AddListener(actionevent[2], Action3);
        EventModule.AddListener(actionevent[3], Action4);
        EventModule.AddListener(actionevent[4], Action5);
        EventModule.AddListener(actionevent[5], Action6);
        EventModule.AddListener(actionevent[6], Action7);
        EventModule.AddListener(actionevent[7], Action8);
        EventModule.AddListener(actionevent[8], Action9);
        EventModule.AddListener(actionevent[9], Action10);
        
        multikeys[0, 0] = KeyCode.Space;
        multikeys[0, 1] = KeyCode.Space;
        multikeys[0, 2] = KeyCode.Space;
        multikeys[0, 3] = KeyCode.Space;
        multikeys[0, 4] = KeyCode.Space;
        multikeysenable[0, 0] = true;
        multikeysenable[0, 1] = true;
        multikeysenable[0, 2] = true;
        multikeysenable[0, 3] = true;
        multikeysenable[0, 4] = true;
        EventModule.AddListener(multikeyactionevent[0], MultiAction1);
        /*
        multikeyslength[0] = 5;
        multikeys[1, 1] = KeyCode.A;
        multikeysenable[1, 1] = true;
        multikeyslength[1] = 2;
        EventModule.AddListener(multikeyactionevent[1], ChangedKeyByA_X);

        combokeys[0] = KeyCode.W;
        combokeys[1] = KeyCode.E;
        combokeyslength[0] = 2;
        EventModule.AddListener(combokeyactionevent[0], ComboAction1);*/
    }

    // Update is called once per frame
    void Update()
    {
        ClearComboCache();
        for (int i = 0; i < keybind.GetLength(1); i++)
        {
            if (Input.GetKeyDown(keybind[GameModule.gameState, i]))
            {
                keyindexnow = i;
                if (keybindmode[GameModule.gameState, i] == 1)
                {
                    KeyCacheRefresh(i);
                    EventModule.TriggerListener(actionevent[i]);
                }
                    

            }
            else if(Input.GetKey(keybind[GameModule.gameState, i]))
            {
                
                if (keybindmode[GameModule.gameState, i] == 2)
                {
                    ComboCacheRefresh(keybind[GameModule.gameState,i]); Debug.Log($"i id {i}!");
                    EventModule.TriggerListener(actionevent[i]);
                }
                    

            }
            else if (Input.GetKeyUp(keybind[GameModule.gameState, i]))
            {
                if (keybindmode[GameModule.gameState, i] == 3)
                    EventModule.TriggerListener(actionevent[i]);

            }
        }

        ComboKeysCheck();
        
    }
    public void KeyCacheRefresh(int i)
    {
        for(int j = 0; j < KEY_CACHE - 1; j++)
        {
            keycache[KEY_CACHE -1-j]=keycache[KEY_CACHE - 1-j-1];    
        }
        keycache[0]=keybind[GameModule.gameState,i];

        MultiKeysCheck();
        
    }
    public void MultiKeysCheck()
    {
        for (int j = 0; j < MULTIKEYS_NUM;j++)
        {
            if (multikeyslength[j] != 0)
            {
                int checkresult = 0;
                for (int i = 0; i < multikeyslength[j]; i++)
                {
                    if (multikeysenable[j, i] == false)
                        continue;
                    if (multikeys[j, i] != keycache[i])
                        checkresult = 1;

                }
                if (checkresult == 0)
                {
                    EventModule.TriggerListener(multikeyactionevent[j]);
                    ClearKeyCache();
                }

            }
        }
        
    }
    public void ClearKeyCache()
    {
        for(int i = 0;i < KEY_CACHE; i++)
        {
            keycache[i] = KeyCode.None;
        } 
    }
    public void ComboCacheRefresh(KeyCode keyCode)
    {
        for(int j = 0;j < COMBO_CACHE; j++)
        {
            if(combocache[j] == KeyCode.None&&!HaveKeyInComboCache(keyCode))
            {
                combocache[j] = keyCode;
                break;
            }

        }
    }
    public bool HaveKeyInComboCache(KeyCode keyCode)
    {
        for (int j = 0; j < COMBO_CACHE; j++)
        {
            if (keyCode == combocache[j])
            {

                Debug.Log($"true {keyCode}=={combocache[j]}"); return true;
                
            }
                
        }
        Debug.Log("false");
        return false;
    }
    public void ClearComboCache()
    {
        for (int i = 0;i<COMBO_CACHE ; i++)
        {
            combocache[i] = KeyCode.None;
        }
    }
    public void ComboKeysCheck()
    {
        bool combocacheusing = false;
        for(int k = 0; k < COMBO_CACHE; k++)
        {
            if (combocache[k] != KeyCode.None)
            {
                combocacheusing = true;
            }

        }
        if (!combocacheusing)
        {
            return;
        }
        for(int i = 0; i < COMBOKEYS_NUM; i++)
        {
            int checkresult = 0;
            if (combokeyslength[i] != 0)
            {
                for(int j = 0; j < combokeyslength[i]; j++)
                {
                    if (!HaveKeyInComboCache(combokeys[j]))
                    {
                        checkresult = 1;
                    }
                }
                if (checkresult == 0)
                {
                    EventModule.TriggerListener(combokeyactionevent[i]);
                }
            }
        }
        ClearComboCache();
    }
    public void ChangedKeyByA_X()
    {
        Debug.Log("¿ªÊ¼¸Ä¼ü");
        KeyCode keyCodetmp;
        keyCodetmp = keybind[GameModule.gameState, changekeyto];
        keybind[GameModule.gameState, changekeyto] = keybind[GameModule.gameState, keyindexnow];
        keybind[GameModule.gameState, keyindexnow] = keyCodetmp;
    }

    #region forDebug functions 
    public void Action1()
    {
        
        Debug.Log("1");//EventModule.TriggerListener("PlayCG");
    }
    public void Action2()
    {
        Debug.Log("2"); //EventModule.TriggerListener("SkipCG");
    }
    public void Action3()
    {
        Debug.Log("3");
    }
    public void Action4()
    {
        Debug.Log("4");
    }
    public void Action5()
    {
        Debug.Log("5");
    }
    public void Action6()
    {

        Debug.Log("6");
    }
    public void Action7()
    {
        Debug.Log("7");
    }
    public void Action8()
    {
        Debug.Log("8");
    }
    public void Action9()
    {
        Debug.Log("9");
    }
    public void Action10()
    {
        Debug.Log("10");
    }
    public void MultiAction1()
    {
        Debug.Log("superjump");
    }
    public void ComboAction1()
    {
        Debug.Log("shiftwaaaa");
    }
    #endregion


}

