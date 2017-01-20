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
        public class UIInfoData
        {
            public EnumUIType UIType
            {
                get;
                private set;
            }
            public string Path
            {
                get;
                private set;
            }
            public object[] UIParams
            {
                get;
                private set;
            }
            public Type ScriptType
            {

                get;
                private set;
            }
            public UIInfoData(EnumUIType Type, string path, params object[] UIData)
            {
                UIType = Type;
                Path = path;
                UIParams = UIData;
                this.ScriptType = UIPathDefines.GetUIScriptType(Type);

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
            if (!dicOpenUIs.TryGetValue(type, out temp))
            {
                throw new Exception("_dicOpen trygetvalue fail");
            }
            return temp;
        }
        public T GetUI<T>(EnumUIType type) where T : BaseUI
        {
            GameObject temp = GetUIGameObject(type);
            if (temp != null)
            {
                return temp.GetComponent<T>();
            }
            return null;
        }
        #region OpenUIMethed
        public void OpenUI(bool _isCloseOther, EnumUIType[] _UIType, params object[] _uiparams)
        {
            //close other ui 
            if (true == _isCloseOther)
            {
                CloseUIAll();
            }
            //push ui in stack
            for (int i = 0; i < _UIType.Length; i++)
            {
                if (!dicOpenUIs.ContainsKey(_UIType[i]))
                {
                    string path = UIPathDefines.GetPrefabsPathByType(_UIType[i]);
                    stackOpenUIs.Push(new UIInfoData(_UIType[i], path, _uiparams));
                }
            }

            //OpenUI 异步加载UI
            if (stackOpenUIs.Count > 0)
            {
                for (int i = 0; i < stackOpenUIs.Count; i++)
                {
                    CoroutineController.Instance.StartCoroutine(asyncLoad());
                }
            }
        }
        public void OpenUI(EnumUIType[] _uitype)
        {
            OpenUI(false, _uitype, null);
        }
        public void OpenUI(EnumUIType _uitype)
        {
            EnumUIType[] uitype = new EnumUIType[1] { _uitype };
            OpenUI(false, uitype, null);
        }
        public void OpenUI(EnumUIType[] _uitype, params object[] _uiparams)
        {
            OpenUI(false, _uitype, _uiparams);
        }
        public void OpenUI(EnumUIType _uitype, params object[] _uiparams)
        {
            EnumUIType[] uitype = new EnumUIType[1] { _uitype };
            OpenUI(false, uitype, _uiparams);
        }
        public void OpenUICloseOthers(EnumUIType _uitype)
        {
            EnumUIType[] uitype = new EnumUIType[1] { _uitype };
            OpenUI(true, uitype, null);
        }
        public void OpenUICloseOthers(EnumUIType _uitype, params object[] _uiparams)
        {
            EnumUIType[] uitype = new EnumUIType[1] { _uitype };
            OpenUI(true, uitype, _uiparams);
        }
        public void OpenUICloseOthers(EnumUIType[] _uitype)
        {
            OpenUI(true, _uitype, null);
        }
        public void OpenUICloseOther(EnumUIType[] _uitype, params object[] _uiparams)
        {
            OpenUI(true, _uitype, _uiparams);
        }
        #endregion
        #region CloseUIMethed
        /// <summary>
        /// Close the UI
        /// </summary>
        /// <param name="_uitype"></param>
        private void CloseUI(EnumUIType _uitype,GameObject _uiObject)
        {
            
            if (_uiObject == null)
            {
               
                GameObject.Destroy(_uiObject);
                dicOpenUIs.Remove(_uitype);


            }
            else
            {
                BaseUI _baseUI = _uiObject.GetComponent<BaseUI>();
                if (_baseUI == null)
                {
                   
                    GameObject.Destroy(_baseUI);
                    dicOpenUIs.Remove(_uitype);

                }
                else
                {
                    _baseUI.StateChange += CloseUIHandle;
                    _baseUI.Release();
                    
                    
                }
            }
        }
        public void CloseUI(EnumUIType _uitype)
        {
            GameObject temp = GetUIGameObject(_uitype);
            if (temp==null)
            {
                dicOpenUIs.Remove(_uitype);
            }
            else
            {
                CloseUI(_uitype, temp);
            }
        }
        public void CloseUIAll()
        {
            List<EnumUIType> _listKey = new List<EnumUIType>(dicOpenUIs.Keys);
            for (int i = 0; i < _listKey.Count; i++)
            {
                CloseUI(_listKey[i]);
            }
            dicOpenUIs.Clear();
        }
        public void CloseUI(EnumUIType[] _uiTypes)
        {
            for (int i = 0; i < _uiTypes.Length; i++)
            {
                CloseUI(_uiTypes[i]);
            }
        }
        private void CloseUIHandle(object sender, EnumObjectState oldState, EnumObjectState NewState)
        {
            //Debug.Log((sender as BaseUI).name);
            if (NewState == EnumObjectState.Closing)
            {
                BaseUI _baseUI = sender as BaseUI;
                dicOpenUIs.Remove(_baseUI.GetUIType());
              
                _baseUI.StateChange -= CloseUIHandle;
            }

        }
        #endregion

        /// <summary>
        /// async load UI data
        /// </summary>
        /// <returns></returns>
        private IEnumerator<int> asyncLoad()
        {
            
            UIInfoData _uiInfoData = null;
            UnityEngine.Object _prefab = null;
            GameObject _uiObject = null;
            if (stackOpenUIs != null && stackOpenUIs.Count > 0)
            {
                do
                {
                    _uiInfoData = stackOpenUIs.Pop();
                    //Debug.Log(_uiInfoData.Path);
                    _prefab = Resources.Load(_uiInfoData.Path);
                    if (_prefab != null)
                    {
                        //Debug.Log("LoadUI");
                        _uiObject = MonoBehaviour.Instantiate(_prefab) as GameObject;
                        BaseUI _baseUI = _uiObject.GetComponent<BaseUI>();
                        if (_baseUI != null)
                        {
                            _baseUI.SetUIWhenOpening();
                        }
                        else
                        {
                            Type temp = _uiInfoData.ScriptType;
                            _baseUI = _uiObject.AddComponent(_uiInfoData.ScriptType) as BaseUI;
                        }
                        dicOpenUIs.Add(_uiInfoData.UIType, _uiObject);
                    }
                } while (stackOpenUIs.Count > 0);
            }
            yield return 0;
        }
        /// <summary>
        /// preloads the UI
        /// </summary>
        /// <param name="_uitype"></param>
        public void PreloadUI(EnumUIType _uitype)
        {
            string path = UIPathDefines.GetPrefabsPathByType(_uitype);
            Resources.Load(path);
        }
        public void PreloadUI(EnumUIType[] _uitypes)
        {
            for (int i  = 0; i  <_uitypes.Length; i ++)
            {
                PreloadUI(_uitypes[i]);
            }
        }
    }
}
