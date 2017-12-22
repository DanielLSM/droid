using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class Observer : MonoBehaviour {

    public LearningEnvironment _environment;

    public bool _debug = false;
    public byte[] _data = new byte[] { };

    protected virtual void Awake () {
      Setup ();
    }

    protected virtual void Start () {
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

    public virtual string ObserverIdentifier { get { return name + "Observer"; } }

    public virtual void Reset () {

    }
  }
}
