using System;
using System.Collections.Generic;
using Neodroid.Models.Observers.NotUsed;
using Neodroid.Scripts.Utilities.ScriptableObjects;
using UnityEngine;

namespace Neodroid.Scripts.Utilities.Tasks {
  //[ExecuteInEditMode]
  public class TaskSequence : NeodroidTask {
    [SerializeField] GoalObserver _current_goal;
    public GoalObserver CurrentGoal { get { return this._current_goal; }
      private set { this._current_goal = value; }
    }

    [SerializeField]
    Stack<GoalObserver> _goal_stack;
    [SerializeField]
    GoalObserver[] _sequence;

    void Start() {
      if (this._sequence == null || this._sequence.Length == 0) {
        this._sequence = FindObjectsOfType<GoalObserver>();
        Array.Sort(
                   array : this._sequence,
                   comparison : (g1, g2) => g1.OrderIndex.CompareTo(value : g2.OrderIndex));
      }

      Array.Reverse(array : this._sequence);
      this._goal_stack = new Stack<GoalObserver>(collection : this._sequence);
      this.CurrentGoal = this.PopGoal();
    }

    void Update() { }

    public GoalObserver[] GetSequence() { return this._sequence; }

    public GoalObserver PopGoal() {
      this.CurrentGoal = this._goal_stack.Pop();
      return this.CurrentGoal;
    }
  }
}
