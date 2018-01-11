using System.Collections;
using Neodroid.Models.Actors;
using Neodroid.Models.Environments;
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

    void Start () {
      if (!this._rigidbody)
        this._rigidbody = this.GetComponent<Rigidbody> ();
      this._explosion = this.GetComponent<ParticleSystem> ();

      NeodroidUtilities.RegisterCollisionTriggerCallbacksOnChildren (
        this,
        this._rigidbody.transform,
        this.ChildOnCollisionEnter,
        this.ChildOnTriggerEnter,
        null,
        null,
        null,
        null,
        this.Debugging);
    }

    void ChildOnCollisionEnter (GameObject child, Collision col) {
      if (this.Debugging)
        print ("Collision");
      if (!col.collider.isTrigger)
        this.De (child.GetComponent<Rigidbody> (), col.collider.attachedRigidbody);
    }

    void ChildOnTriggerEnter (GameObject child, Collider col) {
      if (this.Debugging)
        print ("Trigger colliding");
      if (!col.isTrigger)
        this.De (child.GetComponent<Rigidbody> (), col.attachedRigidbody);
    }

    void De (Rigidbody rb, Rigidbody other = null) {
      var val = 0f;
      if (rb != null)
        val = NeodroidUtilities.KineticEnergy (rb);
      var val_other = 0f;
      if (other != null)
        val_other = NeodroidUtilities.KineticEnergy (rb);
      if (this.Debugging)
        print (string.Format ("{0} {1}", val, val_other));
      if ((val >= this._threshold || val_other >= this._threshold) && !this._has_exploded) {
        this._actor.Kill ();
        this._has_exploded = true;
        if (this._explosion) {
          this._explosion.Play ();
          this._delay = this._explosion.main.duration;
        }

        this.StartCoroutine (
          this.SpawnBroken (
            this._delay,
            this._rigidbody.transform.parent,
            this._rigidbody.transform.position,
            this._rigidbody.transform.rotation,
            this._rigidbody.velocity,
            this._rigidbody.angularVelocity));
        this._rigidbody.gameObject.SetActive (false);
        this._rigidbody.Sleep ();
      }
    }

    public IEnumerator SpawnBroken (
      float wait_time,
      Transform parent,
      Vector3 pos,
      Quaternion rot,
      Vector3 vel,
      Vector3 ang) {
      var explosion = Instantiate (this._explosion_prefab, pos, rot, parent);
      this._broken_object = Instantiate (this._broken_object_prefab, pos, rot, parent);
      var rbs = this._broken_object.GetComponentsInChildren<Rigidbody> ();
      foreach (var rb in rbs) {
        rb.velocity = vel;
        rb.angularVelocity = ang;
        rb.AddForceAtPosition ((pos - rb.transform.position) * this._explosion_force, pos);
      }

      yield return new WaitForSeconds (wait_time);
      Destroy (explosion);
      this._environment.Terminate ("Actor exploded");
    }

    protected virtual void Awake () {
      this.RegisterComponent ();
    }

    public virtual void RegisterComponent () {
      this._environment = NeodroidUtilities.MaybeRegisterComponent (this._environment, (Resetable)this);
    }

    public override void Reset () {
      if (this._broken_object)
        Destroy (this._broken_object);
      if (this._rigidbody) {
        this._rigidbody.WakeUp ();
        this._rigidbody.gameObject.SetActive (true);
      }

      this._has_exploded = false;
    }
  }
}
