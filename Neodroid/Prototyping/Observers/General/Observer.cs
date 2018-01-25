using System;
using System.Collections.Generic;
using Neodroid.Models.Environments;
using Neodroid.Scripts.Utilities;
using Neodroid.Scripts.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Observers.General {
  [ExecuteInEditMode]
  [Serializable]
  public class Observer : MonoBehaviour,
                          IHasFloatEnumarable {
    public PrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    public virtual string ObserverIdentifier { get { return this.name + "Observer"; } }

    public virtual IEnumerable<float> FloatEnumerable { get; protected set; }

    protected virtual void Awake () {
      this.Setup ();
    }

    protected virtual void Start () {
    }

    public void RefreshAwake () {
      this.Awake ();
    }

    public void RefreshStart () {
      this.Start ();
    }

    protected void Setup () {
      this.RegisterComponent ();
      this.FloatEnumerable = new float[]{ };
    }

    public virtual void RegisterComponent () {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent (this.ParentEnvironment, this);
    }

    public virtual void UpdateObservation () {
    }

    public virtual void Reset () {
    }

    #region Fields

    [Header ("References", order = 99)]
    [SerializeField]
    PrototypingEnvironment _environment;

    [Header ("Development", order = 100)]
    [SerializeField]
    bool _debugging;

    #endregion
  }
}
