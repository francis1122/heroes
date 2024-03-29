﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    private Dictionary <string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static string EVENT_END_TURN = "EVENT_END_TURN";
    public static string EVENT_END_YEAR = "EVENT_END_YEAR";
    
    public static string RESOURCES_CHANGED = "RESOURCES_CHANGED";
    public static string BUILDING_CHANGED = "BUILDING_CHANGED";
    public static string EVENT_START_NIGHT = "EVENT_START_NIGHT";
    public static string EVENT_BUILDING_ADDED = "EVENT_BUILDING_ADDED";
    //public static string EVENT_EVENT_ADDED = "EVENT_EVENT_ADDED";
    
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init (); 
                }
            }

            return eventManager;
        }
    }

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener (listener);
        } 
        else
        {
            thisEvent = new UnityEvent ();
            thisEvent.AddListener (listener);
            instance.eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Invoke ();
        }
    }
}