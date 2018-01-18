using System;
using Neodroid.Scripts.Messaging.Messages;
using Neodroid.Scripts.Utilities.Enums;
using UnityEngine;

namespace Neodroid.Models.Motors.Particles {
  [RequireComponent(typeof(ParticleSystem))]
  [RequireComponent(typeof(Rigidbody))]
  public class RocketMotor : RigidbodyMotor {
    [SerializeField] bool _fired_this_step;

    [SerializeField] protected ParticleSystem _particle_system;

    public override String MotorIdentifier { get { return this.name + "Rocket" + this._axis_of_motion; } }

    protected override void Start() {
      this._rigidbody = this.GetComponent<Rigidbody>();
      this._particle_system = this.GetComponent<ParticleSystem>();
      var valid_input = this.MotionValueSpace;
      valid_input.MinValue = 0;
      this.MotionValueSpace = valid_input;
      this.RegisterComponent();
    }

    void LateUpdate() {
      if (this._fired_this_step) {
        if (!this._particle_system.isPlaying)
          this._particle_system.Play(true);
      } else
        this._particle_system.Stop(true);

      this._fired_this_step = false;
    }

    protected override void InnerApplyMotion(MotorMotion motion) {
      if (motion.Strength < this.MotionValueSpace.MinValue
          || motion.Strength > this.MotionValueSpace.MaxValue) {
        print(
            string.Format(
                "It does not accept input {0}, outside allowed range {1} to {2}",
                motion.Strength,
                this.MotionValueSpace.MinValue,
                this.MotionValueSpace.MaxValue));
        return; // Do nothing
      }

      switch (this._axis_of_motion) {
        case Axis.X:
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
          break;
        case Axis.Y:
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.Z:
          if (this._relative_to == Space.World)
            this._rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            this._rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          break;
        case Axis.RotX:
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
          break;
        case Axis.RotY:
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
          break;
        case Axis.RotZ:
          if (this._relative_to == Space.World)
            this._rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            this._rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
          break;
        default:
          break;
      }

      this._fired_this_step = true;
    }
  }
}
