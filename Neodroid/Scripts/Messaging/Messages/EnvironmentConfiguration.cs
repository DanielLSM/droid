using System;

namespace Neodroid.Messaging.Messages {
  public class EnvironmentConfigurable {

    string _configurable_name;
    string _configurable_value;

    public EnvironmentConfigurable (string configurable_name, string configurable_value) {
      _configurable_name = configurable_name;
      _configurable_value = configurable_value;
    }

    public string ConfigurableName {
      get{ return _configurable_name; }
    }

    public string ConfigurableVale {
      get{ return _configurable_value; }
    }

    public override string ToString () {
      return "<EnvironmentConfigurable> " + _configurable_name + ", " + _configurable_value + " </EnvironmentConfigurable>";
    }
  }
}

