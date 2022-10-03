using System;
using UnityEngine;
using UnityEditor;
using Utils;

namespace Data
{
   // [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
   [System.Serializable]
   public class ResourceData
   {
       public int amount;
       public ResourceType type;
       
       public ResourceData(int amount, ResourceType resourceType)
       {
           this.amount = amount;
           this.type = resourceType;

       }
       public ResourceData(ResourceData other)
       {
           this.amount = other.amount;
           this.type = other.type;
       }

       public string getShortString()
       {
           return type.name + " " + amount + "\n";
       }
       
       public bool CanSubtractResource(ResourceData subtractionAmount)
       {
            // test if this resource matches type and has proper amount to subtract 
            if ((this.amount >= subtractionAmount.amount || this.type.amountCanBeNegative ) && this.type == subtractionAmount.type)
                return true;
            return false;
       }

       public void SubtractResource(ResourceData subtractAmount)
       {
           this.amount -= subtractAmount.amount;
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
       public ResourceData AddResource(ResourceData addResourceData)
       {
           this.amount += addResourceData.amount;
           return addResourceData;
           /*int spaceAvailable = this.maxAmount - this.amount;
           int amountToAdd = Math.Min(spaceAvailable, addResourceData.amount);
           this.amount += amountToAdd;
           ResourceData subtracted = new ResourceData(addResourceData);
           subtracted.amount = amountToAdd; 
           return subtracted;*/
       }

       public bool CanTransferResource(ResourceData objectToTransfer)
       {
           //check if types match and limit doesn't get reached
         //  if (type == objectToTransfer.type && amount != maxAmount)
          if (type == objectToTransfer.type)
           {
               return true;
           }
           return false;
       }

       public void TransferResource(ResourceData objectToTransfer)
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