using System;

namespace Neodroid.Scripts.Messaging.Messages {
  [Serializable]
  public class MotorMotion {
    readonly string _actor_name;
    readonly string _motor_name;

    // Has a possible direction given by the sign of the float

    public MotorMotion(string actor_name, string motor_name, float strength) {
      this._actor_name = actor_name;
      this._motor_name = motor_name;
      this.Strength = strength;
    }

    public float Strength { get; private set; }

    public string GetActorName() { return this._actor_name; }

    public string GetMotorName() { return this._motor_name; }

    public override string ToString() {
      return "<MotorMotion> "
             + this._actor_name
             + ", "
             + this._motor_name
             + ", "
             + this.Strength
             + " </MotorMotion>";
    }
  }
}
