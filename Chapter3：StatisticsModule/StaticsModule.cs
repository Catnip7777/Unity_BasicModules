using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StaticsModule : MonoBehaviour
{
    public static StaticsModule Instance { get; private set; }
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




    // Start is called before the first frame update
    public static int SAVE_NUM = 3;
    public int saveindex = 0;
    public string savepath;
    public GameData[] gameDataSet=new GameData[SAVE_NUM];



    void Start()
    {
        savepath = Application.persistentDataPath;
        LoadSave();

        for (int i = 0; i < SAVE_NUM; i++)
        {
            int capturedint = i;
            EventModule.AddListener($"ChooseSave{i}", () =>
            { 
                saveindex = capturedint; LogColorText.LogGreen($"[StaticsModule]LOG>>Choosed Save{capturedint}!");
            });

        }
        EventModule.AddListener("SaveSave", SaveSave);
        EventModule.AddListener("DeleteSave", DeleteSave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChooseSave1Trigger()
    {
        EventModule.TriggerListener("ChooseSave0");
    }
    public void ChooseSave2Trigger()
    {
        EventModule.TriggerListener("ChooseSave1");
    }
    public void ChooseSave3Trigger()
    {
        EventModule.TriggerListener("ChooseSave2");
    }
    void LoadSave()
    {
        for (int i = 0; i < SAVE_NUM; i++)
        {
            if (!File.Exists(savepath + $"/save{i}.json"))
            {
                GameData data = new GameData();
                gameDataSet[i] = data;
                LogColorText.LogYellow($"[StaticsModule]WARNING>>No Save{i}!Have Create An Empty One!");
            }
            else
            {
                string jsondata = File.ReadAllText(savepath + $"/save{i}.json");
                gameDataSet[i] = JsonUtility.FromJson<GameData>(jsondata);
                LogColorText.LogGreen($"[StaticsModule]LOG>>Loaded Save{i}!\n{jsondata}");
            }
        }
    }
    public void SaveSave()
    {
        string jsondata = JsonUtility.ToJson(gameDataSet[saveindex]);
        File.WriteAllText(savepath+$"/save{saveindex}.json", jsondata);
        LogColorText.LogGreen($"[StaticsModule]LOG>>Save{saveindex} Saved!");
    }
    public void DeleteSave()
    {
        GameData gameData=new GameData();
        gameDataSet[saveindex] = gameData;
        File.Delete(savepath + $"/save{saveindex}.json");
        EventModule.TriggerListener("UpdateUI");
        LogColorText.LogGreen($"[StaticsModule]LOG>>Save{saveindex} Deleted!");
        
    }
}
public class GameData
{
    static int SAVEBASICDATA_NUM = 10;
    public int SaveBasicData_ItemNum = 0;
    public string[] SaveBasicDataKey = new string[SAVEBASICDATA_NUM];
    public string[] SaveBasicDataValue = new string[SAVEBASICDATA_NUM];
    public GameData()
    {
        AddItemToMyDictionary("SaveDescription", "EmptySave");
        AddItemToMyDictionary("SaveProgress", "0%");
        AddItemToMyDictionary("PlayerPlayMinutes", "0");
        AddItemToMyDictionary("PlayerPlaySeconds", "0");


    }
    void AddItemToMyDictionary(string key, string value)
    {
        if (SAVEBASICDATA_NUM > SaveBasicData_ItemNum)
        {
            SaveBasicDataKey[SaveBasicData_ItemNum] = key;
            SaveBasicDataValue[SaveBasicData_ItemNum] = value;
            SaveBasicData_ItemNum++;
        }
        else
        {
            LogColorText.LogRed("[StaticsModule]ERROR>>SaveBasicData is full!");
        }
    }
    public string FindValueByKey(string key)
    {
        for (int i = 0; i < SaveBasicData_ItemNum; i++)
        {
            if (SaveBasicDataKey[i] == key) return SaveBasicDataValue[i];
        }
        LogColorText.LogRed($"[StaticsModule]ERROR>>No Item {key}!");
        return "";
    }
    public void ChangeValueByKey(string key, string value)
    {
        bool isfinditem = false;
        for (int i = 0; i < SaveBasicData_ItemNum; i++)
        {
            if (SaveBasicDataKey[i] == key)
            {
                SaveBasicDataValue[i] = value;
                EventModule.TriggerListener("UpdateUI");
                isfinditem = true;
            }
        }
        if (!isfinditem)
        {
            LogColorText.LogRed($"[StaticsModule]ERROR>>No Item {key}!");
        }
    }
}
