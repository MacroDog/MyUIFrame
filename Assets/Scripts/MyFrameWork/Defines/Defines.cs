using UnityEngine;
using System.Collections;
using System;
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
    
    public enum EnumUIType : int
    {
        None = -1,
        TestOne = 0,
        TestTwo=1,

    }
    #endregion
    public class UIPathDefines
    {
        /// <summary>
        /// UI预设
        /// </summary>
        public const string UI_PREFAB = "UIPrefabs/";
        /// <summary>
        /// UI 控件预设
        /// </summary>
        public const string UI_CONTROLS_PAEFAB = "UIPrefab/Control/";
        /// <summary>
        /// UI子页面预设
        /// </summary>
        public const string UI_SUBUI_PREFAB = "UIPrefab/Subui/";
        /// <summary>
        /// UI Icon预设
        /// </summary>
        public const string UI_ICON_PATH = "UI/Icon/";
        public static string GetPrefabsPathByType(EnumUIType _uiTypes)
        {
            string path = string.Empty;
            switch (_uiTypes)
            {
                case EnumUIType.None:
                    
                    break;
                case EnumUIType.TestOne:
                    path = UI_PREFAB + "TestOne";
                    break;
                case EnumUIType.TestTwo:
                    path = UI_PREFAB + "TestTwo";
                    break;

                default:
                    Debug.Log("Not exist " + _uiTypes.ToString() + "UIType !");
                    break;
            }
            return path;
        }
       public static  System.Type  GetUIScriptType(EnumUIType _uiType)
        {
            System.Type temp = null;
            switch (_uiType)
            {
                case EnumUIType.None:
                    break;
                case EnumUIType.TestOne:
                    temp = typeof(TestOne);
                    break;
                case EnumUIType.TestTwo:
                    temp = typeof(TestTwo);
                    break;
                default:
                    break;
            }
            return temp;
        }

    }
    public class Defines
    {

        public Defines()
        {

        }
    }
}

