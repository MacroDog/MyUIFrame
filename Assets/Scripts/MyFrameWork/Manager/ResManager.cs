using UnityEngine;
using System.Collections;

namespace MyFrameWork
{
    public class ResManager : Singleton<ResManager>
    {

        public override void Init()
        {
            base.Init();
            Debug.Log("this ResManager init");
        }
    }
}

