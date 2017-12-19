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
    public byte[] _data = new byte[] { };

    protected virtual void Awake () {
      _has_configurable = false;
      _configurable_name = "";
      Setup ();
    }

    protected virtual void Start () {
    }

    public void SetHasConfigurable (bool val, string str) {
      _has_configurable = true;
      _configurable_name = str;
    }

    public void RefreshAwake () {
      Awake ();
    }

    public void RefreshStart () {
      Start ();
    }

    protected void Setup () {
      _environment = NeodroidUtilities.MaybeRegisterComponent (_environment, this);
    }

    public virtual void UpdateData () {
    }

    public virtual string GetObserverIdentifier () {
      return name + "Observer";
    }

    public virtual void Reset () {

    }
  }
}
