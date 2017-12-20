using System;

namespace Neodroid.Messaging.Messages {
  [Serializable]
  public class MotorMotion {
    string _actor_name;
    string _motor_name;
    float _strength;
    // Has a possible direction given by the sign of the float

    public MotorMotion (string actor_name, string motor_name, float strength) {
      _actor_name = actor_name;
      _motor_name = motor_name;
      _strength = strength;
    }

    public string GetActorName () {
      return _actor_name;
    }

    public string GetMotorName () {
      return _motor_name;
    }

    public float Strength {
      get { return _strength; }
    }

    public override string ToString () {
      return "<MotorMotion> " + _actor_name + ", " + _motor_name + ", " + _strength + " </MotorMotion>";
    }
  }
}
