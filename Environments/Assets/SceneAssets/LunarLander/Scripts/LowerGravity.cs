﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerGravity : MonoBehaviour {

  // Use this for initialization
  void Start () {
    Physics.gravity = Vector3.down * 3.33f;
  }
	
  // Update is called once per frame
  void Update () {
		
  }
}
