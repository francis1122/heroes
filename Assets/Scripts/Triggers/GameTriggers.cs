using System.Runtime.InteropServices;
using UnityEngine;
using Utils;

namespace Triggers
{

    public class GameTriggers : ScriptableObject
    {
        public bool isEndOfTurnTrigger = false;

        public bool checkIfTriggerCanHappen = false;

        public virtual bool CanTriggerFire(StatusIdentifier statusIdentifier = null)
        {
            return true;
        }
        
        public virtual void Trigger(StatusIdentifier statusIdentifier = null, RectTransform transform = null)
        {
            
        }
    }
}
