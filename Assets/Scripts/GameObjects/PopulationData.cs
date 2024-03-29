﻿namespace Data
{
    [System.Serializable]
    public class PopulationData
    { 
        public int amount;
        public int activeAmount;
        public int annualRecruitLimit;
        public int annualRecruitsAvailable;
        public PopulationType type;
       
        public PopulationData(int amount, int activeAmount, PopulationType resourceType)
        {
            this.amount = amount;
            this.activeAmount = activeAmount;
            this.type = resourceType;

        }
        public PopulationData(PopulationData other)
        {
            this.amount = other.amount;
            this.activeAmount = other.activeAmount;
            this.type = other.type;
        }

        public string getShortString()
        {
            return type.name + " " + amount + "\n";
        }
        

        // getting called by gamecenter/ player stuff
        public void ResetActivePopulation()
        {
            activeAmount = amount;
        }
        
        public bool CanSubtractResource(PopulationData subtractionAmount)
        {
            
            // test if this resource matches type and has proper amount to subtract 
            if ((this.amount >= subtractionAmount.amount  ) 
                && this.type == subtractionAmount.type 
                && activeAmount >= subtractionAmount.activeAmount)
                return true;
            return false;
        }

        public void SubtractResource(PopulationData subtractAmount)
        {
            this.amount -= subtractAmount.amount;
            this.activeAmount -= subtractAmount.activeAmount;
        }

        /*
        public bool CanAddResource(ResourceData addResourceData, bool canPartiallyAdd)
        {
            if (this.type != addResourceData.type) return false;
            //test if this resource matches type and has proper amount to subtract
            if (canPartiallyAdd)
            {
                return this.amount < this.maxAmount;
                
            }
            else
            {
                int spaceAvailable = this.maxAmount - this.amount;
                return addResourceData.amount <= spaceAvailable;
                
            }
 
        }
        */
       
        // returns the amount that was added
        public PopulationData AddResource(PopulationData addResourceData)
        {
            this.amount += addResourceData.amount;
            this.activeAmount += addResourceData.activeAmount;
            this.annualRecruitsAvailable += addResourceData.annualRecruitsAvailable;
            this.annualRecruitLimit += addResourceData.annualRecruitLimit;
            //if (this.activeAmount > amount) activeAmount = amount;
            return addResourceData;
            /*int spaceAvailable = this.maxAmount - this.amount;
            int amountToAdd = Math.Min(spaceAvailable, addResourceData.amount);
            this.amount += amountToAdd;
            ResourceData subtracted = new ResourceData(addResourceData);
            subtracted.amount = amountToAdd; 
            return subtracted;*/
        }

        public bool CanTransferResource(PopulationData objectToTransfer)
        {
            //check if types match and limit doesn't get reached
            //  if (type == objectToTransfer.type && amount != maxAmount)
            if (type == objectToTransfer.type)
            {
                return true;
            }
            return false;
        }

        public void TransferResource(PopulationData objectToTransfer)
        {
            //double check type
            //double check size of transfer and limit
            // int maxTransfer = this.maxAmount - this.amount;
            // int amountToTransfer = Math.Min(maxTransfer, objectToTransfer.amount);
            this.amount += objectToTransfer.amount;
            objectToTransfer.amount -= objectToTransfer.amount;
        }
    }
}
