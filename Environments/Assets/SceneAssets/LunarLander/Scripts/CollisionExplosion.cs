using System.Collections;
using Neodroid.Environments;
using Neodroid.Models.Actors;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace SceneAssets.LunarLander.Scripts {
  public class CollisionExplosion : Resetable {
    public Actor _actor;
    GameObject _broken_object;
    public GameObject _broken_object_prefab;

    float _delay = 2f;
    public LearningEnvironment _environment;
    public ParticleSystem _explosion;
    public float _explosion_force = 50;
    public GameObject _explosion_prefab;
    bool _has_exploded;
    public Rigidbody _rigidbody;
    public float _threshold = 150;

    public bool Debugging;

    public override string ResetableIdentifier { get { return this.name; } }

    void Start() {
      if (!this._rigidbody) this._rigidbody = this.GetComponent<Rigidbody>();
      this._explosion = this.GetComponent<ParticleSystem>();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren(
                                                                    caller : this,
                                                                    parent : this._rigidbody.transform,
                                                                    on_collision_enter_child : this
                                                                      .ChildOnCollisionEnter,
                                                                    on_trigger_enter_child : this
                                                                      .ChildOnTriggerEnter,
                                                                    on_collision_exit_child : null,
                                                                    on_trigger_exit_child : null,
                                                                    on_collision_stay_child : null,
                                                                    on_trigger_stay_child : null,
                                                                    debug : this.Debugging);
    }

    void ChildOnCollisionEnter(GameObject child, Collision col) {
      if (this.Debugging)
        print(message : "Collision");
      if (!col.collider.isTrigger)
        this.De(
                rb : child.GetComponent<Rigidbody>(),
                other : col.collider.attachedRigidbody);
    }

    void ChildOnTriggerEnter(GameObject child, Collider col) {
      if (this.Debugging)
        print(message : "Trigger colliding");
      if (!col.isTrigger)
        this.De(
                rb : child.GetComponent<Rigidbody>(),
                other : col.attachedRigidbody);
    }

    void De(Rigidbody rb, Rigidbody other = null) {
      var val = 0f;
      if (rb != null) val = NeodroidUtilities.KineticEnergy(rb : rb);
      var val_other = 0f;
      if (other != null) val_other = NeodroidUtilities.KineticEnergy(rb : rb);
      if (this.Debugging)
        print(
              message : string.Format(
                                      format : "{0} {1}",
                                      arg0 : val,
                                      arg1 : val_other));
      if ((val >= this._threshold || val_other >= this._threshold) && !this._has_exploded) {
        this._actor.Kill();
        this._has_exploded = true;
        if (this._explosion) {
          this._explosion.Play();
          this._delay = this._explosion.main.duration;
        }

        this.StartCoroutine(
                            routine : this.SpawnBroken(
                                                       wait_time : this._delay,
                                                       parent : this._rigidbody.transform.parent,
                                                       pos : this._rigidbody.transform.position,
                                                       rot : this._rigidbody.transform.rotation,
                                                       vel : this._rigidbody.velocity,
                                                       ang : this._rigidbody.angularVelocity)
                           );
        this._rigidbody.gameObject.SetActive(value : false);
        this._rigidbody.Sleep();
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
                                  original : this._explosion_prefab,
                                  position : pos,
                                  rotation : rot,
                                  parent : parent);
      this._broken_object = Instantiate(
                                        original : this._broken_object_prefab,
                                        position : pos,
                                        rotation : rot,
                                        parent : parent);
      var rbs = this._broken_object.GetComponentsInChildren<Rigidbody>();
      foreach (var rb in rbs) {
        rb.velocity = vel;
        rb.angularVelocity = ang;
        rb.AddForceAtPosition(
                              force : (pos - rb.transform.position) * this._explosion_force,
                              position : pos);
      }

      yield return new WaitForSeconds(seconds : wait_time);
      Destroy(obj : explosion);
      this._environment.Terminate(reason : "Actor exploded");
    }

    protected virtual void Awake() { this.RegisterComponent(); }

    public virtual void RegisterComponent() {
      this._environment = NeodroidUtilities.MaybeRegisterComponent(
                                                                   r : this._environment,
                                                                   c : (Resetable)this);
    }

    public override void Reset() {
      if (this._broken_object)
        Destroy(obj : this._broken_object);
      if (this._rigidbody) {
        this._rigidbody.WakeUp();
        this._rigidbody.gameObject.SetActive(value : true);
      }

      this._has_exploded = false;
    }
  }
}
