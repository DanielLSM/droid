using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Agents;
using Neodroid.Managers;
using Neodroid.Messaging.Messages;
using Neodroid.Utilities;

namespace Neodroid.Configurations {
  //[Serializable]
  public class Configurable : MonoBehaviour {

    //public NeodroidAgent _agent_game_object;
    public EnvironmentManager _environment_manager;

    public Vector3 _position;
    public Vector3 _rotation;
    public Vector3 _direction;

    protected virtual void Start () {
      Setup ();
      AddToEnvironment ();
      UpdatePosRotDir ();
    }

    protected virtual void Setup () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
    }

    protected void AddToEnvironment () {
      NeodroidUtilities.MaybeRegisterComponent (_environment_manager, this);
    }


    public virtual void Configure (Configuration configuration) {
    }

    public virtual void Configure (string configuration) {
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