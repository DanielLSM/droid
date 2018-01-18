namespace Neodroid.Scripts.Messaging.Messages {
  public class Configuration {
    public Configuration(string configurable_name, float configurable_value) {
      this.ConfigurableName = configurable_name;
      this.ConfigurableValue = configurable_value;
    }

    public string ConfigurableName { get; private set; }

    public float ConfigurableValue { get; private set; }

    public override string ToString() {
      return "<Configuration> " + this.ConfigurableName + ", " + this.ConfigurableValue + " </Configuration>";
    }
  }
}
