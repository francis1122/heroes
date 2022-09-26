using UnityEngine;

namespace Triggers
{

    public class GameTriggers : ScriptableObject
    {
        public bool isEndOfTurnTrigger = false;
        public virtual void Trigger()
        {
            
        }
    }
}
