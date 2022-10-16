using Data;

namespace GameObjects
{
    [System.Serializable]
    public class BuildingObject
    {
        public BuildingData buildingData;

        public int buildingsOwned = 0;
        public int timesPurchased = 0;

        public BuildingObject(BuildingData buildingData)
        {
            this.buildingData = buildingData;
        }

        public bool CheckBuildingRequirement()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {
                int count = GameCenter.instance.playerBuildings
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
                    if (GameCenter.instance.playerResources.CanSubtractResourceBundle(buildingData.costRequirements))
                    {
                        return true;
                    }
            }

            return false;
        }

        public void DestroyBuildingRequirements()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {
                var ownedBuildings = GameCenter.instance.playerBuildings
                    .FindAll(e => e.buildingData == buildingBundle.buildingData);

                int removedBuildings = 0;
                foreach (var ownedBuilding in ownedBuildings)
                {
                    if (removedBuildings < buildingBundle.amount)
                    {
                        ownedBuilding.buildingsOwned--;
                        GameCenter.instance.playerBuildings.Remove(ownedBuilding);
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
                var ownedBuildings = GameCenter.instance.playerBuildings
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

        public void PurchaseBuilding()
        {
            if (CanPurchaseBuilding())
            {
                if (GameCenter.instance.playerResources.SubtractResourceBundle(buildingData.costRequirements))
                {
                    this.timesPurchased++;
                    if (buildingData.addToOwnedBuildings)
                    {
                        this.buildingsOwned++;
                        //GameCenter.instance.buildingsOwned.Add(this);
                    }

                    if (buildingData.buildingAdditionsOnPurchase != null)
                    {
                        foreach (var buildingAdditionData in buildingData.buildingAdditionsOnPurchase)
                        {
                            GameCenter.instance.playerBuildings.Add(
                                new BuildingObject(buildingAdditionData));
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
                        trigger.Trigger();
                    }

                    EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
                    EventManager.TriggerEvent(EventManager.BUILDING_CHANGED);
                }
            }
        }
    }
}