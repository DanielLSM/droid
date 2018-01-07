using System.Collections;
using Assets.Neodroid.Models.Actors;
using Neodroid.Environments;
using Neodroid.Utilities;
using UnityEngine;

public class CollisionExplosion : Resetable {
  public Actor _actor;
  private GameObject _broken_object;
  public GameObject _broken_object_prefab;

  private float _delay = 2f;
  public LearningEnvironment _environment;
  public ParticleSystem _explosion;
  public float _explosion_force = 50;
  public GameObject _explosion_prefab;
  private bool _has_exploded;
  public Rigidbody _rigidbody;
  public float _threshold = 150;

  public bool Debugging;

  public override string ResetableIdentifier { get { return name; } }

  private void Start() {
    if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
    _explosion = GetComponent<ParticleSystem>();

    NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                  this,
                                                                  _rigidbody.transform,
                                                                  ChildOnCollisionEnter,
                                                                  ChildOnTriggerEnter,
                                                                  null,
                                                                  null,
                                                                  null,
                                                                  null,
                                                                  Debugging);
  }

  private void ChildOnCollisionEnter(GameObject child, Collision col) {
    if (Debugging)
      print("Collision");
    if (!col.collider.isTrigger)
      De(
         child.GetComponent<Rigidbody>(),
         col.collider.attachedRigidbody);
  }

  private void ChildOnTriggerEnter(GameObject child, Collider col) {
    if (Debugging)
      print("Trigger colliding");
    if (!col.isTrigger)
      De(
         child.GetComponent<Rigidbody>(),
         col.attachedRigidbody);
  }

  private void De(Rigidbody rb, Rigidbody other = null) {
    var val = 0f;
    if (rb != null) val = NeodroidUtilities.KineticEnergy(rb);
    var val_other = 0f;
    if (other != null) val_other = NeodroidUtilities.KineticEnergy(rb);
    if (Debugging)
      print(
            string.Format(
                          "{0} {1}",
                          val,
                          val_other));
    if ((val >= _threshold || val_other >= _threshold) && !_has_exploded) {
      _actor.Kill();
      _has_exploded = true;
      if (_explosion) {
        _explosion.Play();
        _delay = _explosion.main.duration;
      }

      StartCoroutine(
                     SpawnBroken(
                                 _delay,
                                 _rigidbody.transform.parent,
                                 _rigidbody.transform.position,
                                 _rigidbody.transform.rotation,
                                 _rigidbody.velocity,
                                 _rigidbody.angularVelocity)
                    );
      _rigidbody.gameObject.SetActive(false);
      _rigidbody.Sleep();
    }
  }

  public IEnumerator SpawnBroken(
    float wait_time,
    Transform parent,
    Vector3 pos,
    Quaternion rot,
    Vector3 vel,
    Vector3 ang) {
    var explosion = Instantiate(
                                _explosion_prefab,
                                pos,
                                rot,
                                parent);
    _broken_object = Instantiate(
                                 _broken_object_prefab,
                                 pos,
                                 rot,
                                 parent);
    var rbs = _broken_object.GetComponentsInChildren<Rigidbody>();
    foreach (var rb in rbs) {
      rb.velocity = vel;
      rb.angularVelocity = ang;
      rb.AddForceAtPosition(
                            (pos - rb.transform.position) * _explosion_force,
                            pos);
    }

    yield return new WaitForSeconds(wait_time);
    Destroy(explosion);
    _environment.Terminate("Actor exploded");
  }

  protected virtual void Awake() { RegisterComponent(); }

  public virtual void RegisterComponent() {
    _environment = NeodroidUtilities.MaybeRegisterComponent(
                                                            _environment,
                                                            (Resetable)this);
  }

  public override void Reset() {
    if (_broken_object)
      Destroy(_broken_object);
    if (_rigidbody) {
      _rigidbody.WakeUp();
      _rigidbody.gameObject.SetActive(true);
    }

    _has_exploded = false;
  }
}
