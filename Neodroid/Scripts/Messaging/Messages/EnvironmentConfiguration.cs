using System;

namespace Neodroid.Messaging.Messages {
  public class Configuration {

    string _configurable_name;
    string _configurable_value;

    public Configuration (string configurable_name, string configurable_value) {
      _configurable_name = configurable_name;
      _configurable_value = configurable_value;
    }

    public string ConfigurableName {
      get{ return _configurable_name; }
    }

    public string ConfigurableValue {
      get{ return _configurable_value; }
    }

    public override string ToString () {
      return "<Configuration> " + _configurable_name + ", " + _configurable_value + " </Configuration>";
    }
  }
}

