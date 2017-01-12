using UnityEngine;
using System.Collections;
using System;

namespace MyFrameWork
{
    public class SingletonException : Exception
    {
        public SingletonException(string msg):base(msg)
        {

        }
       
    }
}

