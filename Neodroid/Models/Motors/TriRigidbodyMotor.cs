using Neodroid.Messaging.Messages;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Motors {
  [RequireComponent(typeof(Rigidbody))]
  public class TriRigidbodyMotor : Motor {
    [SerializeField]
    protected Space _relative_to = Space.Self;

    [SerializeField]
    protected Rigidbody _rigidbody;

    [SerializeField]
    protected bool _rotational_motors;

    private string _X;
    private string _Y;
    private string _Z;

    protected override void Start() { _rigidbody = GetComponent<Rigidbody>(); }

    public override void RegisterComponent() {
      _X = GetMotorIdentifier() + "X";
      _Y = GetMotorIdentifier() + "Y";
      _Z = GetMotorIdentifier() + "Z";
      if (_rotational_motors) {
        _X = GetMotorIdentifier() + "RotX";
        _Y = GetMotorIdentifier() + "RotY";
        _Z = GetMotorIdentifier() + "RotZ";
      }

      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _X);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _Y);
      ParentActor = NeodroidUtilities.MaybeRegisterNamedComponent(
                                                                  ParentActor,
                                                                  (Motor)this,
                                                                  _Z);
    }

    public override string GetMotorIdentifier() { return name + "Rigidbody"; }

    public override void InnerApplyMotion(MotorMotion motion) {
      if (!_rotational_motors) {
        if (motion.GetMotorName() == _X)
          if (_relative_to == Space.World)
            _rigidbody.AddForce(Vector3.left * motion.Strength);
          else
            _rigidbody.AddRelativeForce(Vector3.left * motion.Strength);
        else if (motion.GetMotorName() == _Y)
          if (_relative_to == Space.World)
            _rigidbody.AddForce(Vector3.up * motion.Strength);
          else
            _rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
        else if (motion.GetMotorName() == _Z)
          if (_relative_to == Space.World)
            _rigidbody.AddForce(Vector3.forward * motion.Strength);
          else
            _rigidbody.AddRelativeForce(Vector3.forward * motion.Strength);
      } else {
        if (motion.GetMotorName() == _X)
          if (_relative_to == Space.World)
            _rigidbody.AddTorque(Vector3.left * motion.Strength);
          else
            _rigidbody.AddRelativeTorque(Vector3.left * motion.Strength);
        else if (motion.GetMotorName() == _Y)
          if (_relative_to == Space.World)
            _rigidbody.AddTorque(Vector3.up * motion.Strength);
          else
            _rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
        else if (motion.GetMotorName() == _Z)
          if (_relative_to == Space.World)
            _rigidbody.AddTorque(Vector3.forward * motion.Strength);
          else
            _rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
      }
    }
  }
}
