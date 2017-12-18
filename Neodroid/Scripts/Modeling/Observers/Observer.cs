using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class Observer : MonoBehaviour {

    public LearningEnvironment _environment;

    public bool _has_configurable = false;
    public string _configurable_name = "";
    public bool _debug = false;
    public string _observer_identifier = "";
    public byte[] _data;

    void Awake () {
      Setup ();
    }

    public void SetHasConfigurable (bool val, string str) {
      _has_configurable = true;
      _configurable_name = str;
    }

    public void Refresh () {
      Awake ();
    }

    protected void Setup () {
      _environment = NeodroidUtilities.MaybeRegisterComponent (_environment, this);
    }

    public virtual byte[] GetData () {
      if (_data != null)
        return _data;
      else
        return new byte[] { };
    }

    public virtual string GetObserverIdentifier () {
      return name + "Observer";
    }

    public virtual void Reset () {

    }
  }
}
