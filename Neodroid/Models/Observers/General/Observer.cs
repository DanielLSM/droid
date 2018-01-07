
using Neodroid.Environments;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Observers {
  [ExecuteInEditMode]
  [System.Serializable]
  public class Observer : MonoBehaviour {
    public LearningEnvironment ParentEnvironment {
      get { return _environment; }
      set { _environment = value; }
    }

    public bool Debugging { get { return _debugging; } set { _debugging = value; } }

    public virtual string ObserverIdentifier { get { return name + "Observer"; } }

    protected virtual void Awake() { Setup(); }

    protected virtual void Start() { }

    public void RefreshAwake() { Awake(); }

    public void RefreshStart() { Start(); }

    protected void Setup() {
      ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
                                                                   ParentEnvironment,
                                                                   this);
    }

    public virtual void UpdateData() { }

    public virtual void Reset() { }

    #region Fields

    [Header(
      "References",
      order = 99)]
    [SerializeField]
    private LearningEnvironment _environment;

    [Header(
      "Development",
      order = 100)]
    [SerializeField]
    private bool _debugging;

    //[Header ("General", order = 101)]

    #endregion
  }
}
