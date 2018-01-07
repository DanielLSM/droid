

namespace Neodroid.Messaging.Messages {
  [System.Serializable]
  public class MotorMotion {
    private readonly string _actor_name;
    private readonly string _motor_name;

    // Has a possible direction given by the sign of the float

    public MotorMotion(string actor_name, string motor_name, float strength) {
      _actor_name = actor_name;
      _motor_name = motor_name;
      Strength = strength;
    }

    public float Strength { get; private set; }

    public string GetActorName() { return _actor_name; }

    public string GetMotorName() { return _motor_name; }

    public override string ToString() {
      return "<MotorMotion> " + _actor_name + ", " + _motor_name + ", " + Strength + " </MotorMotion>";
    }
  }
}
