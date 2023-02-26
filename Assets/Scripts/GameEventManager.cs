using System;
using System.Collections.Generic;
using System.ComponentModel;
using Data;
using GameObjects;
using Triggers;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    //
    [SerializeField] public List<EventWrapper> eventList = new();


    [System.Serializable]
    public class EventWrapper
    {
        public BuildingData eventData;

        public int amountOfTimesTriggered = 0;
        public int timeSinceEventTriggered = 0;
        [Header("OneTrigger ")] public int triggerOnTurn = 1;
        [Header("multi trigger")] public bool canTriggerMultipleTimes = false;
        public int triggerXTimes = 1;
        public int turnsTillRepeat = 4;
    }

    void Start()
    {
    }

    public BuildingData EnemyAttackEvent(int currentTurn, int eventLifeSpan, int powerToDefeat, int stabilityLoss)
    {
        BuildingData eventData = ScriptableObject.CreateInstance<BuildingData>();
        eventData.category = BuildingData.BuildingCategory.Event;
        eventData.buildingName = "We're being attacked!";
        eventData.uniqueName = "defend_the_city_" + currentTurn;
        eventData.buildingDetails = "If attack goes uncheck, we'll lose " + stabilityLoss + "stability";
        eventData.destroyOnPurchase = true;
        eventData.addToOwnedBuildings = false;
        eventData.costRequirements = new ResourceBundle();

        eventData.costRequirements.AddResourceData(
            GameCenter.instance.resourceOrganizer.CreateResourceData(5, ResourceType.LinkType.Authority));
        eventData.costRequirements.AddResourceData(
            GameCenter.instance.resourceOrganizer.CreateResourceData(powerToDefeat,
                ResourceType.LinkType.MilitaryPower));

        eventData.eventLifeSpan = eventLifeSpan;
        T_GenerateResources onExpire = ScriptableObject.CreateInstance<T_GenerateResources>();
        onExpire.resourceBundle.AddResourceData(
            GameCenter.instance.resourceOrganizer.CreateResourceData(-stabilityLoss, ResourceType.LinkType.Stability));
        eventData.onExpiredEvent.Add(onExpire);
        return eventData;
    }

    public void GenerateNewEvents(int currentTurn, bool isNewYear)
    {
        if (currentTurn > 12)
        {
            //
            // OnSlaught of attackers
            // every other year
            if (isNewYear)
            {
                int powerToDefeat = ((currentTurn - 12) / 10) + 2;
                BuildingData eventData = EnemyAttackEvent(currentTurn, 6, powerToDefeat, Math.Max(1, currentTurn / 10));
                //EventWrapper newEvent = new EventWrapper();
                GameCenter.instance.playerBuildings.Add(new BuildingObject(eventData));
            }
        }
    }

    public void OnTurnStart(int currentTurn, bool isNewYear)
    {
        GenerateNewEvents(currentTurn, isNewYear);

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