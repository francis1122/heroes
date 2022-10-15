using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class StatusIdentifier
    {
        [SerializeField]
        public List<String> nameList = new();
        public StatusIdentifier()
        {
        }
        
        public StatusIdentifier( List<String> names)
        {
            nameList = names;
        }
    }
}