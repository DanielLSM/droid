using Neodroid.Models.Actors;
using Neodroid.Models.Environments;
using Neodroid.Scripts.Utilities.BoundingBoxes;
using SceneAssets.ScripterGrasper.Scripts;
using UnityEngine;

namespace SceneAssets.GridWorlds {
  public class EnsureValidState : MonoBehaviour {
    [SerializeField] Transform _goal;

    [SerializeField] Actor _actor;

    [SerializeField] LearningEnvironment _environment;

    [SerializeField] BoundingBox _playable_area;

    [SerializeField] Obstruction[] _obstructions;

    void Awake () {
      if (!this._goal)
        this._goal = FindObjectOfType<Transform> ();
      if (!this._actor)
        this._actor = FindObjectOfType<Actor> ();
      if (!this._environment)
        this._environment = FindObjectOfType<LearningEnvironment> ();
      if (this._obstructions.Length <= 0)
        this._obstructions = FindObjectsOfType<Obstruction> ();
      if (!this._playable_area)
        this._playable_area = FindObjectOfType<BoundingBox> ();
    }
    
    void Update() { this.ValidateState(); }

    void ValidateState () {
      if (this._playable_area != null && !this._playable_area._bounds.Intersects (this._actor.ActorBounds))
        this._environment.Terminate ("Actor outside playable area");
      if (this._playable_area != null && !this._playable_area._bounds.Intersects (this._goal.GetComponent<Collider> ().bounds))
        this._environment.Terminate ("Goal outside playable area");

      foreach (var obstruction in this._obstructions) {
        if (obstruction != null
            && !obstruction.GetComponent<Collider> ().bounds.Intersects (this._actor.ActorBounds))
          this._environment.Terminate ("Actor overlapping obstruction");
        if (obstruction != null
            && !obstruction.GetComponent<Collider> ().bounds
                .Intersects (this._goal.GetComponent<Collider> ().bounds))
          this._environment.Terminate ("Goal overlapping obstruction");

      }
    }
  }
}
