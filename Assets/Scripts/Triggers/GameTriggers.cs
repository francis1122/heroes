using System.Runtime.InteropServices;
using UnityEngine;
using Utils;

namespace Triggers
{

    public class GameTriggers : ScriptableObject
    {
        public bool isEndOfTurnTrigger = false;
        public virtual void Trigger(StatusIdentifier statusIdentifier = null)
        {
            
        }
    }
}
