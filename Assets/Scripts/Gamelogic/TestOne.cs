﻿using UnityEngine;
using System.Collections;
using MyFrameWork;
using System;

public class TestOne :BaseUI  {
    public override EnumUIType GetUIType()
    {
        return EnumUIType.TestOne;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Open()
    {
        UIManager.Intance.OpenUICloseOthers(EnumUIType.TestTwo);
        Debug.Log("asd");
    }
}
