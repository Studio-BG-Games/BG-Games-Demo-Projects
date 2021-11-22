using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Serializables.Responses
{
    [Serializable]        
    public struct RegisterResponse
    {
        public bool success;
    }
    
    [Serializable]        
    public struct Error
    {
        public string code;
        public string message;
    }
}
