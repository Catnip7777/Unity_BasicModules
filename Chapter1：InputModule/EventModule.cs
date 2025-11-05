using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public static class EventModule
{
    private static Dictionary<string,Action> EventDictionary=new Dictionary<string,Action>();

    public static void AddListener(string name,Action action)
    {
        if (!EventDictionary.ContainsKey(name))
        {
            EventDictionary.Add(name, null);
        }     
        EventDictionary[name] += action;
    }
    public static void RemoveListener(string name, Action action)
    {
        if (EventDictionary.ContainsKey(name))
        {
            EventDictionary[name] -= action;
        }
        if (EventDictionary[name]==null)
        {
            EventDictionary.Remove(name);
        }
        
    }
    public static void TriggerListener(string name)
    {
        if (EventDictionary.ContainsKey(name))
        {
            if(EventDictionary[name] != null)
            {
                EventDictionary[name].Invoke();
            }
        }
    }

}
