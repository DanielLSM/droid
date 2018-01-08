using System;
using Neodroid.Models.Environments;
using Neodroid.Scripts.Utilities;
using UnityEngine;

namespace Neodroid.Models.Observers.General {
  [ExecuteInEditMode]
  [Serializable]
  public class Observer : MonoBehaviour {
    public LearningEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public virtual string ObserverIdentifier { get { return this.name + "Observer"; } }

    protected virtual void Awake() { this.Setup(); }

    protected virtual void Start() { }

    public void RefreshAwake() { this.Awake(); }

    public void RefreshStart() { this.Start(); }

    protected void Setup() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(this.ParentEnvironment, this);
    }

    public virtual void UpdateData() { }

    public virtual void Reset() { }

    #region Fields

    [Header("References", order = 99)]
    [SerializeField]
    LearningEnvironment _environment;

    [Header("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    //[Header ("General", order = 101)]

    #endregion
  }
}
