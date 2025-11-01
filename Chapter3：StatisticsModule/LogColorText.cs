using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogColorText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void LogRed(string message)
    {
        Debug.Log($"<color=red>{message}</color>");
    }

    public static void LogGreen(string message)
    {
        Debug.Log($"<color=green>{message}</color>");
    }

    public static void LogYellow(string message)
    {
        Debug.Log($"<color=yellow>{message}</color>");
    }

    public static void LogBlue(string message)
    {
        Debug.Log($"<color=blue>{message}</color>");
    }

    public static void Log(string message, string color)
    {
        Debug.Log($"<color={color}>{message}</color>");
    }
}

