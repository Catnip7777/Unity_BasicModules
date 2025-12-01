using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIModule : MonoBehaviour
{
    public static int TEXT_NUM = 10;
    public GameObject[] textsGameobject=new GameObject[TEXT_NUM];
    public static int IMAGE_NUM = 10;
    public GameObject[] imagesGameobject=new GameObject[IMAGE_NUM];
    public string InTransAnimationName;
    public static bool StatisticIni=false;



    // Start is called before the first frame update
    private void OnEnable()
    {
        if (StatisticIni)
        {
            UpdateUI();
            if (InTransAnimationName != "") { PerformanceModule.Instance.SetPlayNameAndPlay(InTransAnimationName); }
        }

    }
    void Start()
    {
        EventModule.AddListener("UpdateUI", UpdateUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateUI()
    {
        for (int i = 0; i < TEXT_NUM; i++)
        {
            if (textsGameobject[i] != null)
            {
                textsGameobject[i].GetComponent<TMP_Text>().text =
                    StatisticsModule.Instance.gameDataSet[int.Parse(textsGameobject[i].name.Substring(textsGameobject[i].name.Length - 1,1))]
                    .FindValueByKey(textsGameobject[i].name.Substring(0, textsGameobject[i].name.Length-1));
            }
        }
        for (int i = 0;i < IMAGE_NUM; i++)
        {
            if(imagesGameobject[i] != null)
            {
                imagesGameobject[i].GetComponent<Image>().sprite =Resources.Load<Sprite>(
                    StatisticsModule.Instance.gameDataSet[int.Parse(imagesGameobject[i].name.Substring(imagesGameobject[i].name.Length - 1, 1))]
                    .FindValueByKey(imagesGameobject[i].name.Substring(0, imagesGameobject[i].name.Length - 1))
                    );
            }
        }
        LogColorText.LogGreen($"[UI]LOG>>Updated UI!");
    }
    /*public void ChangeValue()
    {
        StaticsModule.Instance.gameDataSet[StaticsModule.Instance.saveindex - 1].ChangeValueByKey("PlayerProgressPercent", "95%"); LogColorText.LogYellow("[UI]DEBUG>>Changed UI!");
    }*/
    public void SwitchObjectTrigger()
    {
        EventModule.TriggerListener("SwitchGameObject");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
