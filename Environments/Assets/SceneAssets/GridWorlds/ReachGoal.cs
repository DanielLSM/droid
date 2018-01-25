using Neodroid.Models.Actors;
using Neodroid.Models.Environments;
using Neodroid.Models.Evaluation;
using Neodroid.Prototyping.Evaluation.General;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace SceneAssets.GridWorlds {
  public class ReachGoal : ObjectiveFunction {
    enum ActorOverlapping {
      InsideArea,
      OutsideArea
    }

    [SerializeField] Actor _actor;

    [SerializeField] EmptyCell _goal;

    [SerializeField] bool _based_on_tags = false;

    //Used for.. if outside playable area then reset
    [SerializeField] ActorOverlapping _overlapping = ActorOverlapping.OutsideArea;

    public override float InternalEvaluate () {
      var distance = Mathf.Abs (Vector3.Distance (this._goal.transform.position, this._actor.transform.position));
      
      if (this._overlapping == ActorOverlapping.InsideArea || distance < 0.5f) {
        this.ParentEnvironment.Terminate ("Inside goal area");
        return 1f;
      }
        
      return 0f;
    }

    public override void InternalReset () {
      this.Setup ();
      this._overlapping = ActorOverlapping.OutsideArea;
    }

    public void SetGoal (EmptyCell goal) {
      this._goal = goal;
      this.InternalReset ();
    }

    void Setup () {
      if (!this._goal)
        this._goal = FindObjectOfType<EmptyCell> ();
      if (!this._actor)
        this._actor = FindObjectOfType<Actor> ();
      if (this._goal)
        NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
          this,
          this._goal.transform,
          null,
          on_trigger_enter_child: this.OnTriggerEnterChild,
          debug: this.Debugging);
      if (this._actor)
        NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
          this,
          this._actor.transform,
          null,
          on_trigger_enter_child: this.OnTriggerEnterChild,
          debug: this.Debugging);
    }

    void OnTriggerEnterChild (GameObject child_game_object, Collider other_game_object) {
      print ("triggered");
      if (this._actor) {
        if (this._based_on_tags) {
          if (other_game_object.tag == this._actor.tag) {
            if (this.Debugging)
              Debug.Log ("Actor is inside area");
            this._overlapping = ActorOverlapping.InsideArea;
          }
        } else {
          if (child_game_object == this._goal.gameObject
              && other_game_object.gameObject == this._actor.gameObject) {
            if (this.Debugging)
              Debug.Log ("Actor is inside area");
            this._overlapping = ActorOverlapping.InsideArea;
          }
        }
      }
    }
  }
}
