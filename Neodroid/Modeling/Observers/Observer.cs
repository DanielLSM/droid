using UnityEngine;
using Neodroid.Utilities;
using Neodroid.Environments;
using Neodroid.Managers;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class Observer : MonoBehaviour {
    #region Fields

    [Header ("References", order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header ("Development", order = 100)]
    [SerializeField]
    bool _debugging = false;

    //[Header ("General", order = 101)]



    #endregion

    public LearningEnvironment ParentEnvironment {
      get {
        return _environment;
      }
      set {
        _environment = value;
      }
    }

    public bool Debugging {
      get {
        return _debugging;
      }
      set {
        _debugging = value;
      }
    }



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
      ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent (ParentEnvironment, this);
    }

    public virtual void UpdateData () {
    }

    public virtual string ObserverIdentifier { get { return name + "Observer"; } }

    public virtual void Reset () {

    }
  }
}
