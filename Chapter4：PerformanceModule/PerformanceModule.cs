using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Video;

public class PerformanceModule : MonoBehaviour
{
    public static PerformanceModule Instance { get; private set; }
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

    // Start is called before the first frame update
    /*public static VideoClip videoClip;
    public TimelineAsset timelineAsset;
    public RenderTexture renderTexture;
    public GameObject videoDisplayer;*/
    public string PlayName="";
    public GameObject BlackTransmission;
    public GameObject WhiteTransmission;
    void Start()
    {
        EventModule.AddListener("PlayTransAnimation", PlayTransAnimation);
        EventModule.AddListener("PM_TransAnimationFinished", () =>
        {
            LogColorText.LogGreen(
                $"[PerformanceModule]LOG>>TransAnimationFinished!:" +
                $"{PerformanceModule.Instance.PlayName}");
        });
    }




        /*
        EventModule.AddListener("PlayCG", PlayCG);
        EventModule.AddListener("AC5", SkipCG);
        GetComponent<VideoPlayer>().loopPointReached +=FinishedCG;
        EventModule.AddListener("TimeLine", TimeLine);

        videoDisplayer.SetActive(false);*/
    

    public void PlayTransAnimation()
    {
        switch(PlayName)
        {
            case "BlackIn":
                BlackTransmission.GetComponent<Animator>().Play("BlackIn");
                break;
            case "BlackOut":
                BlackTransmission.GetComponent<Animator>().Play("BlackOut");
                break;
            case "WhiteIn":
                WhiteTransmission.GetComponent<Animator>().Play("WhiteIn");
                break;
            case "WhiteOut":
                WhiteTransmission.GetComponent<Animator>().Play("WhiteOut");
                break;
            default:
                LogColorText.LogYellow($"[PerformanceModule]WARNING>> No Match Animation");
                break;
        }
    }
    public void SetPlayName(string playName)
    {
        PlayName = playName;
        LogColorText.LogGreen($"[PerformanceModule]LOG>>Set PlayName to {playName}");
    }
    public void SetPlayNameAndPlay(string playName)
    {
        PlayName = playName;
        LogColorText.LogGreen($"[PerformanceModule]LOG>>Set PlayName to {playName}");
        PlayTransAnimation();
    }
    /*
    public void PlayCG()
    {
        GetComponent<VideoPlayer>().clip=videoClip;
        videoDisplayer.SetActive(true);
        GetComponent<VideoPlayer>().Play();Debug.Log("PlayCG");
    }
    public void SkipCG()
    {
        videoDisplayer.SetActive(false);
        GetComponent<VideoPlayer>().Stop(); Debug.Log("SkipCG");
        EventModule.TriggerListener("GameContinue");
    }
    public void FinishedCG(VideoPlayer videoPlayer)
    {
        videoDisplayer.SetActive(false); Debug.Log("FinishedCG");
        EventModule.TriggerListener("GameContinue");
    }
    public void TimeLine()
    {
        GetComponent<PlayableDirector>().Play(); Debug.Log("TimeLine");
        EventModule.TriggerListener("GameContinue");
    }*/
}
