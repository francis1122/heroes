using Data;
using Triggers;
using UnityEngine;

namespace GameObjects
{
    [System.Serializable]
    public class BuildingObject
    {
        public BuildingData buildingData;

        public int buildingsOwned = 0;
        public int timesPurchased = 0;
        public int timesPurchasedThisTurn = 0;
        public int timesPurchasedThisYear = 0;

        //public bool is
        public bool hasBeenSeen = false;

        public int eventLifeSpanLeft;

        public BuildingObject(BuildingData buildingData)
        {
            this.buildingData = buildingData;
            eventLifeSpanLeft = buildingData.eventLifeSpan;
        }

        public bool CheckBuildingRequirement()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {
                int count = GameCenter.instance.GetPlayerBuildings()
                    .FindAll(e => e.buildingData == buildingBundle.buildingData && e.buildingsOwned > 0).Count;

                if (count < buildingBundle.amount)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CanPurchaseBuilding()
        {
            if (CheckBuildingRequirement())
            {
                if (buildingsOwned == 0 || buildingData.repeatablePurchase)
                    if (GameCenter.instance.playerResources.CanSubtractResourceBundle(buildingData.ScaledResourceBundle()))
                    {
                        // check if triggers can fire
                        foreach (GameTriggers trigger in buildingData.onPurchaseTrigger)
                        {
                            if (trigger.checkIfTriggerCanHappen && !trigger.CanTriggerFire(null))
                            {
                                return false;
                            }
                        }
                        // &&
                        // GameCenter.instance.playerResources.CanAddResourceBundle(buildingData.ScaledResourceBundle(), false)
                        
                        return true;
                    }
            }

            return false;
        }

        public void DestroyBuildingRequirements()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {
                var ownedBuildings = GameCenter.instance.GetPlayerBuildings()
                    .FindAll(e => e.buildingData == buildingBundle.buildingData);

                int removedBuildings = 0;
                foreach (var ownedBuilding in ownedBuildings)
                {
                    if (removedBuildings < buildingBundle.amount)
                    {
                        //ownedBuilding.buildingsOwned--;
                        GameCenter.instance.GetPlayerBuildings().Remove(ownedBuilding);
                        //removedBuildings++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        public void SellBuildingRequirements()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {
                var ownedBuildings = GameCenter.instance.GetPlayerBuildings()
                    .FindAll(e => e.buildingData == buildingBundle.buildingData);

                int removedBuildings = 0;
                foreach (var ownedBuilding in ownedBuildings)
                {
                    if (removedBuildings < buildingBundle.amount)
                    {
                        ownedBuilding.buildingsOwned--;
                        //GameCenter.instance.playerBuildings.Remove(ownedBuliding);
                        removedBuildings++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void PurchaseBuilding(RectTransform transform = null)
        {
            if (CanPurchaseBuilding())
            {
                if (GameCenter.instance.playerResources.SubtractResourceBundle(buildingData.ScaledResourceBundle()))
                {
                    timesPurchased++;
                    timesPurchasedThisTurn++;
                    timesPurchasedThisYear++;
                    GameCenter.instance.prestigeScore += buildingData.prestigeGainedOnPurchase;
                    if (buildingData.addToOwnedBuildings)
                    {
                        this.buildingsOwned++;
                        //GameCenter.instance.buildingsOwned.Add(this);
                    }

                    if (buildingData.buildingAdditionsOnPurchase != null)
                    {
                        foreach (var buildingAdditionData in buildingData.buildingAdditionsOnPurchase)
                        {
                            if (!GameCenter.instance.GetPlayerBuildings().Exists(element =>
                                    element.buildingData.uniqueName == buildingAdditionData.uniqueName))
                            {
                                GameCenter.instance.AddBuildingToPlayer(new BuildingObject(buildingAdditionData));
                            }
                        }
                    }

                    /*if (buildingData.removeFromPurchasableOnPurchase)
                    {
                        GameCenter.instance.playerBuildings.Remove(this);
                    }*/

                    if (buildingData.destroyRequiredBuildings)
                    {
                        DestroyBuildingRequirements();
                    }

                    if (buildingData.sellRequiredBuildings)
                    {
                        SellBuildingRequirements(); 
                    }

                    foreach (var trigger in buildingData.onPurchaseTrigger)
                    {
                        trigger.Trigger(null, transform);
                    }

                    if (buildingData.destroyOnPurchase)
                    {
                        GameCenter.instance.GetPlayerBuildings().Remove(this);
                        
                    }

                    EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
                    EventManager.TriggerEvent(EventManager.BUILDING_CHANGED);
                }
            }
        }
    }
}