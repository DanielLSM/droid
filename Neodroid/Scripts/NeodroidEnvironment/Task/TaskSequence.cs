using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Neodroid.NeodroidEnvironment.Task {
  public class TaskSequence : NeodroidTask {

    public Transform[] _sequence;
    public Transform _current_goal;
    public Stack<Transform> _goal_stack;

    void Start () {
      Array.Reverse (_sequence);
      _goal_stack = new Stack<Transform> (_sequence);
      _current_goal = PopGoal ();
    }

    void Update () {
	  	
    }

    public Transform PopGoal () {
      return _goal_stack.Pop ();
    }
  }

}