using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Evaluation {
  [RequireComponent(typeof(Rigidbody))]
  public class PunishmentFunction : ObjectiveFunction {
    [SerializeField]
    LayerMask _layer_mask;
    [SerializeField]
    GameObject _player;
    int _hits;

    // Use this for initialization
    private void Start() {
      ResetHits();
      var balls = GameObject.FindGameObjectsWithTag("balls");

      foreach (var ball in balls)
        ball.AddComponent<ChildCollisionPublisher>().CollisionDelegate = OnChildCollision;
    }

    private void OnChildCollision(Collision collision) {
      if (collision.collider.name == _player.name)
        _hits += 1;

      if (true) Debug.Log(_hits);
    }

    private void ResetHits() { _hits = 0; }

    public override float InternalEvaluate() { return _hits * -1f; }
  }
}
