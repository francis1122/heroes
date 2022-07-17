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
        
        public void PurchaseBuilding()
        {
            if (GameCenter.instance.playerResources.CanSubtractResource(buildingData.costRequirements))
            {
                GameCenter.instance.playerResources.SubtractResource(buildingData.costRequirements);
                if (buildingData.addToOwnedBuildings)
                {
                    GameCenter.instance.buildingsOwned.Add(this);
                }

                if (buildingData.buildingAdditionsOnPurchase != null)
                {
                    foreach (var buildingAdditionData in buildingData.buildingAdditionsOnPurchase)
                    {
                        GameCenter.instance.purchasableBuildings.Add(new BuildingObject(buildingAdditionData));
                    }
                }

                if (buildingData.removeFromPurchasableOnPurchase)
                {
                    GameCenter.instance.purchasableBuildings.Remove(this);
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