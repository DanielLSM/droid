﻿using UnityEngine;

namespace SceneAssets.Satellite.Scripts {
  [RequireComponent( typeof(Rigidbody))]
  public class InitialForce : MonoBehaviour {
    [SerializeField] bool _on_awake = true;

    [SerializeField] Rigidbody _rb;
    [SerializeField]  bool _relative;
    [SerializeField]  bool _torque;

    [SerializeField] Vector3 _force;

    void ApplyInitialForce() {
      if (this._torque) {
        if (this._relative)
          this._rb.AddRelativeTorque(
                                     torque : this._force,
                                     mode : ForceMode.Impulse);
        else
          this._rb.AddTorque(
                             torque : this._force,
                             mode : ForceMode.Impulse);
      } else {
        if (this._relative)
          this._rb.AddRelativeForce(
                                    force : this._force,
                                    mode : ForceMode.Impulse);
        else
          this._rb.AddForce(
                            force : this._force,
                            mode : ForceMode.Impulse);
      }
    }

    void Awake() {
      this._rb = this.GetComponent<Rigidbody>();

      if (this._on_awake) this.ApplyInitialForce();
    }

    void Start() {
      if (!this._on_awake) this.ApplyInitialForce();
    }
  }
}
