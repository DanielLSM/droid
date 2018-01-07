namespace Neodroid.Messaging.Messages {
  public class Configuration {
    public Configuration(string configurable_name, float configurable_value) {
      ConfigurableName = configurable_name;
      ConfigurableValue = configurable_value;
    }

    public string ConfigurableName { get; private set; }

    public float ConfigurableValue { get; private set; }

    public override string ToString() {
      return "<Configuration> " + ConfigurableName + ", " + ConfigurableValue + " </Configuration>";
    }
  }
}
