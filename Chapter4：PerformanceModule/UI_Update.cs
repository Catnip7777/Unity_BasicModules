using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_Update : MonoBehaviour
{
    public static int TEXT_NUM = 10;
    public GameObject[] textsGameobject=new GameObject[TEXT_NUM];
    public int[] savechoice = new int[TEXT_NUM];
    public string InTransAnimationName;
    // Start is called before the first frame update
    private void OnEnable()
    {
        UpdateUI();
        PerformanceModule.Instance.SetPlayNameAndPlay(InTransAnimationName);
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
                    StaticsModule.Instance.gameDataSet[savechoice[i]].FindValueByKey(textsGameobject[i].name.Substring(0, textsGameobject[i].name.Length-1));
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
