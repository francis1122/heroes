using System.Collections.Generic;
using Data;
using GameObjects;
using UnityEngine;

    public class GameEventManager : MonoBehaviour 
    {
        //
        [SerializeField]
        public  List<EventWrapper> eventList = new ();
        
        [System.Serializable]
        public class EventWrapper
        {
            public BuildingData eventData;
            
            public int amountOfTimesTriggered = 0;
            public int timeSinceEventTriggered = 0;
            [Header("OneTrigger ")]
            public int triggerOnTurn = 1;
            [Header("multi trigger")]
            public bool canTriggerMultipleTimes = false;
            public int triggerXTimes = 1;
            public int turnsTillRepeat = 4;




        }

        void Start()
        {

        }
        
        public void OnTurnStart(int currentTurn, bool isNewYear)
        {
            //eventList
                
            // determine if event should get added to player board
            for (int i = eventList.Count - 1; i >= 0; i--)
            {
                var eventWrapper = eventList[i];
                eventWrapper.timeSinceEventTriggered++;
                
                if (currentTurn == eventWrapper.triggerOnTurn 
                    || (eventWrapper.amountOfTimesTriggered > 1 
                        && eventWrapper.timeSinceEventTriggered >= eventWrapper.turnsTillRepeat))
                {
                    GameCenter.instance.playerBuildings.Add(new BuildingObject(eventWrapper.eventData));
                    eventWrapper.amountOfTimesTriggered++;
                    eventWrapper.timeSinceEventTriggered = 0;
                    if (!eventWrapper.canTriggerMultipleTimes)
                    {
                        eventList.RemoveAt(i);
                    }
                    else if (eventWrapper.amountOfTimesTriggered >= eventWrapper.triggerXTimes)
                    {
                        eventList.RemoveAt(i);
                    }
                }
            }
        }
    }
