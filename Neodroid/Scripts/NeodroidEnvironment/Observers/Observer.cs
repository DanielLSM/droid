using UnityEngine;
using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Agents;
using System;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.NeodroidEnvironment.Observers {
  [Serializable]
  public class Observer : MonoBehaviour {

    public NeodroidAgent _agent_game_object;
    public EnvironmentManager _environment_manager;

    public Vector3 _position;
    public Vector3 _rotation;
    public Vector3 _direction;

    //public Quaternion _rotation;
    //public Quaternion _direction;

    public bool _debug = false;
    public string _observer_identifier = "";
    public byte[] _data;

    protected virtual void Start () {
      Setup ();
      AddToAgent ();
      UpdatePosRotDir ();
    }

    protected virtual void Setup () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
    }

    protected void AddToAgent () {
      NeodroidUtilities.MaybeRegisterComponent (_agent_game_object, this);
    }

    public virtual byte[] GetData () { 
      if (_data != null)
        return _data;
      else
        return new byte[]{ };
    }



    public virtual string GetObserverIdentifier () {
      return name + "BaseObserver";
    }


    void UpdatePosRotDir () {
      if (_environment_manager) {
        _position = _environment_manager.TransformPosition (this.transform.position);
        _direction = _environment_manager.TransformDirection (this.transform.forward);
        _rotation = _environment_manager.TransformDirection (this.transform.up);
      } else {
        _position = this.transform.position;
        _direction = this.transform.forward;
        _rotation = this.transform.up;
      }
    }

    private void Update () {
      UpdatePosRotDir ();
    }

    public virtual void Reset () {

    }
  }
}
