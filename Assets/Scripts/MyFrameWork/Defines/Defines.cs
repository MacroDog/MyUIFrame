using UnityEngine;
using System.Collections;
namespace MyFrameWork
{
    public delegate void StateChangeEvent(object ui, EnumObjectState old, EnumObjectState newState);
    #region 全局枚举
    public enum EnumObjectState
    {
        None,
        Initial,
        Loading,
        Ready,
        Disable,
        Closing
    }
    public enum EnumUIType:int 
    {
        None=-1,
        TestOne=0,
        
    }
    #endregion
    public class Defines
    {

        public Defines()
        {

        }
    }
}

