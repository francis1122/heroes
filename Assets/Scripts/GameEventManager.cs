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
            public bool canTriggerMultipleTimes = false;

            public int triggerOnTurn = 1;
            public int timeSinceEventTriggered = 0;


            
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
                
                if (currentTurn == eventWrapper.triggerOnTurn)
                {
                    GameCenter.instance.playerBuildings.Add(new BuildingObject(eventWrapper.eventData));
                    eventWrapper.amountOfTimesTriggered++;
                    eventWrapper.timeSinceEventTriggered = 0;
                    if (!eventWrapper.canTriggerMultipleTimes)
                    {
                        eventList.RemoveAt(i);
                    }
                }
            }
        }
    }
