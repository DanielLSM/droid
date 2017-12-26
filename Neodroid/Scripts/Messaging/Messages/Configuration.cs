

namespace Neodroid.Messaging.Messages
{
    public class Configuration
    {

        string _configurable_name;
        float _configurable_value;

        public Configuration(string configurable_name, float configurable_value)
        {
            _configurable_name = configurable_name;
            _configurable_value = configurable_value;
        }

        public string ConfigurableName {
            get { return _configurable_name; }
        }

        public float ConfigurableValue {
            get { return _configurable_value; }
        }

        public override string ToString()
        {
            return "<Configuration> " + _configurable_name + ", " + _configurable_value + " </Configuration>";
        }
    }
}

