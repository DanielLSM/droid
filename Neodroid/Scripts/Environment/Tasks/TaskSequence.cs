using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Neodroid.Task {
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
      _current_goal = _goal_stack.Pop ();
      return _current_goal;
    }
  }

}