using UnityEngine;
using System.Collections;
using MyFrameWork;
public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //ResManager.Intance;
        //UIManager.Intance; ;
        GameObject go = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/TestOne"));
        TestOne tt = go.GetComponent<TestOne>();
        if (null ==tt)
        {
            tt = go.AddComponent<TestOne>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
