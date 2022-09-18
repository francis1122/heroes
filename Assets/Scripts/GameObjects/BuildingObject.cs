using Data;

namespace GameObjects
{
    [System.Serializable]
    public class BuildingObject
    {
        public BuildingData buildingData;
        
        public int timesPurchased = 0;

        public BuildingObject(BuildingData buildingData)
        {
            this.buildingData = buildingData;
        }

        public bool CheckBuildingRequirement()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {

                int count = GameCenter.instance.buildingsOwned
                    .FindAll(e => e.buildingData == buildingBundle.buildingData).Count;

                if (count < buildingBundle.amount)
                {
                    return false;
                }
            }
            return true;
        }

        public void DestroyBuildingRequirements()
        {
            foreach (BuildingData.BuildingBundle buildingBundle in buildingData.buildingRequirement)
            {
                var ownedBulidings = GameCenter.instance.buildingsOwned
                    .FindAll(e => e.buildingData == buildingBundle.buildingData);

                int removedBuildings = 0;
                foreach (var ownedBuliding in ownedBulidings)
                {
                    if (removedBuildings < buildingBundle.amount)
                    {
                        GameCenter.instance.buildingsOwned.Remove(ownedBuliding);
                        removedBuildings++;
                    }else {
                        break;
                    }
                }
            }
        }
        
        public void PurchaseBuilding()
        {
            if (CheckBuildingRequirement())
            {
                if (GameCenter.instance.playerResources.CanSubtractResourceData(
                        GameCenter.instance.resourceOrganizer.CreateResourceData(10, ResourceType.LinkType.Authority)))
                {

                    if (GameCenter.instance.playerResources.CanSubtractResourceBundle(buildingData.costRequirements))
                    {
                        if (GameCenter.instance.playerResources.SubtractResourceBundle(buildingData.costRequirements))
                        {
                            if (buildingData.addToOwnedBuildings)
                            {
                                GameCenter.instance.buildingsOwned.Add(this);
                            }

                            if (buildingData.buildingAdditionsOnPurchase != null)
                            {
                                foreach (var buildingAdditionData in buildingData.buildingAdditionsOnPurchase)
                                {
                                    GameCenter.instance.purchasableBuildings.Add(
                                        new BuildingObject(buildingAdditionData));
                                }
                            }

                            if (buildingData.removeFromPurchasableOnPurchase)
                            {
                                GameCenter.instance.purchasableBuildings.Remove(this);
                            }

                            if (buildingData.destroyRequiredBuildings)
                            {
                                DestroyBuildingRequirements();
                            }

                            foreach (var trigger in buildingData.onPurchaseTrigger)
                            {
                                trigger.Trigger();
                                GameCenter.instance.playerResources.SubtractResourceData(
                                    GameCenter.instance.resourceOrganizer.CreateResourceData(10,
                                        ResourceType.LinkType.Authority));
                            }

                            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
                            EventManager.TriggerEvent(EventManager.BUILDING_CHANGED);
                        }

                    }
                }
            }
        }
        
    }
}