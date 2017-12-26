using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Neodroid.Utilities;

namespace Neodroid.Task {

  [ExecuteInEditMode]
  public class TaskSequence : NeodroidTask {

    public GoalObserver[] _sequence;
    public GoalObserver _current_goal;
    public Stack<GoalObserver> _goal_stack;

    void Start () {
      if (_sequence == null || _sequence.Length == 0) {
        _sequence = FindObjectsOfType<GoalObserver> ();
        System.Array.Sort (_sequence, (g1, g2) => g1._order_index.CompareTo (g2._order_index));
      }

      System.Array.Reverse (_sequence);
      _goal_stack = new Stack<GoalObserver> (_sequence);
      _current_goal = PopGoal ();
    }

    void Update () {

    }

    public GoalObserver[] GetSequence () {
      return _sequence;
    }

    public GoalObserver PopGoal () {
      _current_goal = _goal_stack.Pop ();
      return _current_goal;
    }
  }

}