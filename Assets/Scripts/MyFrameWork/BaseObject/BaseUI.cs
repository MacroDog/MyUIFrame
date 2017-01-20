using UnityEngine;
using System.Collections;

namespace MyFrameWork
{
    public abstract class BaseUI : MonoBehaviour
    {
        #region cache UI GameObejct&&Transform
        //缓存当前UIGameObject
        private GameObject _cacheGameObject;
        public GameObject CacheGameObject
        {
            get
            {
                if (_cacheGameObject == null)
                    _cacheGameObject = this.gameObject;
                return _cacheGameObject;
            }
        }
        //缓存当前UITransform
        private Transform _cacheTransform;
        public Transform CacheTransform
        {
            get
            {
                if (_cacheTransform == null)
                {
                    _cacheTransform = this.transform;
                }
                return _cacheTransform;
            }
        }
            
        #endregion

        #region UI状态和UIType
        protected EnumObjectState _state = EnumObjectState.None;
        public event StateChangeEvent StateChange;
        public EnumObjectState State
        {
            protected get
            {
                return _state;
            }
            set
            {
                EnumObjectState oldState = this._state;
                _state -= value;
                if (StateChange != null)
                    StateChange(this, oldState, _state);
            }
        }
        public abstract EnumUIType GetUIType();
        #endregion
        void Awake()
        {
            _state = EnumObjectState.Initial;
            OnAwake();
        }
        // Use this for initialization
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            if (this._state == EnumObjectState.Ready)
            {
                OnUpdate(Time.deltaTime);
            }
        }

        //释放当前UI
        public void Release()
        {
            this._state = EnumObjectState.Closing;
            OnRelease();
            GameObject.Destroy(this.gameObject);

        }

        void Destory()
        {
            this._state = EnumObjectState.None;
        }
        protected virtual void OnStart()
        {

        }
        protected virtual void OnAwake()
        {
            this.State = EnumObjectState.None;
            this.OnPlayOpenUIAduio();
        }
        protected virtual void OnUpdate(float deltatime)
        {

        }
        protected virtual void OnRelease()
        {
            this.OnCloseUIAudio();
            this.State = EnumObjectState.None;

        }
        protected virtual void OnDestroy()
        {
            this.State = EnumObjectState.None;
        }
        protected virtual void OnLoadData()
        {

        }
        //播放打开界面音效
        protected virtual void OnPlayOpenUIAduio()
        {

        }
        protected virtual void OnCloseUIAudio()
        {

        }
        protected virtual void SetUI(params object[] UIParams)
        {
            this.State = EnumObjectState.Loading;
        }
        //打开UI时自动设置 加载数据
        public void SetUIWhenOpening(params object[] uiparams)
        {
            SetUI(uiparams);
            StartCoroutine(loadDataAsyn());
        }

        /// <summary>
        /// 异步加载UI数据
        /// </summary>
        /// <returns></returns>
        private IEnumerator loadDataAsyn()
        {
            yield return new WaitForSeconds(0);
            if (this.State == EnumObjectState.Loading)
            {
                this.OnLoadData();
                this.State = EnumObjectState.Ready;
            }
        }

    }
}

