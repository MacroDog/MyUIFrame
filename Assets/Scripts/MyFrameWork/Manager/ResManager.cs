using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MyFrameWork
{

    /// <summary>
    /// 资源类
    /// </summary>
    public class AssetInfo
    {
        private UnityEngine.Object _object;
        public Type AssetType{get;set;}
        public string Path { get; set; }
        public int RefCount { get; set; }
        public UnityEngine.Object AssetObject
        {
            get
            {
                if (_object == null)
                {
                    _resourceLoad();

                }
                return _object;
            }
        }
        public IEnumerator GetCoroutioneObject(Action<UnityEngine.Object> _loaded)
        {
            while (true)
            {
                if (_object != null)
                {
                    if (_loaded!=null)
                    {
                        _loaded(_object);
                    }

                }
                else
                {
                    yield return null;
                    _resourceLoad();
                    yield return null;
                }
                yield break;
            }
        } 
       
        public IEnumerator GetAsyncloadObject(Action<UnityEngine.Object> _loaded) 
        {
           return  GetAsynLoadObject(_loaded, null);
        }
        public IEnumerator GetAsynLoadObject(Action<UnityEngine.Object>_loaded,Action <float> _progress)
        {
         

            if (null!=_object)
            {
                _loaded(_object);
                yield break;
            }
            ResourceRequest _resResquest = Resources.LoadAsync(Path);

            while (_resResquest.progress <= 0.9)
            {
                if (null != _resResquest)
                {
                    _progress(_resResquest.progress);
                }
                yield return null;
            }
            while(!_resResquest.isDone)
            {
                if (_progress != null)
                {
                    _progress(_resResquest.progress);
                }
                yield return null;
            }
            _object = _resResquest.asset;
            if (_loaded!=null)
            {
                _loaded(_object);
            }
            yield return _resResquest;
        }
        private void _resourceLoad()
        {
            try
            {
                _object = Resources.Load(Path);
                if (!_object)
                {
                    Debug.Log("Resource Load Failure: Path:" + Path);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
    /// <summary>
    /// 资源加载类用于UI资源加载
    /// </summary>
    public class ResManager : Singleton<ResManager>
    {
        
        
        private Dictionary<string, AssetInfo> dicAseetInfo = null;

        public override void Init()
        {
            dicAseetInfo = new Dictionary<string, AssetInfo>();
            //Resources.Load();
            //Resources.LoadAsync();
        }
        #region Load Resources
        public UnityEngine.Object Load(string _path)
        {
           AssetInfo _assetInfo = getAssetInfo(_path);
            if (_assetInfo!=null)
            {
                return _assetInfo.AssetObject;
            }
            return null;
        }

        #endregion
        #region Load Coroutine Resoruces
        /// <summary>
        /// Loads the Coroutine.
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_loaded"></param>
        public void LoadCoroutine(string _path,Action <UnityEngine .Object> _loaded)
        {
            AssetInfo _assetInfo = getAssetInfo(_path, _loaded);
            if (_assetInfo != null)
            {
                CoroutineController.Instance.StartCoroutine(_assetInfo.GetCoroutioneObject(_loaded));
            }
        }
        #endregion
        #region Load Async Resources
        public void LoadAsync(string _path,Action <UnityEngine.Object >_loaded)
        {
            LoadAsync(_path, _loaded, null);
        }
        public void LoadAsync(string _path,Action <UnityEngine .Object> _loaded,Action<float> _progress)
        {
            AssetInfo _assetInfo = getAssetInfo(_path, _loaded);
            if (_assetInfo!=null)
            {
                CoroutineController.Instance.StartCoroutine(_assetInfo.GetAsynLoadObject(_loaded, _progress));
            }
        }
        #endregion 
        #region Load Resources & Instantiate Object
        public UnityEngine.Object LoadInstance(string _path)
        {
            UnityEngine.Object _obj= Load(_path);
            return instentiate(_obj);

        }
      
        public void LoadCoroutineInstence(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            LoadCoroutine(_path, _obj=> { instentiate(_obj, _loaded); });
        }
        public void LoadAsyncInstance(string _path,Action<UnityEngine.Object> _loaded)
        {
            LoadAsync(_path, _loaded);
        }
        #endregion
        #region get AssetInfo&Instantiate object
        /// <summary>
        /// try get AssetInfo by Asset path
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        private AssetInfo getAssetInfo(string _path)
        {
            return getAssetInfo(_path, null);
        }
        private AssetInfo getAssetInfo(string _path, Action<UnityEngine.Object> _loaded)
        {
            if (string.IsNullOrEmpty(_path))
            {
                Debug.LogError("Error:null _path name.");
                if (_loaded == null)
                {
                    _loaded(null);
                }
            }
            AssetInfo _assetInfo = null;
            if (!dicAseetInfo.TryGetValue(_path, out _assetInfo))//
            {
                _assetInfo = new AssetInfo();
                _assetInfo.Path = _path;
                dicAseetInfo.Add(_path, _assetInfo);
            }
            _assetInfo.RefCount++;
            return _assetInfo;
        }
        private UnityEngine.Object instentiate(UnityEngine.Object _obj)
        {
            return instentiate(_obj, null);
        }
        private UnityEngine.Object instentiate(UnityEngine.Object _obj, Action<UnityEngine.Object> _loaded)
        {
            UnityEngine.Object _retObj = null;

            if (_obj != null)
            {
                _retObj = MonoBehaviour.Instantiate(_obj);
                if (_retObj != null)
                {
                    if (_loaded != null)
                    {
                        _loaded(_retObj);
                        return null;
                    }
                }
                else
                {
                    Debug.LogError("Error: null Instenoate _retobj.");
                }
            }
            else
            {
                Debug.LogError("Error:null Resources Load return _obj");
            }
            return null;
        }
        #endregion




    }
}

