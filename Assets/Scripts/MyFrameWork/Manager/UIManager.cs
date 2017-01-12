using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MyFrameWork
{
    /// <summary>
    /// user Interface Manager
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// user interface Data
        /// </summary>
        class UIInfoData
        {
            public EnumUIType _UIType
            {
                get;
                private set;
            }
            public string _path
            {
                get;
                private set;
            }
            public object[] _UIParams
            {
                get;
                private set;
            }
            UIInfoData(EnumUIType Type, string path, params object[] UIData)
            {
                _UIType = Type;
                _path = path;
                _UIParams = UIData;

            }
        }
        //已经打开的UI
        private Dictionary<EnumUIType, GameObject> dicOpenUIs = null;
        //将要打开的UI
        private Stack<UIInfoData> stackOpenUIs;
        public override void Init()
        {
            base.Init();
            dicOpenUIs = new Dictionary<EnumUIType, GameObject>();
            stackOpenUIs = new Stack<UIInfoData>();
            Debug.Log("UIManger:Singleton<UIManage>Init");
        }
        public GameObject GetUIGameObject(EnumUIType type)
        {
            GameObject temp = null;
            if (!dicOpenUIs.TryGetValue(type,out temp))
            {
                throw new Exception("_dicOpen trygetvalue fail");
            }
            return temp;
            
        }
        public T GetUI<T>(EnumUIType type)where T : BaseUI
        {
            GameObject temp = GetUIGameObject(type);
            if (temp!=null)
            {
                return temp.GetComponent<T>();
            }
            return null;
        }
    }
}

