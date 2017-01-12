
using UnityEngine;
using System.Collections;
namespace MyFrameWork
{
    /// <summary>
    /// 单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : class, new()
    {
        protected static T _instence = null;
        public static T Intance
        {
            get
            {
                if (_instence == null)
                {
                    _instence = new T();
                }
                return _instence;
            }
        }
        protected Singleton()
        {
            if (null != _instence)//未初始化完成应该为空
                throw new SingletonException("this."+typeof(T).ToString()); 
                Init();
        }
        public virtual void Init()
        {
        }
        
    }
}

